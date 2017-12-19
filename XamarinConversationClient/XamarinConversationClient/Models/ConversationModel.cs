using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IBM.WatsonDeveloperCloud.Conversation.v1;
using IBM.WatsonDeveloperCloud.Conversation.v1.Model;

namespace XamarinConversationClient.Models
{
    public class ConversationModel
    {
        public List<string> WorkSpaces { get; private set; }
        public string WorkSpaceId { get; private set; }
        public object Context { get; private set; }
        public ObservableCollection<ChatMessage> ChatMassages { get; set; }

        private ConversationService _conversation;

        public ConversationModel()
        {
            _conversation = new ConversationService();
            this.ChatMassages = new ObservableCollection<ChatMessage>();
        }

        public void SetCredential(string id, string pass){
            _conversation.SetCredential(id,pass);
            _conversation.VersionDate = ConversationService.CONVERSATION_VERSION_DATE_2017_05_26;
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
