//Conversion vers une image en base 64 pour l'affichage direct en navigateur sans création de fichier

namespace App
{
    public class ClassName
    {
        
        public string ConvertToBase64Jpeg(byte[] imageData)
        {
			// Convert byte[] to Base64 String
            string base64String = Convert.ToBase64String(imageData);

			return @"data:image/jpg;base64," + base64String;
        }
	}
}