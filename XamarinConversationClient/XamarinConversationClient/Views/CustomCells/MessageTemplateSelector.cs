using System;
using Xamarin.Forms;
using XamarinConversationClient.Models;

namespace XamarinConversationClient.Views.CustomCells
{
    public class MessageTemplateSelector: DataTemplateSelector
    {
        public MessageTemplateSelector()
        {
            // Retain instances!
            this.userDataTemplate = new DataTemplate(typeof(UserViewCell));
            this.botDataTemplate = new DataTemplate(typeof(BotViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as ChatMessage;
            if (messageVm == null)
                return null;
            return messageVm.IsUserInput ? this.userDataTemplate : this.botDataTemplate;
        }

        private readonly DataTemplate userDataTemplate;
        private readonly DataTemplate botDataTemplate;
    }
}
