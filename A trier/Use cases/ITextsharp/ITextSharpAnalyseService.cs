using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

using Editique.Composition.Domain.Analyses;
using Editique.Composition.Domain.Scenarios;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Filter;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using System.Dynamic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;

namespace Editique.Composition.Infrastructure.Analyses
{

    public class Globals
    {
        public string Input { get; set; }

        public dynamic Context { get; set; }
    }


    public class ITextSharpAnalyseService : IAnalyseService
    {

        private const string GROUPE = "_Groupe";
        private const string PAGE_INDEX = "_PageIndex";
        private string CompositionInputPath { get; set; }

        private IAnalyseRepository _analyseRepository;

        static ITextSharpAnalyseService()
        {
            //Warm up
            var script = CSharpScript.Create("string dummy = \"start\";", globalsType: typeof(Globals),
                       options: ScriptOptions.Default.WithImports("System.Dynamic")
                       .AddReferences(
                           Assembly.GetAssembly(typeof(System.Dynamic.DynamicObject)),  // System.Code
                           Assembly.GetAssembly(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo)),  // Microsoft.CSharp
                           Assembly.GetAssembly(typeof(System.Dynamic.ExpandoObject))  // System.Dynamic
                           )
                    ).RunAsync(new Globals { });
        }

        public ITextSharpAnalyseService(string compositionInputPath, IAnalyseRepository analyseRepository)
        {
            CompositionInputPath = compositionInputPath;
            _analyseRepository = analyseRepository;
        }

        public string LancerAnalyse(AnalyseurDocument analyseurDocument, ExecutionScenario executionScenario)
        {
            var savedFileName = CompositionInputPath + @"/" + executionScenario.ClientId + @"/Input/" + executionScenario.DepotId;

            using (var wsStorage = new WsStorage.DataTransfertSoapClient())
            {
                var data = wsStorage.GetDocument(WsStorage.TypeBucket.CompositionEditique, savedFileName);
                if (data != null && data.Length > 0)
                {
                    List<dynamic> contextes = new List<dynamic>();
                    using (MemoryStream msReader = new MemoryStream(data))
                    {
                        using (PdfDocument document = new PdfDocument(new PdfReader(msReader)))
                        {
                            CompilerScript(analyseurDocument);

                            for (int pageIndex = 1; pageIndex <= document.GetNumberOfPages(); pageIndex++)
                            {
                                var page = document.GetPage(pageIndex);

                                dynamic context = new ExpandoObject();
                                context._PageIndex = pageIndex;

                                analyseurDocument.Rectangles.ForEach(r => PerformAction(r, page, context));

                                contextes.Add(context);
                            }
                        }
                    }

                    var analyseFichier = ConsoliderAnalyse(contextes, executionScenario);

                    _analyseRepository.Save(analyseFichier);

                    return analyseFichier.Id;
                }

                return null;
            }

        }

        private AnalyseFichier ConsoliderAnalyse(List<dynamic> contextes, ExecutionScenario executionScenario)
        {
            var dictionnaries = contextes.Cast<IDictionary<string, object>>().ToList();

            if (!dictionnaries.Any(d => d.ContainsKey("Groupe")))
            {
                dictionnaries[0]["Groupe"] = true;
            }

            //Regrouper
            int groupCount = 0;
            int indexDebutGroupe = 0;
            int nbPages = dictionnaries.Count;
            List<Groupe> groupes = new List<Groupe>();

            for (int i = 0; i < nbPages; i++)
            {
                var dictionnary = dictionnaries[i];
                if (dictionnary.ContainsKey(GROUPE) && (bool)dictionnary[GROUPE] == true)
                {
                    if (groupCount > 0)
                    {
                        var groupe = Regrouper(indexDebutGroupe, i, groupCount, dictionnaries);

                        groupes.Add(groupe);
                    }
                    indexDebutGroupe = i;
                    groupCount += 1;
                }
            }
            //Dernier groupe
            var dernierGroupe = Regrouper(indexDebutGroupe, nbPages, groupCount, dictionnaries);
            groupes.Add(dernierGroupe);

            AnalyseFichier analyseFichier = new AnalyseFichier(Guid.NewGuid().ToString(), executionScenario.Id, groupes);
            return analyseFichier;
        }

        private Groupe Regrouper(int indexDebutGroupe, int indexFinGroupe, int indexGroupe, List<IDictionary<string, object>> dictionnaries)
        {
            dynamic merged = new ExpandoObject();

            for (int i = indexDebutGroupe; i < indexFinGroupe; i++)
            {
                merged = Merge(merged, dictionnaries[i]);
            }

            var groupe = new Groupe(indexGroupe, indexDebutGroupe + 1, indexFinGroupe - indexDebutGroupe, merged);

            return groupe;
        }

