using Xamarin.Forms;
using XamarinConversationClient.ViewModels;

namespace XamarinConversationClient.Views
{
    public partial class ChatView : ContentPage
    {
        public ChatView()
        {
            InitializeComponent();

            ChatViewModel vm = this.BindingContext as ChatViewModel;
            vm.Messages.CollectionChanged += (sender, e) =>
            {
                var target = vm.Messages[vm.Messages.Count - 1];
                MessagesListView.ScrollTo(target, ScrollToPosition.End, true);
            };
        }
    }
}

