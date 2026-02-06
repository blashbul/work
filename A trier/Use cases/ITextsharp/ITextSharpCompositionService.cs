using Editique.Composition.Domain.Scenarios;
using Editique.Composition.Domain.Sorties;
using Editique.Composition.Domain.Sorties.Courrier;

using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.IO;

namespace Editique.Composition.Infrastructure.Sorties
{

    public class ITextSharpCompositionService : ICompositionService
    {
        private string InputPath { get; set; }
        private string OutputPath { get; set; }

        /// <summary>
        /// Service de composition de fichier pdf ItextSharp
        /// </summary>
        /// <param name="inputPath">chemin où trouver les fichiers en entrée pour la composition</param>
        /// <param name="outputPath">chemin de sortie des documents produits</param>
        public ITextSharpCompositionService(string inputPath, string outputPath)
        {
            InputPath = inputPath;
            OutputPath = outputPath;
        }


        public void Compose(ExecutionScenario executionScenario)
        {
            //Pas de documents a composer
            if (executionScenario.DocumentsGeneres.Count == 0)
                return;

            using (var wsStorage = new WsStorage.DataTransfertSoapClient())
            {
                //clé du fichier
                var key = InputPath + @"/" + executionScenario.ClientId + @"/Input/" + executionScenario.DepotId;

                //Lecture du fichier de S3 dans le bucket de composition
                var data = wsStorage.GetDocument(WsStorage.TypeBucket.CompositionEditique, key);

                using (MemoryStream msReader = new MemoryStream(data))
                {
                    PdfReader reader = new PdfReader(msReader);
                    PdfDocument pdfSource = new PdfDocument(reader);

                    foreach (var document in executionScenario.DocumentsGeneres)
                    {
                        //Création des documents de sorties
                        var fileName = document.Id + ".pdf";
                        var keyFichierSortie = OutputPath + @"/" + fileName;

                        using (MemoryStream msWriter = new MemoryStream())
                        {
                            PdfWriter writer = new PdfWriter(msWriter);
                            
                            writer.SetSmartMode(true);

                            if (document.Sortie.TypeSortie == TypeSortie.Recommande || document.Sortie.TypeSortie == TypeSortie.Courrier)
                            {
                                ComposeCourrier(wsStorage, pdfSource, document, keyFichierSortie, msWriter, writer);
                            }
                            if(document.Sortie.TypeSortie == TypeSortie.Email)
                            {
                                ComposeEmail(wsStorage, pdfSource, document, keyFichierSortie, msWriter, writer);
                            }
                        }
                    }

                    pdfSource.Close();
                }
            }

        }

