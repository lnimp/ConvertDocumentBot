using System.IO;
using ConvertDocument.Models.Convert;
using ConvertDocument.Models.Convert.Words;
using SkiaSharp;
using Telegram.Bot;
using Telegram.Bot.Types;
using static System.Net.WebRequestMethods;
using Document = Telegram.Bot.Types.Document;
using File = System.IO.File;
namespace ConvertDocument.Models.Commands
{
    public class ConvertCommand : ICommand<CallbackQuery>
    {
        public string Name => "Convert";
        private string DirectoryPath { get { return $"{Directory.GetCurrentDirectory()}\\Models\\FileStorage\\"; } set { } }
        private string filePath;
        public async Task Execute(CallbackQuery callback, TelegramBotClient botClient)
        {
            if (callback.Message?.ReplyToMessage?.Document == null) return;
            var document = callback.Message.ReplyToMessage?.Document!;
            string documentType = ConvertInfo.GetType(document.FileName.ToUpper());
            filePath = Path.Combine(DirectoryPath, $"{document.FileId!}{documentType.ToLower()}");
            var ConvertType = GetDocument(documentType);
            UploadFile(document, botClient).Wait();
            if (!IsFileLocked())
            {
                ConvertType.Execute(callback.Data.ToUpper(), filePath).Wait();
                filePath = filePath.Replace($"{ConvertType.NameType.ToLower()}", $"{callback.Data.ToUpper()}");
                document.FileName = document.FileName.Replace($"{ConvertType.NameType.ToLower()}", $"{callback.Data.ToLower()}");
                DownloadFile(callback.From.Id, document, botClient).Wait();
                if (!IsFileLocked()) File.Delete(filePath);
                return;
            }
            await botClient.SendTextMessageAsync(callback.From.Id, "куда торопишься дядя");
        }
        public async Task DownloadFile(long chatId, Document document, TelegramBotClient botClient)
        {
            if (File.Exists(filePath))
            {
                if (!IsFileLocked())
                {
                    using var stream = File.Open(filePath, FileMode.Open);
                    await botClient.SendDocumentAsync(chatId, new Telegram.Bot.Types.InputFiles.InputOnlineFile(stream, document.FileName), document.FileName);
                    return;
                }
            }
        }
        public async Task UploadFile(Document document, TelegramBotClient botClient)
        {
            if (File.Exists(filePath)) return;

            var file = await botClient.GetFileAsync(document!.FileId);

            await using var fs = new FileStream(filePath, FileMode.CreateNew);
            await botClient.DownloadFileAsync(file.FilePath!, fs);
        }
        public IConvert GetDocument(string fileType)
        {
            switch (fileType)
            {
                case ".PDF":
                    Pdf pdf = new Pdf();
                    return pdf;
                case ".DOC":
                    Doc doc = new Doc();
                    return doc;
            }
            return null;
        }
        private bool IsFileLocked()
        {
            FileInfo filepath = new FileInfo(filePath);
            while (true)
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        FileStream stream = filepath.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                        stream.Close();
                        return false;
                    } 
                    else
                        return true;
                }
                catch (IOException e)
                {
                    Thread.Sleep(5000);
                }
            }
        }
    }
}
