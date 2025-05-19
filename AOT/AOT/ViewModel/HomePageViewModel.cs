using AOT.Models;
using AOT.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

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

    }
}
