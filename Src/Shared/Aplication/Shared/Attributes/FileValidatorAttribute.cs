using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace SharedCore.Aplication.Shared.Attributes {
    public class ValidateFileAttribute : ValidationAttribute {
        public override bool IsValid(object value) {

            int MaxContentLength = 1024 * 1024 * 3;

            string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf" };

            var file = value as IFormFile;
            if (file == null)
                return false;
            else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.')))) {
                ErrorMessage = "Upload File Photo Type: " + string.Join(", ", AllowedFileExtensions);
                return false;
            } else if (file.Length > MaxContentLength) {
                ErrorMessage = "The Size of Photo is too large : " + (MaxContentLength / 1024).ToString() + "MB";
                return false;
            } else
                return true;
        }
    }
}