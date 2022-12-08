using Aspose.Slides;
using Aspose.Slides.Export;

namespace ConvertDocument.Models.Convert.PowerPoint
{
    public struct Pptx : IConvert
    {
        private string nameType = ".PPTX";
        private string[] convertibleTypes = new string[] { ".PDF", ".PPT" };
        public string NameType { get { return nameType; } }
        public string[] ConvertibleTypes { get { return convertibleTypes; } }
        public Pptx() { }
        public async Task Execute(string toType, string filePath)
        {
            if (!File.Exists(filePath)) return;
            SaveFormat format = SaveFormat.Pdf;
            switch (toType)
            {
                case ".PDF":
                    format = SaveFormat.Pdf;
                    break;
                case ".PPT":
                    format = SaveFormat.Ppt;
                    break;
            }
            Convert(filePath, format).Wait();
            File.Move(filePath, filePath.Replace($"{NameType.ToLower()}", $"{toType.ToLower()}"));
        }
        private async Task Convert(string filePath, SaveFormat format)
        {
            Presentation pres = new Presentation(filePath);
            pres.Save(filePath, format);
        }
    }
}
