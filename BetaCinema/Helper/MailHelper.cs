using MimeKit;

namespace BetaCinema.Helper
{
    public class MailHelper
    {
        public void GuiMail(string emailReceiver, string body)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("Sender Name", "pthanhduc3012@gmail.com"));
            email.To.Add(new MailboxAddress("Receiver Name", emailReceiver));

            email.Subject = "Mail from my web";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                smtp.Authenticate("pthanhduc3012@gmail.com", "yjef ziit qnye vyqz");

                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}
