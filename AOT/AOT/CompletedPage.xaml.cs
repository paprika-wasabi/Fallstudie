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
    public partial class CompletedPage : UserControl
    {
        readonly CompletedPageViewModel _viewModel = new();

        public CompletedPage()
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
    }
}
