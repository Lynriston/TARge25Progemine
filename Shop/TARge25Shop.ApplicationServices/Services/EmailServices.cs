using Microsoft.Extensions.Configuration;
using MimeKit;
using TARge25Shop.Core.Dto;

namespace TARge25Shop.ApplicationServices.Services
{
    public class EmailServices
    {
        private readonly IConfiguration _config;
        public EmailServices
            (
                IConfiguration config
            )
        {
            _config = config;
        }



        public void SendEmail(EmailDto dto)
        {
            var email = new MimeMessage();
            // Set the sender and recipient addresses
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUserName").Value));
            email.To.Add(MailboxAddress.Parse(dto.To));
            email.Subject = dto.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = dto.Body
            };

        }
    }
}
