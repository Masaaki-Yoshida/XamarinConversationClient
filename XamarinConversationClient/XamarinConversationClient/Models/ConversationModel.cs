using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using IBM.WatsonDeveloperCloud.Conversation.v1;
using IBM.WatsonDeveloperCloud.Conversation.v1.Model;
using IBM.WatsonDeveloperCloud.Http.Exceptions;

namespace XamarinConversationClient.Models
{
    public class ConversationModel : IConversationModel
    {
        public List<string> WorkSpaces { get; private set; }
        public string WorkSpaceId { get; private set; }
        public object Context { get; private set; }
        public ObservableCollection<ChatMessage> ChatMassages { get; private set; }= new ObservableCollection<ChatMessage>();

        private ConversationService _conversation = new ConversationService();
         
        public ConversationModel()
        {
        }

        public void SetCredential(string id, string pass){
            _conversation.SetCredential(id,pass);
            _conversation.VersionDate = ConversationService.CONVERSATION_VERSION_DATE_2017_05_26;
        }

        public bool IsValidCredential(){
            try
            {
                var res = _conversation.ListWorkspaces();
                if (res != null && res.Workspaces.Count > 1) return true;
                return false;
            }
            catch (System.AggregateException ex){
                var innerEx = ex.InnerException as ServiceResponseException;
                if(innerEx.Status == HttpStatusCode.Unauthorized){                    
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<WorkspaceResponse> ListWorkspaces(){
            return _conversation.ListWorkspaces().Workspaces;
        }

        public WorkspaceExportResponse GetWorkspace(string id){
            return _conversation.GetWorkspace(id);
        }

        public void SetWorkspace(string id){
            this.WorkSpaceId = id;
        }

        public void PushMessage(string message){
            var messageRequest = new MessageRequest();
            messageRequest.Input = new InputData();
            messageRequest.Input.Text = message;
            messageRequest.Context = this.Context;

            this.ChatMassages.Add(new ChatMessage()
            {
                Message = message,
                MessageDateTime = DateTime.Now, 
                IsUserInput = true
            });

            var res = _conversation.Message(this.WorkSpaceId,messageRequest);

            this.Context = res.Context;
            this.ChatMassages.Add(new ChatMessage()
            {
                Message = res.Output.Text[0],
                MessageDateTime = DateTime.Now,
                IsUserInput = false
            });
        }

        public void ClearContext(){
            this.Context = null;
            this.ChatMassages = new ObservableCollection<ChatMessage>();
        }
    }
}
