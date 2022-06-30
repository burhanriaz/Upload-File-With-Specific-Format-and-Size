using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
namespace TaskTwo.Models.ViewModel
{
   
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected  override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           
            var file = value as IFormFile;

            var extension = Path.GetExtension(file.FileName);

            if (file != null)
            {
                if (!_extensions.Contains(extension.ToLower()) )
                {
                    return new ValidationResult(GetErrorMessage());
                }
                if (file.Length > 2097152)
                {
                    return new ValidationResult(GetErrorMessageLen());
                }
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Your Document type is not valid Allowed Only .PDF or .DOC!";
        }
        public string GetErrorMessageLen()
        {
            return $"File size shoud be less than 2MB.";
        }
    }
    public class DocumentVM
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Please Select file")]
        [AllowedExtensions(new string[] { ".pdf", ".docx" })]
         public IFormFile fileOne { get; set; }



        [Required(ErrorMessage = "please select file")]
        [AllowedExtensions(new string[] { ".pdf", ".docx" })]
        public IFormFile fileTwo { get; set; }



        [Required(ErrorMessage = "please select file")]
        [AllowedExtensions(new string[] { ".pdf", ".docx" })]
        public IFormFile fileThree { get; set; }



    }


}

