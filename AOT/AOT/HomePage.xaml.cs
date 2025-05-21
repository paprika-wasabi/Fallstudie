using AOT.Models;
using AOT.View;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using System.Windows.Controls;

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
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            bool isChecked = IsPflicht.IsChecked == true;
            WeakReferenceMessenger.Default.Send(new Message(Message.MessageType.Search, BudgetMin.Text, BudgetMax.Text, ProjectName.Text, isChecked) );
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
                MessageBox.Show($"Viewing: {item.Name}");
            }
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            var item = CollectionView.SelectedItem as Project;
            if (item != null)
            {
                DatabaseService db = new();
                db.MoveToDone(item);
            }
            WeakReferenceMessenger.Default.Send(new Message() { Type = Message.MessageType.RefreshUI });
        }

        private void Fail_Click(object sender, RoutedEventArgs e)
        {
            var item = CollectionView.SelectedItem as Project;
            if (item != null)
            {
                DatabaseService db = new();
                db.MoveToFail(item);
            }
            WeakReferenceMessenger.Default.Send(new Message() { Type = Message.MessageType.RefreshUI });
        }
    }
}
