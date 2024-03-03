using FlicoProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.BusinessLayer.Concrete.Mail.MailFormatters
{
    public class ContactMessageAnswerMailFormatter
    {
        public string FormatContactMessageEmail(ContactMessage message)
        {
            var currentYear = DateTime.Now.Year;
            var messageDate = message.MessageDate.HasValue ? message.MessageDate.Value.ToString("yyyy-MM-dd") : "N/A";
            var answerDate = message.AnswerDate.HasValue ? message.AnswerDate.Value.ToString("yyyy-MM-dd") : "N/A";

            return $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
<meta charset=""UTF-8"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
<style>
    body {{ font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; margin: 0; padding: 0; background-color: #f9f9f9; }}
    .container {{ max-width: 600px; margin: auto; background: #ffffff; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); }}
    .header {{ background-color: #007bff; color: #ffffff; padding: 20px; text-align: center; font-size: 24px; }}
    .content {{ padding: 20px; line-height: 1.6; color: #444444; }}
    .content p {{ margin: 10px 0; }}
    .bold {{ font-weight: bold; }}
    .footer {{ background-color: #f2f2f2; color: #888888; text-align: center; padding: 10px; font-size: 12px; }}
    .answer {{ padding: 15px; background-color: #e7f4ff; border-left: 4px solid #007bff; margin: 20px 0; }}
    .contact-link {{ display: inline-block; background-color: #007bff; color: #ffffff; padding: 10px 15px; text-decoration: none; border-radius: 5px; margin-top: 20px; }}
</style>
</head>
<body>
<div class=""container"">
    <div class=""header"">
        Your Response from FLICO
    </div>
    <div class=""content"">
        <p>Hello <span class=""bold"">{message.Name}</span>,</p>
        <p>We have received and reviewed your message. Below you can find the details of your message and our response:</p>
        <p><span class=""bold"">Subject:</span> {message.Subject}</p>
        <p><span class=""bold"">Your Message:</span> {message.Message}</p>
        <p><span class=""bold"">Message Date:</span> {messageDate}</p>
        <div class=""answer"">
            <p><span class=""bold"">Our Response:</span></p>
            <p>{message.Answer}</p>
        </div>
        <p><span class=""bold"">Response Date:</span> {answerDate}</p>
        <p>If you have any further questions, please do not hesitate to contact us at <a href=""mailto:companyflico@gmail.com"" class=""contact-link"">companyflico@gmail.com</a>.</p>
        <div class=""signature"">
            Best Regards,<br>
            The FLICO Team
        </div>
    </div>
    <div class=""footer"">
        © {currentYear} FLICO. All rights reserved.
    </div>
</div>
</body>
</html>
";
        }
    }
}
