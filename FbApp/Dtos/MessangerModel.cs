using FbApp.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FbApp.Dtos
{
    public class MessangerModel
    {
        public IEnumerable<MessageModel> Messages { get; set; }

        [Required]
        [MaxLength(DataConstants.MaxMessageLength)]
        public string MessageText { get; set; }
    }
}