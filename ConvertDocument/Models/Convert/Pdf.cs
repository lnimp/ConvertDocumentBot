using Aspose.Pdf;

namespace ConvertDocument.Models.Convert
{
    public struct Pdf : IConvert
    {
        private string nameType = ".PDF";
        private string[] convertibleTypes = new string[] { ".DOC", ".DOCX", ".PPTX", "Excel",".PPT" };
        public string NameType { get { return nameType; } }
        public string[] ConvertibleTypes { get { return convertibleTypes; } }
        public Pdf() { }
        public void Execute(string toType, string filePath)
        {
            if (!File.Exists(filePath))
                return;
            SaveFormat format = SaveFormat.Pdf;
            switch (toType)
            {
                case ".DOC":
                    format = SaveFormat.Doc;
                    break;
                case ".DOCX":
                    format = SaveFormat.DocX;
                    break;
                case ".PPTX":
                    format = SaveFormat.Pptx;
                    break;
                default:
                    return;
            }
            Convert(filePath, format).Wait();
            File.Move(filePath, filePath.Replace($"{NameType.ToLower()}", $"{toType.ToLower()}"));
            return;
        }
        internal Task Convert(string filePath, SaveFormat format)
        {
            Document document = new Document(filePath);
            document.Save(filePath, format);
            return Task.CompletedTask;
        }
    }
}
