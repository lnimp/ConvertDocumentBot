using Aspose.Words;

namespace ConvertDocument.Models.Convert.Words
{
    public struct Doc : IConvert
    {
        private string nameType = ".DOC";
        private string[] convertibleTypes = new string[] { ".PDF", ".DOCX", };
        public string NameType { get { return nameType; } }
        public string[] ConvertibleTypes { get { return convertibleTypes; } }
        public Doc() { }
        public void Execute(string toType, string filePath)
        {
            if (!File.Exists(filePath)) return;
            SaveFormat format = SaveFormat.Doc;
            switch (toType)
            {
                case ".PDF":
                    format = SaveFormat.Pdf;
                    break;
                case ".DOCX":
                    format = SaveFormat.Docx;
                    break;
            }
            Convert(filePath, format).Wait();
            File.Move(filePath, filePath.Replace($"{NameType.ToLower()}", $"{toType.ToLower()}"));
        }
        private Task Convert(string filePath, SaveFormat format)
        {
            Document document = new Document(filePath);
            document.Save(filePath, format);
            return Task.CompletedTask;
        }
    }
}
