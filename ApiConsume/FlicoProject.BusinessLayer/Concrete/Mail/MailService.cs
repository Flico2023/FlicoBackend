using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FlicoProject.BusinessLayer.Concrete.Mail
{

    //BURDA HİÇ HATA KONTROLÜ YOK
    public class MailService : IMailService
    {
        private readonly IConfiguration _config;
        public MailService(IConfiguration config) {
            _config= config;
        }

        public void SendMail(string to, string subject, string html)
        {
            
            var emailConfig = _config.GetSection("EmailConfig");
            var host = emailConfig.GetSection("EmailHost").Value;
            var username = emailConfig.GetSection("EmailUsername").Value;
            var password = emailConfig.GetSection("EmailPassword").Value;
            var port = Convert.ToInt32(emailConfig.GetSection("EmailPort").Value);

            //kimden
            var mimeMessage = new MimeMessage();
            var mailboxAddressFrom = new MailboxAddress("Flico",username);
            mimeMessage.From.Add(mailboxAddressFrom);

            //kime
            var mailboxAddressTo = new MailboxAddress("User", to);
            mimeMessage.To.Add(mailboxAddressTo);

            //içerik
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = html;
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            mimeMessage.Subject = subject;

            SmtpClient client = new SmtpClient();
            client.Connect(host, port, false);
            client.Authenticate(username, password);
            client.Send(mimeMessage);
            client.Disconnect(true);
          
        }
    }
}
