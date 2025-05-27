using AOT.Models;
using AOT.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace AOT
{
    public partial class HomePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Project> _projectsCollection;

        public HomePageViewModel()
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

        public async void Refresh()
        {
            if (ProjectsCollection != null)
            {
                ProjectsCollection.Clear();
                
            }
            await Task.Delay(200);
            DatabaseService databaseService = new DatabaseService();
            List<Project> projects = databaseService.GetAllProjects();
            ProjectsCollection = new ObservableCollection<Project>(projects);
        }

        public async void Search(string minBudget, string maxBudget, string name, bool isPflicht, string leader, string department, string type)
        {
            if (ProjectsCollection != null)
            {
                ProjectsCollection.Clear();

            }
            await Task.Delay(200);
            DatabaseService databaseService = new DatabaseService();
            List<Project> projects = databaseService.SearchActiveProject(minBudget, maxBudget, name, isPflicht, leader, department, type);
            ProjectsCollection = new ObservableCollection<Project>(projects);
        }

    }
}
