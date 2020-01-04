using FbApp.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FbApp.Dtos
{
    public class MessangerModel
    {
        public IEnumerable<MessageModel> Messages { get; set; } = new List<MessageModel>();

        [Required]
        [MaxLength(DataConstants.MaxMessageLength)]
        public string MessageText { get; set; }
    }
}