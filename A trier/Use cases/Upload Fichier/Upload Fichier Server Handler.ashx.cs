// A utiliser pour les Wed forms pour faire un upload en ajax
// Handler ashx pour l'upload de fichier 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.SessionState;

namespace App
{
    /// <summary>
    /// Charge un fihier de façon asynchrone
    /// </summary>
    public class FileUploadHandler : IHttpHandler, IRequiresSessionState
    {
        public static string Url
        {
            get { return "~/FileUploadHandler.ashx"; }
        }

        private class UploadConfiguration
        {
            /// <summary>
            /// Identifiant du fichier
            /// </summary>
            public string FileId { get; set; }

            /// <summary>
            /// Chemin physique réel qui va servir d'ID
            /// </summary>
            public string FilePath { get; set; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {                
                    string pathrefer = context.Request.UrlReferrer.ToString();
					//recupération du dossier prévu pour l'upload
                    
					string Serverpath = HttpContext.Current.Server.MapPath("Upload");

                    var postedFile = context.Request.Files[0];

                    string file;


                    //In case of IE
                    if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE")
                    {
                        string[] files = postedFile.FileName.Split(new char[] { '\\' });
                        file = files[files.Length - 1];
                    }
                    else // In case of other browsers
                    {
                        file = postedFile.FileName;
                    }

                    //sauvegarde du fichier
                    postedFile.SaveAs(file);
                    
                    context.Response.AddHeader("Vary", "Accept");
                    try
                    {
                        if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
                            context.Response.ContentType = "application/json";
                        else
                            context.Response.ContentType = "text/plain";
                    }
                    catch
                    {
                        context.Response.ContentType = "text/plain";
                    }
                    
					var jsonResponse = JsonConvert.SerializeObject(new
                    {
                        Status = "success",
                       ...

                    });
                    context.Response.Write(jsonResponse);
            }
            catch (Exception exp)
            {
                var jsonResponse = JsonConvert.SerializeObject(new
                {
                    Status = "error",
                    Message = exp.Message

                });
                context.Response.Write(jsonResponse);
            }

        }

    
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}