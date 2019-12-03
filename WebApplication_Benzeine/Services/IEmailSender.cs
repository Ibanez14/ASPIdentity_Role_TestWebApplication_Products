namespace WebApplication_Benzeine.Services
{
    public interface IEmailSender
    {
        System.Threading.Tasks.Task<SendGrid.Response> SendEmailAsync(string email, string subject, string message);
    }
}