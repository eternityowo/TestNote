using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TestNote.Tesseract;

namespace TestNote.WEB.Controllers
{
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IMemoryCache cache;

        public FileController(IWebHostEnvironment appEnvironment, IMemoryCache memoryCache)
        {
            _appEnvironment = appEnvironment;
            cache = memoryCache;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            long size = file.Length;

            if (size > 0)
            {
                var filePathImg = Path.GetTempFileName();
                var filePathTxt = Path.GetTempFileName();

                using (var stream = new FileStream(filePathImg, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                string hash = ImageHash.ProcessImage(filePathImg);

                string text = "";
                if (!cache.TryGetValue(hash, out text))
                {
                    text = TextOCR.GetText(_appEnvironment.ContentRootPath + "\\tessdata", filePathImg);
                    if (text != "")
                    {
                        cache.Set(hash, text, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                    }
                }

                using (FileStream fs = System.IO.File.Create(filePathTxt))
                {
                    byte[] content = new UTF8Encoding(true).GetBytes(text);
                    fs.Write(content, 0, content.Length);
                }
                FileInfo info = new FileInfo(filePathTxt);

                return PhysicalFile(info.FullName, "text/plain", info.Name.Replace(".tmp", ".txt"));
            }

            return BadRequest(new { text = "Try another file" });
        }
    }
}