using Aspose.Words;
using Telegram.Bot.Types;
using Document = Aspose.Words.Document;

namespace ConvertDocument.Models.Convert
{
    public static class ConvertInfo
    {
        public readonly static IConvert[] arrayConvertType = new IConvert[] { new Pdf(),new Doc(),new Pptx() };
        public static string[] GetConvertibleTypes(string documentName)
        {
            return arrayConvertType?.FirstOrDefault(i => i.NameType == GetType(documentName.ToUpper()))?.ConvertibleTypes.ToArray()!;
        }
        public static string GetType(string documentName)
        {
            return documentName.Remove(0, documentName.LastIndexOf("."));
        }
        public static bool CheckType(string documentName)
        {
            string type = GetType(documentName);
            if (arrayConvertType.Any(i => i.NameType.ToString() == type)) return true;
            return false;
        }
    }
}
