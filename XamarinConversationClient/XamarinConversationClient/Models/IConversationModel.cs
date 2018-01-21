using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IBM.WatsonDeveloperCloud.Conversation.v1.Model;

namespace XamarinConversationClient.Models
{
    public interface IConversationModel
    {
        List<string> WorkSpaces { get; }
        string WorkSpaceId { get; }
        object Context { get; }
        ObservableCollection<ChatMessage> ChatMassages { get; }

        void SetCredential(string id, string pass);
        bool IsValidCredential();
        List<WorkspaceResponse> ListWorkspaces();
        WorkspaceExportResponse GetWorkspace(string id);
        void SetWorkspace(string id);
        void PushMessage(string message);
        void ClearContext();
    }
}