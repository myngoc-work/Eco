using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace BLL
{
    public class EmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly bool _enableSsl;
        private readonly string _fromEmail;

        public EmailService(string smtpHost, int smtpPort, string smtpUsername, string smtpPassword, bool enableSsl, string fromEmail)
        {
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
            _enableSsl = enableSsl;
            _fromEmail = fromEmail;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_fromEmail, "Công ty TNHH EcoGreen");
                    mail.To.Add(toEmail);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true; // Cho phép HTML trong nội dung email

                    using (SmtpClient smtp = new SmtpClient(_smtpHost, _smtpPort))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                        smtp.EnableSsl = _enableSsl;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                        await smtp.SendMailAsync(mail);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gửi email đến {toEmail}: {ex.Message}");
                // Log lỗi chi tiết hơn nếu cần
                return false;
            }
        }

        // Gửi email có đính kèm tệp đơn
        public Task<bool> SendEmailAsync(string toEmail, string subject, string body, string attachmentPath)
        {
            IEnumerable<string> paths = string.IsNullOrWhiteSpace(attachmentPath)
                ? null
                : new[] { attachmentPath };
            return SendEmailAsync(toEmail, subject, body, paths);
        }

        // Gửi email với nhiều tệp đính kèm
        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body, IEnumerable<string> attachmentPaths)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_fromEmail, "Công ty TNHH EcoGreen");
                    mail.To.Add(toEmail);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    if (attachmentPaths != null)
                    {
                        foreach (var path in attachmentPaths)
                        {
                            if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                            {
                                var attachment = new Attachment(path, "application/pdf");
                                mail.Attachments.Add(attachment);
                            }
                        }
                    }

                    using (SmtpClient smtp = new SmtpClient(_smtpHost, _smtpPort))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                        smtp.EnableSsl = _enableSsl;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                        await smtp.SendMailAsync(mail);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gửi email (đính kèm) đến {toEmail}: {ex.Message}");
                return false;
            }
        }

        // Method đồng bộ để gửi email
        public bool SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                return SendEmailAsync(toEmail, subject, body).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Lỗi khi gửi email đến {toEmail}: {ex.Message}");
                return false;
            }
        }
    }
}