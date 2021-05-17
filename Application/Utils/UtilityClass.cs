using System;
using System.Net;
using System.Net.Mail;

namespace Application.Utils
{
    public class UtilityClass
    {
        public string EncodePasswordToBase64(string password)
        {
            byte[] encryptData = new byte[password.Length];
            encryptData = System.Text.Encoding.UTF8.GetBytes(password);
            string encodedData = Convert.ToBase64String(encryptData);
            return encodedData;
        }

        public string DecodePasswordFromBase64(string encodedData, string ResetCode)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder decoder = encoder.GetDecoder();
            byte[] decodePassword = Convert.FromBase64String(encodedData);
            int charCount = decoder.GetCharCount(decodePassword, 0, decodePassword.Length);
            char[] decodedChar = new char[charCount];
            decoder.GetChars(decodePassword, 0, decodePassword.Length, decodedChar, 0);
            string decodedData = new string(decodedChar);
            return decodedData;
        }
        public string SendCodeToResetPassword(string email, string resetCode)
        {
            var fromEmail = new MailAddress("haniya4004092@cloud.neduet.edu.pk");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "0334Waqas";
            var subject = "Reset Password";
            var body = "Hi,<br/>br/>We got request for reset your account password. Reset your password by entering this 6-digit code" +
                    "<br/><br/><strong>Reset Password code " + resetCode + "</strong>";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);

            return "message sent";
        }
    }
}