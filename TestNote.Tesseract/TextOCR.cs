using System;
using System.Diagnostics;
using Tesseract;

namespace TestNote.Tesseract
{
    public static class TextOCR
    {
        public static string GetText(string path, string ImagePath)
        {
            string text = "";
            try
            {
                using (var engine = new TesseractEngine(path, "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(ImagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            text = page.GetText();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                text = e.ToString();
            }
            return text;
        }
    }
}