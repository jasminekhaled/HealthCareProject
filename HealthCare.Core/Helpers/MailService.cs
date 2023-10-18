using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Helpers
{
    public static class MailServices
    {

        public static string DisplayName = "Testingmailservice";
        public static string Password = "aujh ueet cxdo npar";
        public static string Email = "jemyjemy1212@gmail.com";
        public static string Host = "smtp.gmail.com";
        public static int Port = 587;


        public static async Task<bool> SendEmailAsync(string mailTo, string subject, string body)
        {

            try
            {

                var email = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(Email),
                    Subject = subject
                };

                email.To.Add(MailboxAddress.Parse(mailTo));

                var builder = new BodyBuilder();


                builder.HtmlBody = body;
                email.Body = builder.ToMessageBody();
                email.From.Add(new MailboxAddress(DisplayName, Email));

                using var smtp = new SmtpClient();
                smtp.Connect(Host, Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(Email, Password);
                await smtp.SendAsync(email);

                smtp.Disconnect(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
