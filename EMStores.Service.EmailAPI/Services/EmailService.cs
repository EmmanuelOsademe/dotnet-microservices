using EMStore.Services.EmailAPI.Data;
using EMStore.Services.EmailAPI.Dtos;
using EMStore.Services.EmailAPI.Dtos.Cart;
using EMStore.Services.EmailAPI.Models;
using EMStore.Services.EmailAPI.Repositories;
using EMStore.Services.EmailAPI.Repositories.Interfaces;
using EMStore.Services.EmailAPI.Services.Interfaces;
using EMStore.Services.EmailAPI.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EMStore.Services.EmailAPI.Services
{
    public class EmailService(EmailRepository emailRepository, SMTPSetting smtpSetting) : IEmailService
    {
        private readonly IEmailRepository _emailRepository = emailRepository;
        private readonly SMTPSetting _smtpSetting = smtpSetting;

        public async Task EmailCartAndLog(CartDto cartDto)
        {
            
            StringBuilder message = new();

            message.AppendLine("<br/>Cart Email Requested");
            message.AppendLine($"<br/>Total {cartDto.CartHeader.CartTotal}");
            message.Append("<br/>");
            message.Append("<ul>");
            foreach(var item in cartDto.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("</li>");
            }
            message.Append("</ul>");

            // Send the email
            await SendMailAsync(cartDto.CartHeader.Email, "Cart Email", message.ToString());

            // Log the email to the database
            await _emailRepository.LogAndEmailAsync(message.ToString(), cartDto.CartHeader.Email);
        }

        public async Task EmailUserRegistrationAndLog(UserDTO userDto)
        {
            StringBuilder message = new();

            message.AppendLine("<br />New User Registration");
            message.AppendLine("<br />User with the following details just registered:");
            message.AppendLine($"<br />Name: {userDto.Name}");
            message.AppendLine($"<br />Email: {userDto.Email}");
            message.AppendLine($"<br />Phone number: {userDto.PhoneNumber}");

            await SendMailAsync("emma.osademe@gmail.com", "New User Registration", message.ToString());

            await _emailRepository.LogAndEmailAsync(message.ToString(), "emma.osademe@gmail.com");
        }

        private async Task SendMailAsync (string to, string subject, string body)
        {
            var message = new MailMessage(_smtpSetting.From, to, subject, body);

            using(var emailClient = new SmtpClient(_smtpSetting.Host, _smtpSetting.Port))
            {
                emailClient.Credentials = new NetworkCredential(_smtpSetting.User, _smtpSetting.Password);

                await emailClient.SendMailAsync(message);
            }

        }
    }
}
