﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace uitinfo.ProgReseau.UploadFichiers.WebUi.Controllers
{
    public class UploadController : Controller
    {
        public object RegexOption { get; private set; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Simple(IFormFile file)
        {
            if (file.Length == 0)
            {
                throw new ArgumentException();
            }
            string v_Path = $"Tmp{Path.DirectorySeparatorChar}{file.FileName}";

            using (var flux = new FileStream(v_Path, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(flux);
            }
            return View(nameof(Index));
        }

        public async Task<IActionResult> Flux()
        {

            Regex v_BoundaryRegex = new Regex("boundary=(.+)", RegexOptions.Singleline);
            string v_Boundary = v_BoundaryRegex.Match(Request.ContentType).Groups[1].Value;

            MultipartReader v_Reader = new MultipartReader(v_Boundary, Request.Body);

            var v_Section = await v_Reader.ReadNextSectionAsync();

            while (v_Section != null)
            {
                ContentDispositionHeaderValue v_Tmp;

                bool v_HasContentDisposition = ContentDispositionHeaderValue.TryParse(v_Section.ContentDisposition, out v_Tmp);

                if (v_HasContentDisposition && v_Section.AsFileSection() != null)
                {
                    var v_FileSection = v_Section.AsFileSection();

                    string v_Path = $"Tmp{Path.DirectorySeparatorChar}{v_FileSection.FileName}";

                    using (var flux = new FileStream(v_Path, FileMode.Create, FileAccess.Write))
                    {
                        v_Section.Body.CopyTo(flux);
                    }
                }

                v_Section = await v_Reader.ReadNextSectionAsync();
            }

            return View(nameof(Index));
        }

        public IActionResult Partition()
        {
            foreach (var v_File in Request.Form.Files)
            {

            }

            return Ok();
        }
    }
}