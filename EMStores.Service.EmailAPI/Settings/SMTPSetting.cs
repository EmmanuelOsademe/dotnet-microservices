namespace EMStore.Services.EmailAPI.Settings
{
    public class SMTPSetting
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }

        // SMTP Username
        public string User { get; set; } = string.Empty;
        // SMTP Password
        public string Password { get; set; } = string.Empty;

        public string From { get; set; } = string.Empty;    
    }
}
