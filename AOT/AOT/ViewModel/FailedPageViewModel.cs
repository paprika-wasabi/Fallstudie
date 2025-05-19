using AOT.Models;
using AOT.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace AOT
{
    public partial class FailedPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Project> _failedProjectsCollection;

        public FailedPageViewModel()
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

        public void Refresh()
        {
            if (FailedProjectsCollection != null)
            {
                FailedProjectsCollection.Clear();
            }
            DatabaseService databaseService = new DatabaseService();
            List<Project> projects = databaseService.GetAllFailedProjects();
            FailedProjectsCollection = new ObservableCollection<Project>(projects);
        }

    }
}
