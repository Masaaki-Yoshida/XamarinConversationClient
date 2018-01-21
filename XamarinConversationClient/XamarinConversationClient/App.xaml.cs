using Xamarin.Forms;
using Prism.Unity;
using System.ComponentModel;
using Prism.Services;
using System.Windows.Input;
using Prism.Commands;
using Prism;
using Prism.Ioc;
using XamarinConversationClient.Views;
using IBM.WatsonDeveloperCloud.Conversation.v1;
using IBM.WatsonDeveloperCloud.Conversation.v1.Model;
using XamarinConversationClient.Models;

namespace XamarinConversationClient
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();
            NavigationService.NavigateAsync("NavigationPage/LoginView");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginView>();
            containerRegistry.RegisterForNavigation<ChatView>();
            containerRegistry.RegisterSingleton(typeof(IConversationModel),typeof(ConversationModel));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
