using AOT.Models;
using AOT.View;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AOT
{
    /// <summary>
    /// Interaktionslogik für HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        readonly HomePageViewModel _viewModel = new();

        public HomePage()
        {
            DataContext = _viewModel;
            InitializeComponent();
            this.Loaded += Window_Loaded;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            bool isChecked = IsPflicht.IsChecked == true;
            WeakReferenceMessenger.Default.Send(new Message(Message.MessageType.Search, BudgetMin.Text, BudgetMax.Text, ProjectName.Text, isChecked, ProjectLeaderComboBox.Text, DepartmentComboBox.Text, ProjectTypeComboBox.Text));
        }

        private void LeaderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProjectLeaderComboBox.SelectedItem is Leader selectedLeader)
            {
                DatabaseService service = new DatabaseService();
                int id = selectedLeader.DepartmentId;
                DepartmentComboBox.SelectedItem = DepartmentComboBox.Items.Cast<Department>().FirstOrDefault(d => d.DepartmentId == id);
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var service = new DatabaseService();

            var options1 = await service.GetLeadersAsync();
            var options2 = await service.GetProjectTypesAsync();
            var options3 = await service.GetDepartmentsAsync();

            // Insert empty/default item at the top
            options1.Insert(0, new Leader { Name = "", LastName = "" });
            options2.Insert(0, new ProjectType { Type = "" });
            options3.Insert(0, new Department { Name = "" });

            ProjectLeaderComboBox.ItemsSource = options1;
            ProjectTypeComboBox.ItemsSource = options2;
            DepartmentComboBox.ItemsSource = options3;

            UserBlock.Text = AuthState.UserName;
            RoleBlock.Text = AuthState.Role;
        }

        private void OpenForm_Click(object sender, RoutedEventArgs e)
        {
            FillFormWindow formWindow = new();
            var parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                formWindow.Owner = parentWindow;
            }

            formWindow.ShowDialog(); // or use Show() if you want it non-modal
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            var item = CollectionView.SelectedItem as Project;
            if (item != null)
            {
                MessageBox.Show($"Viewing: {item.Name}\n{item.Beschreibung}");
            }
        }

        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            // Set the project status to "Genehmigt"
            var item = CollectionView.SelectedItem as Project;
            if (item != null)
            {
                DatabaseService db = new();
                db.UpdateProjectStatus(item, "Genehmigt");
            }
            
            WeakReferenceMessenger.Default.Send(new Message() { Type = Message.MessageType.RefreshUI });
        }

        private void Deny_Click(object sender, RoutedEventArgs e)
        {
            // Set the project status to "Abgelehnt"
            var item = CollectionView.SelectedItem as Project;
            if (item != null)
            {
                DatabaseService db = new();
                db.UpdateProjectStatus(item, "Abgelehnt");
            }
            WeakReferenceMessenger.Default.Send(new Message() { Type = Message.MessageType.RefreshUI });

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Delete the document

        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            var item = CollectionView.SelectedItem as Project;
            if (item != null)
            {
                DatabaseService db = new();
                //db.MoveToDone(item);
            }
            WeakReferenceMessenger.Default.Send(new Message() { Type = Message.MessageType.RefreshUI });
        }

        private void Fail_Click(object sender, RoutedEventArgs e)
        {
            var item = CollectionView.SelectedItem as Project;
            if (item != null)
            {
                DatabaseService db = new();
                //db.MoveToFail(item);
            }
            WeakReferenceMessenger.Default.Send(new Message() { Type = Message.MessageType.RefreshUI });
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextDecimal(((TextBox)sender).Text, e.Text);
        }

        private bool IsTextDecimal(string currentText, string newText)
        {
            string fullText = currentText + newText;

            // Try parse decimal, allow also empty string (for deleting)
            return decimal.TryParse(fullText, out _) || string.IsNullOrEmpty(fullText);
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

}
