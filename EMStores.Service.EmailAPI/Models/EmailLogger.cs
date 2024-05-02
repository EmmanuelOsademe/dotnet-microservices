using System.ComponentModel.DataAnnotations;

namespace EMStore.Services.EmailAPI.Models
{
    public class EmailLogger
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty ;
        public DateTime? EmailSent { get; set; }
    }
}
