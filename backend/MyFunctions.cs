using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using SendGrid;


namespace photo_add
{
    public class MyFunctions
    {
        public static async Task Run(string fileName, string emailAddress)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("hiznakliza45@gmail.com", "Uploader");
            var subject = $"file {fileName} is successfully uploaded!!!";
            var to = new EmailAddress(emailAddress, "Hello");

            var plainTextContent = $"{fileName} is successfully uploaded!!!";
            var htmlContent = $"<strong>{fileName} is successfully uploaded!!!</strong>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            string sasToken = "sp=r&st=2024-02-15T16:06:12Z&se=2024-02-16T05:06:12Z&spr=https&sv=2022-11-02&sr=c&sig=htuXYmjTqWR%2FO%2FMD5XTfb8jzIzA3cAjh0KjXRKVcPTo%3D";

            var attachment = new Attachment
            {
                Filename = fileName,
                Content = sasToken,
                Type = "application/octet-stream",
                Disposition = "attachment"
            };
            msg.AddAttachment(attachment);

            var response = await client.SendEmailAsync(msg);
        }

    }
}
