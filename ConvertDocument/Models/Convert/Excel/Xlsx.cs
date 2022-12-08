namespace ConvertDocument.Models.Convert.Excel
{
    public struct Xlsx : IConvert
    {
        private string nameType = ".XLSX";
        private string[] convertibleTypes = new string[] { ".PDF", };
        public string NameType { get { return nameType; } }
        public string[] ConvertibleTypes { get { return convertibleTypes; } }
        public Xlsx() { }
        public async Task Execute(string toType, string filePath)
        {
            if (!File.Exists(filePath)) return;
            switch (toType)
            {
                case ".PDF":
                    ConvertToDoc(filePath).Wait();
                    break;
            }
            File.Move(filePath, filePath.Replace($"{NameType.ToLower()}", $"{toType.ToLower()}"));
        }

        private async Task ConvertToPdf(string filePath)
        {
            Document pdfDocument = new Document(filePath);
            pdfDocument.Save(filePath, SaveFormat.Pptx);
        }
    }
}
