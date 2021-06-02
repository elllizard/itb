using System;

namespace itb.Models.Api
{
    public class ChatModel
    {
        public string Id { get; set; } = null;

        public long ChatId { get; set; }

        public string Username { get; set; } = null;

        public string Path { get; set; } = null;

        public string State { get; set; } = null;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}