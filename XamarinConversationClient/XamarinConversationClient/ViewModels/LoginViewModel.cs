using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using IBM.WatsonDeveloperCloud.Conversation.v1.Model;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using XamarinConversationClient.Models;

namespace XamarinConversationClient.ViewModels
{
    public class LoginViewModel : BindableBase, INavigationAware, IDisposable
    {
        #region PrivateField
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IConversationModel _conversationModel;
        private SynchronizationContext context = new SynchronizationContext();
        #endregion

        #region Property
        public ReactiveProperty<string> UserName { get; private set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Password { get; private set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> JsonCode { get; private set; } = new ReactiveProperty<string>();

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        #endregion

        #region Command
        public AsyncReactiveCommand LoginCommand { get; private set; }
        #endregion

        #region Constractor
        public LoginViewModel(INavigationService navigationService,
                             IPageDialogService pageDialogService,
                             IConversationModel conversationModel)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _conversationModel = conversationModel;

            LoginCommand = this.ObserveProperty(vm => vm.IsBusy).Select(x => !x).ToAsyncReactiveCommand();
            LoginCommand.Subscribe(async () => await LoginCommandExecute());
        }
        #endregion

        #region CommandMethod
        private async Task LoginCommandExecute(){
            try
            {
                IsBusy = true;

                if(!string.IsNullOrEmpty(JsonCode.Value)){
                    var cred = JsonConvert.DeserializeObject<WatsonCredential>(JsonCode.Value);
                    if(string.IsNullOrEmpty(cred.UserName) || string.IsNullOrEmpty(cred.Password)){
                        await _pageDialogService.DisplayAlertAsync(
                            "Information",
                            "Input Information is Invalid.",
                            "OK");
                        return;
                    }

                    UserName.Value = cred.UserName;
                    Password.Value = cred.Password;
                }

                _conversationModel.SetCredential(UserName.Value, Password.Value);
                if (!_conversationModel.IsValidCredential())
                {
                    await _pageDialogService.DisplayAlertAsync(
                            "Information",
                            "Login failed.",
                            "OK");
                    return;
                }

                await _navigationService.NavigateAsync("/NavigationPage/ChatView");
            }
            catch (Exception ex)
            {
                await _pageDialogService.DisplayAlertAsync(
                            "Error",
                            "Login failed. " + ex.Message,
                            "OK");
                return;
            }
            finally{
                IsBusy = false;
            }
        }
        #endregion

        #region LifeCycleMethod
        public void Dispose()
        {
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        #endregion

    }
}
