using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace iutinfo.ProgReseau.UploadFichiers.WebUi.Controllers
{
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Simple(IFormFile file)
        {
            if( file.Length == 0)
            {
                throw new ArgumentException();
            }

            string v_Path = $"Tmp{Path.DirectorySeparatorChar}{file.FileName}";

            using (var flux = new FileStream(v_Path, FileMode.Create, FileAccess.Write)){
                file.CopyTo(flux);
            }

                return View(nameof(Index));
        }
    }
}
