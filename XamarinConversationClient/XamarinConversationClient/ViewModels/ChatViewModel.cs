using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Xamarin.Forms;
using Reactive.Bindings.Extensions;
using System.Reactive.Disposables;
using System;
using System.Reactive.Linq;
using System.Linq;
using System.Threading.Tasks;
using Prism.Services;
using IBM.WatsonDeveloperCloud.Conversation.v1;
using XamarinConversationClient.Models;
using System.Collections.Generic;
using IBM.WatsonDeveloperCloud.Conversation.v1.Model;
using System.Threading;

namespace XamarinConversationClient.ViewModels
{
    public class ChatViewModel : BindableBase, INavigationAware, IDisposable
    {
        #region PrivateField
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

        private readonly ConversationModel _conversation;

        private SynchronizationContext context = new SynchronizationContext();

        #endregion

        #region Property
        public ReactiveCollection<ChatMessage> Messages { get; set; } = new ReactiveCollection<ChatMessage>();
        public ReactiveProperty<string> MessageText { get; private set; } = new ReactiveProperty<string>();

        public ReactiveCollection<WorkspaceResponse> WorkSpaces { get; private set; } = new ReactiveCollection<WorkspaceResponse>();
        public ReactiveProperty<WorkspaceResponse> SelectedWorkSpace { get; private set; } = new ReactiveProperty<WorkspaceResponse>();
        #endregion

        #region Command
        public ReactiveCommand ChatStartCommand { get; private set; }
        public ReactiveCommand SendCommand { get; private set; }
        #endregion

        public ChatViewModel(INavigationService navigationService,
                             IPageDialogService pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;

            //TODO:Credential Setting
            _conversation = new ConversationModel();
            _conversation.SetCredential("{{Your-ConversationService-UserName}}", "{{Your-ConversationService-Password}}");

            _conversation.ListWorkspaces().ForEach((space) =>
            {
                WorkSpaces.Add(space);
            });

            SelectedWorkSpace.Value = WorkSpaces.FirstOrDefault();

            // this.Messages = _conversation.ChatMassages.ObserveAddChanged<ChatMessage>().ToReactiveCollection();

            //メアド認証 新規登録
            this.ChatStartCommand = new ReactiveCommand();
            this.ChatStartCommand.Subscribe(_ => ChatStartExecute());
            this.SendCommand = new ReactiveCommand();
            this.SendCommand.Subscribe(_ => SendExecute());
        }

        private void ChatStartExecute()
        {
            try
            {
                _conversation.ClearContext();
                _conversation.SetWorkspace(SelectedWorkSpace.Value.WorkspaceId);
                _conversation.PushMessage(string.Empty);
                this.Messages.Add(_conversation.ChatMassages.Last());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
            }
        }

        private void SendExecute()
        {
            try
            {
                _conversation.PushMessage(MessageText.Value);
                this.Messages.Add(new ChatMessage(){ IsUserInput = true, Message = MessageText.Value, MessageDateTime = DateTime.Now});
                this.Messages.Add(_conversation.ChatMassages.Last());
                MessageText.Value = string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
            }
        }
        #region LifeCycleMethod

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        public void Dispose()
        {
            this.Disposable.Dispose();
        }
        #endregion

    }
}
