// Utilisation de Ghostscript.NET pour le conversion d'un PDF vers un jpg
// Nécessite l'installation de Ghostscript sur le poste executant le code
// https://www.ghostscript.com/download/gsdnld.html

using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace App
{
    public class ClassName
    {
        public ... GeneratePreviewImage(string fileId)
        {
            string filepath = "c:\....\file.pdf";
           
			//Résolution des PDF par defaut
            int desired_x_dpi = 72;
            int desired_y_dpi = 72;


            ImageCodecInfo myImageCodecInfo;
            Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            //Code MSDN
            // Get an ImageCodecInfo object that represents the JPEG codec.
            myImageCodecInfo = ImageCodecInfo.GetImageEncoders()
                .Single(codec => codec.FormatID == System.Drawing.Imaging.ImageFormat.Jpeg.Guid);


            // for the Quality parameter category.
            myEncoder = Encoder.Quality;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.
			
            myEncoderParameters = new EncoderParameters(1);

            // Save the bitmap as a JPEG file with quality level 25.
            myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            using (var rasterizer = new GhostscriptRasterizer())
            {
                rasterizer.Open(filepath);

                var img = rasterizer.GetPage(desired_x_dpi, desired_y_dpi, 1);
                using (MemoryStream ms = new MemoryStream())
                {
                    // Convert Image to byte[]
                    img.Save(ms, myImageCodecInfo, myEncoderParameters);
                    img.Save(filepath + ".jpg", myImageCodecInfo, myEncoderParameters);
					
					//Reste du code
					...
                }
            }

        }
	}
    
}