using AOT.Models;
using AOT.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace AOT
{
    public partial class PendingPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Project> _pendingProjectsCollection;

        public PendingPageViewModel()
        {
            Refresh();
            WeakReferenceMessenger.Default.Register<Message>(this, (r, m) =>
            {
                switch (m.Type)
                {
                    case Message.MessageType.RefreshUI:
                        Refresh();
                        break;
                }
            });
            
        }

        public async void Refresh()
        {
            if (PendingProjectsCollection != null)
            {
                PendingProjectsCollection.Clear();
            }
            await Task.Delay(200);
            DatabaseService databaseService = new DatabaseService();
            List<Project> projects = databaseService.GetAllPendingProjects();
            PendingProjectsCollection = new ObservableCollection<Project>(projects);
        }
    }
}
