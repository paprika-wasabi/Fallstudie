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
                    case Message.MessageType.Search:
                        Search(m.MinBudget, m.MaxBudget, m.Name, m.IsPflicht, m.Leader, m.Department, m.ProjectType);
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

        public async void Search(string minBudget, string maxBudget, string name, bool isPflicht, string leader, string department, string type)
        {
            if (CompletedProjectsCollection != null)
            {
                CompletedProjectsCollection.Clear();

            }
            await Task.Delay(200);
            DatabaseService databaseService = new DatabaseService();
            List<Project> projects = databaseService.SearchCompletedProject(minBudget, maxBudget, name, isPflicht, leader, department, type);
            CompletedProjectsCollection = new ObservableCollection<Project>(projects);
        }

    }
}
