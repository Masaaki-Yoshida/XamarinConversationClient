using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using IBM.WatsonDeveloperCloud.Conversation.v1.Model;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Reactive.Bindings;
using XamarinConversationClient.Models;

namespace XamarinConversationClient.ViewModels
{
    public class ChatViewModel : BindableBase, INavigationAware, IDisposable
    {
        #region PrivateField
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IConversationModel _conversationModel;
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
                             IPageDialogService pageDialogService,
                             IConversationModel conversationModel)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _conversationModel = conversationModel;

            _conversationModel.ListWorkspaces().ForEach((space) =>
            {
                WorkSpaces.Add(space);
            });

            SelectedWorkSpace.Value = WorkSpaces.FirstOrDefault();

            // this.Messages = _conversation.ChatMassages.ObserveAddChanged<ChatMessage>().ToReactiveCollection();

            this.ChatStartCommand = new ReactiveCommand();
            this.ChatStartCommand.Subscribe(_ => ChatStartExecute());
            this.SendCommand = new ReactiveCommand();
            this.SendCommand.Subscribe(_ => SendExecute());
        }

        private void ChatStartExecute()
        {
            try
            {
                _conversationModel.ClearContext();
                _conversationModel.SetWorkspace(SelectedWorkSpace.Value.WorkspaceId);
                _conversationModel.PushMessage(string.Empty);
                this.Messages.Add(_conversationModel.ChatMassages.Last());
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
                _conversationModel.PushMessage(MessageText.Value);
                this.Messages.Add(new ChatMessage(){ IsUserInput = true, Message = MessageText.Value, MessageDateTime = DateTime.Now});
                this.Messages.Add(_conversationModel.ChatMassages.Last());
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
