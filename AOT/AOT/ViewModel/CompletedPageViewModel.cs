using AOT.Models;
using AOT.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace AOT
{
    public partial class CompletedPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Project> _completedProjectsCollection;

        public CompletedPageViewModel()
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
            if (CompletedProjectsCollection != null)
            {
                CompletedProjectsCollection.Clear();
            }
            DatabaseService databaseService = new DatabaseService();
            List<Project> projects = databaseService.GetAllCompletedProjects();
            CompletedProjectsCollection = new ObservableCollection<Project>(projects);
        }

    }
}
