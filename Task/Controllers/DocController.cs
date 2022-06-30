using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TaskTwo.Models;
using TaskTwo.Models.AppDbContext;
using TaskTwo.Models.ViewModel;

namespace TaskTwo.Controllers
{
    public class DocController : Controller
    {
        private readonly AppDbContext _appDbcontext;

       // private IHostingEnvironment Environment;


        public DocController(AppDbContext appDbContext /*IHostingEnvironment _environment*/)
        {
            _appDbcontext = appDbContext;
          //  Environment = _environment;
        }
        [HttpGet]
        public IActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(DocumentVM model)
        {
            if (ModelState.IsValid)
            {
                List<IFormFile> files = new List<IFormFile>();
                files.Add(model.fileOne);
                files.Add(model.fileTwo);
                files.Add(model.fileThree);

                List<string> filesnames = UploadDoc(files);
                if (filesnames != null)
                {
                    var document = new Document();
                    document.FileOne = filesnames[0];
                    document.FileTwo = filesnames[1];
                    document.FIleThree = filesnames[2];
                    _appDbcontext.Documents.Add(document);
                    await _appDbcontext.SaveChangesAsync();
                    ViewBag.Message = Helper.Helper.Messagesuccess;
                }
                else
                {
                    ViewBag.Message = Helper.Helper.MessageFail;

                }
            }
            return View();
        }
        private List<string> UploadDoc(List<IFormFile> model)
        {

            List<string> filesnames = new List<string>();

            foreach (IFormFile file in model)
            {

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(file.FileName);
                string fileName = Guid.NewGuid().ToString() + "-" + file.FileName /*+ fileInfo.Extension*/;

                filesnames.Add(fileName);

                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    file.CopyToAsync(stream);
                }

            }
            return filesnames;
        }

    }

}
