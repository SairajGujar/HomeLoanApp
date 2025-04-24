using Microsoft.AspNetCore.Identity.UI.Services;

namespace HomeLoan.Services
{
    public class EmailSender:IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Implement your email sending logic here
            Console.WriteLine($"Email sent to {email} with subject {subject} and message {htmlMessage}");
            return Task.CompletedTask;
        }
    }
}
