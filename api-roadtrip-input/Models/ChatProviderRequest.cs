﻿namespace api_roadtrip_input.Models
{
    public class ChatProviderRequest
    {
        public string? userId { get; set; } = string.Empty;
        public string? sessionId { get; set; } = string.Empty;
        public string? tenantId { get; set; } = string.Empty;
        public string? prompt { get; set; } = string.Empty;

    }
}
