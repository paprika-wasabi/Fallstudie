using AOT.Models;
using System.Windows;

namespace AOT
{
    /// <summary>
    /// Interaktionslogik für FillFormWindow.xaml
    /// </summary>
    public partial class FillFormWindow : Window
    {
        public FillFormWindow()
        {
            InitializeComponent();
        }

        private void SubmitForm_Click(object sender, RoutedEventArgs e)
        {
            Project project = new Project() 
            {
                Budget = BudgetBox.Text,
                Name = NameBox.Text,
                Leader = LeaderBox.Text,
                Department = DepartmentBox.Text,
                Type = TypeBox.Text,
                Member = MemberBox.Text,
            };

            DatabaseService dbs = new();
            if (dbs.AddNewProject(project))
            {
                Close();
            }
            else
            {
                MessageBox.Show("An error occurred while saving data.",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            }
        }
    }
}
