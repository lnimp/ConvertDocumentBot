using Aspose.Pdf;
using Telegram.Bot.Types;

namespace ConvertDocument.Models.Convert
{
    public interface IConvert
    {
        string NameType { get;}
        string[] ConvertibleTypes { get;}
        public void Execute(string toType, string filePath);
    }
}
