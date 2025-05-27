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
                    case Message.MessageType.Search:
                        Search(m.MinBudget, m.MaxBudget, m.Name, m.IsPflicht, m.Leader, m.Department, m.ProjectType);
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

        public async void Search(string minBudget, string maxBudget, string name, bool isPflicht, string leader, string department, string type)
        {
            if (FailedProjectsCollection != null)
            {
                FailedProjectsCollection.Clear();

            }
            await Task.Delay(200);
            DatabaseService databaseService = new DatabaseService();
            List<Project> projects = databaseService.SearchFailedProject(minBudget, maxBudget, name, isPflicht, leader, department, type);
            FailedProjectsCollection = new ObservableCollection<Project>(projects);
        }

    }
}