        private void ComposeCourrier(WsStorage.DataTransfertSoapClient wsStorage, PdfDocument pdfSource, Document document, string keyFichierSortie, MemoryStream msWriter, PdfWriter writer)
        {
            var sortie = (SortieCourrier)document.Sortie;

            //Creation du pdf
            //  PdfDocument pdfDocumentTemp = new PdfDocument(writerTemp);
            PdfDocument pdfDocument = new PdfDocument(writer);
            // pdfDocumentTemp.SetDefaultPageSize(PageSize.A4);
            pdfDocument.SetDefaultPageSize(PageSize.A4);


            //Composition de page porte adresse 
            if (sortie.OptionsImpression.ComposerPageDeGarde)
            {
                AjouterPageDeGarde(pdfDocument, sortie.TextePageDeGarde);

                if (sortie.OptionsImpression.IsRectoVerso)
                    pdfDocument.AddNewPage();
            }

            foreach (var groupe in document.Groupes)
            {
                //Copies des pages
                //    pdfSource.CopyPagesTo(groupe.IndexPremierePage, groupe.IndexPremierePage - 1 + groupe.NombrePages, pdfDocumentTemp);

                //Copies des pages
                for (int i = groupe.IndexPremierePage; i <= groupe.IndexPremierePage - 1 + groupe.NombrePages; i++)
                {
                    var page = pdfSource.GetPage(i);
                    //Si une rotation est définie on la remet à 0
                    page.SetRotation(0);

                    //Rotation des pages paysages en portrait 
                    if (page.GetPageSize().GetWidth() > page.GetPageSize().GetHeight())
                    {
                        page.SetRotation(270);
                    }

                    Rectangle orig = page.GetPageSizeWithRotation();
                    //Add A4 page
                    PdfPage pageFinale = pdfDocument.AddNewPage();
                    //Shrink original page content using transformation matrix
                    PdfCanvas canvas = new PdfCanvas(pageFinale);

                    if (orig.GetWidth() > PageSize.A4.GetWidth() || orig.GetHeight() > PageSize.A4.GetHeight())
                    {
                        AffineTransform transformationMatrix = AffineTransform.GetScaleInstance(pageFinale.GetPageSize().GetWidth() / orig
                         .GetWidth(), pageFinale.GetPageSize().GetHeight() / orig.GetHeight());
                        canvas.ConcatMatrix(transformationMatrix);
                    }
                    if (page.GetPageSize().GetWidth() > page.GetPageSize().GetHeight())
                    {
                        AffineTransform transformationMatrix = AffineTransform.GetRotateInstance(page.GetRotation() * Math.PI / 180.0);
                        canvas.ConcatMatrix(transformationMatrix);
                        AffineTransform D = AffineTransform.GetTranslateInstance(-orig.GetHeight(), 0);
                        canvas.ConcatMatrix(D);
                    }

                    PdfFormXObject pageCopy = page.CopyAsFormXObject(pdfDocument);
                    canvas.AddXObject(pageCopy, 0, 0);
                }
            }

            //Ajout d'une page pour les documents en recto verso et nb de pages impaire
            if (sortie.OptionsImpression.IsRectoVerso && pdfDocument.GetNumberOfPages() % 2 != 0)
            {
                pdfDocument.AddNewPage();
            }

            pdfDocument.Close();

            //Enregistrement du fichier dans S3
            var outputData = msWriter.ToArray();
            wsStorage.DeposerFichier(WsStorage.TypeBucket.CompositionEditique, keyFichierSortie, outputData);
        }

        private void ComposeEmail(WsStorage.DataTransfertSoapClient wsStorage, PdfDocument pdfSource, Document document, string keyFichierSortie, MemoryStream msWriter, PdfWriter writer)
        {

            //Creation du pdf
            //  PdfDocument pdfDocumentTemp = new PdfDocument(writerTemp);
            PdfDocument pdfDocument = new PdfDocument(writer);
            // pdfDocumentTemp.SetDefaultPageSize(PageSize.A4);
            pdfDocument.SetDefaultPageSize(PageSize.A4);

            foreach (var groupe in document.Groupes)
            {
                //Copies des pages
                //    pdfSource.CopyPagesTo(groupe.IndexPremierePage, groupe.IndexPremierePage - 1 + groupe.NombrePages, pdfDocumentTemp);

                //Copies des pages
                for (int i = groupe.IndexPremierePage; i <= groupe.IndexPremierePage - 1 + groupe.NombrePages; i++)
                {
                    var page = pdfSource.GetPage(i);
                    //Si une rotation est définie on la remet à 0
                    page.SetRotation(0);

                    //Rotation des pages paysages en portrait 
                    if (page.GetPageSize().GetWidth() > page.GetPageSize().GetHeight())
                    {
                        page.SetRotation(270);
                    }

                    Rectangle orig = page.GetPageSizeWithRotation();
                    //Add A4 page
                    PdfPage pageFinale = pdfDocument.AddNewPage();
                    //Shrink original page content using transformation matrix
                    PdfCanvas canvas = new PdfCanvas(pageFinale);

                    if (orig.GetWidth() > PageSize.A4.GetWidth() || orig.GetHeight() > PageSize.A4.GetHeight())
                    {
                        AffineTransform transformationMatrix = AffineTransform.GetScaleInstance(pageFinale.GetPageSize().GetWidth() / orig
                         .GetWidth(), pageFinale.GetPageSize().GetHeight() / orig.GetHeight());
                        canvas.ConcatMatrix(transformationMatrix);
                    }
                    if (page.GetPageSize().GetWidth() > page.GetPageSize().GetHeight())
                    {
                        AffineTransform transformationMatrix = AffineTransform.GetRotateInstance(page.GetRotation() * Math.PI / 180.0);
                        canvas.ConcatMatrix(transformationMatrix);
                        AffineTransform D = AffineTransform.GetTranslateInstance(-orig.GetHeight(), 0);
                        canvas.ConcatMatrix(D);
                    }

                    PdfFormXObject pageCopy = page.CopyAsFormXObject(pdfDocument);
                    canvas.AddXObject(pageCopy, 0, 0);
                }
            }

            pdfDocument.Close();

            //Enregistrement du fichier dans S3
            var outputData = msWriter.ToArray();
            wsStorage.DeposerFichier(WsStorage.TypeBucket.CompositionEditique, keyFichierSortie, outputData);
        }

