using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsoleApp_Chat_Streaming.Models
{
    public class UserMessageRequest
    {
        [JsonPropertyName("prompt")]
        public string? Prompt { get; set; }
    }
}

