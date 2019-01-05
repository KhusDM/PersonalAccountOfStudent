using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace PersonalAccountOfStudent.Controllers
{
    public class ProgressController : Controller
    {
        private IHostingEnvironment Env { get; }

        public ProgressController(IHostingEnvironment env)
        {
            this.Env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DownloadProgress(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                return Content("Not found");

            var path = Path.Combine(Env.WebRootPath, "files", filename);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;

            string extension = new FileInfo(filename).Extension;
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(extension, false);
            object value = key != null ? key.GetValue("Content Type", null) : null;
            string mimeType = value != null ? value.ToString() : String.Empty;

            if (!String.IsNullOrEmpty(mimeType))
                return File(memory, mimeType, filename);

            return Content("trouble...");

        }
    }
}