        private void AjouterPageDeGarde(PdfDocument pdfDocument, string adresse)
        {
            var document = new iText.Layout.Document(pdfDocument);
            document.SetMargins(20, 20, 20, 20);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("\n\n\n\n\n\n\n\n\n\n");
            sb.Append(adresse);

            Paragraph prAdresse = new Paragraph(sb.ToString());

            prAdresse.SetFont(PdfFontFactory.CreateFont(FontConstants.COURIER));
            prAdresse.SetFontSize(10);
            prAdresse.SetMarginLeft(pdfDocument.GetDefaultPageSize().GetWidth() / 2);

            document.Add(prAdresse);
        }

        /// <summary>
        /// Fusionne plusieurs fichiers PDF en en seul document
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="documentId">Identifiant du document; le fichier de sortie aura "yyyyMMdd_hhmmssffff_documentId" dans le dossier Archive d'input du client</param>
        /// <param name="executionId"></param>
        /// <param name="fichiersAAplatir">Chemin relatif des fichiers a fusionner pour faire un seul document</param>
        /// <param name="isRectoVerso">Indique si la composition des fichiers est en recto verso</param>
        /// <returns >
        /// le nom de fichier de sortie qui servira de depotId pour la suite du workflow
        /// </returns>
        public string Fusionner(string clientId, string documentId, string executionId, List<string> fichiersAAplatir, bool isRectoVerso)
        {
            var fileName = DateTime.Now.ToString("yyyyMMdd_hhmmssffff_") + documentId;

            var key = InputPath + "/" + clientId + @"/Input/" + fileName;

            using (var wsStorage = new WsStorage.DataTransfertSoapClient())
            {
                using (MemoryStream msWriter = new MemoryStream())
                {
                    PdfWriter writer = new PdfWriter(msWriter);
                    writer.SetSmartMode(true);

                    PdfDocument pdfDocument = new PdfDocument(writer);
                    pdfDocument.SetDefaultPageSize(PageSize.A4);


                    foreach (var nomFichier in fichiersAAplatir)
                    {
                        //Lecture du fichier de S3 dans le bucket d'upload de la webSuite
                        var data = wsStorage.GetDocument(WsStorage.TypeBucket.WebSuiteUploads, nomFichier);

                        using (MemoryStream msReader = new MemoryStream(data))
                        {
                            PdfReader reader = new PdfReader(msReader);
                            PdfDocument pdfSource = new PdfDocument(reader);

                            pdfSource.CopyPagesTo(1, pdfSource.GetNumberOfPages(), pdfDocument);


                            if (isRectoVerso && pdfSource.GetNumberOfPages() % 2 != 0)
                            {
                                pdfDocument.AddNewPage();
                            }

                            pdfSource.Close();
                        }
                    }

                    pdfDocument.Close();

                    var outputdata = msWriter.ToArray();
                    wsStorage.DeposerFichier(WsStorage.TypeBucket.CompositionEditique, key, outputdata);
                    return fileName;
                }
            }
        }


    }
}
