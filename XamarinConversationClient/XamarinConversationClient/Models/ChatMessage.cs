using System;
namespace XamarinConversationClient.Models
{
    public class ChatMessage
    {
        public string Message { get; set; }
        public DateTime MessageDateTime { get; set; }
        public bool IsUserInput { get; set; }
    }
}