        private static dynamic Merge(dynamic item1, dynamic item2)
        {
            var merged = (new ExpandoObject()) as IDictionary<string, object>;

            IDictionary<string, object> dictionary1 = item1 as IDictionary<string, object>;
            IDictionary<string, object> dictionary2 = item2 as IDictionary<string, object>;

            foreach (var pair in dictionary1.Concat(dictionary2))
            {
                merged[pair.Key] = pair.Value;
            }

            if (merged.ContainsKey(PAGE_INDEX))
            {
                merged.Remove(PAGE_INDEX);
            }

            if (merged.ContainsKey(GROUPE))
            {
                merged.Remove(GROUPE);
            }

            return merged;
        }

        private void CompilerScript(AnalyseurDocument analyseurDocument)
        {
            analyseurDocument.Rectangles.ToList().ForEach(r => CompilerRectangle(r));
        }

        private void CompilerRectangle(AnalyseRectangle rectangle)
        {
            if (rectangle.ConditionCode != null)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                rectangle.ConditionDelegate = new FunctionDelegate<object>(
                    CSharpScript.Create(rectangle.ConditionCode, globalsType: typeof(Globals)).CreateDelegate());

                sw.Stop();
                Debug.WriteLine(rectangle.ConditionCode + " : " + sw.ElapsedMilliseconds);
            }

            if (rectangle.FonctionCode != null)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                rectangle.FunctionDelegate = new FunctionDelegate<object>(
                    CSharpScript.Create(rectangle.FonctionCode, globalsType: typeof(Globals),
                       options: ScriptOptions.Default.WithImports("System.Dynamic")
                       .AddReferences(
                           Assembly.GetAssembly(typeof(System.Dynamic.DynamicObject)),  // System.Code
                           Assembly.GetAssembly(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo)),  // Microsoft.CSharp
                           Assembly.GetAssembly(typeof(System.Dynamic.ExpandoObject))  // System.Dynamic
                           )
                    ).CreateDelegate());

                sw.Stop();
                Debug.WriteLine(rectangle.FonctionCode + " : " + sw.ElapsedMilliseconds);
            }

            if (rectangle.RectangleEnfants != null)
            {
                rectangle.RectangleEnfants.ForEach(re => CompilerRectangle(re));
            }
        }

        private void PerformAction(AnalyseRectangle rectangle, PdfPage page, dynamic context = null)
        {
            rectangle.Value = ExtractTextAt(page,
                          rectangle.Rectangle.Left,
                          rectangle.Rectangle.Top,
                          rectangle.Rectangle.Height,
                          rectangle.Rectangle.Width);

            var conditionRemplie = true;

            if (rectangle.ConditionCode != null)
            {
                conditionRemplie = rectangle.CheckCondition(new Globals { Input = rectangle.Value });
            }

            if (conditionRemplie)
            {
                //Execution de la fonction
                if (rectangle.FonctionCode != null)
                {
                    rectangle.FunctionDelegate(new Globals { Input = rectangle.Value, Context = context });
                }
                //Lancement pour les enfants
                if (rectangle.RectangleEnfants != null)
                {
                    rectangle.RectangleEnfants.ToList().ForEach(r => PerformAction(r, page, context));
                }
            }
        }

        private string ExtractTextAt(PdfPage page, int left, int top, int height, int width)
        {

            Rectangle mediabox = page.GetPageSize();
            //Les PDFs ont leur origine (0,0) dans le coin droite bas (au lieu du haut droit classique)
            Rectangle rect = new Rectangle(left, mediabox.GetTop() - (top + height), width, height);

            FilteredEventListener listener = new FilteredEventListener();
            TextRegionEventFilter filter = new TextRegionEventFilter(rect);

            LocationTextExtractionStrategy extractionStrategy = listener.AttachEventListener(new LocationTextExtractionStrategySupp(filter), filter);
            new PdfCanvasProcessor(listener).ProcessPageContent(page);
            return extractionStrategy.GetResultantText();
        }

        #region Classe pour ITextSharp
        private class LocationTextExtractionStrategySupp : LocationTextExtractionStrategy
        {
            private IEventFilter _filter;
            public LocationTextExtractionStrategySupp(IEventFilter filter)
                : base()
            {
                _filter = filter;
            }

            public override void EventOccurred(IEventData data, EventType type)
            {
                var affinerData = data as TextRenderInfo;
                if (affinerData != null)
                {
                    foreach (var @char in affinerData.GetCharacterRenderInfos())
                    {
                        if (!string.IsNullOrEmpty(@char.GetText()) && _filter.Accept(@char, type))
                            base.EventOccurred(@char, type);
                    }
                }
            }

        }
        #endregion
    }
}
