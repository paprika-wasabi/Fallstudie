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
    public partial class PendingPage : UserControl
    {
        readonly PendingPageViewModel _viewModel = new();

        public PendingPage()
        {
            DataContext = _viewModel;
            InitializeComponent();
            this.Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UserBlock.Text = AuthState.UserName;
            RoleBlock.Text = AuthState.Role;
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            var item = CollectionView.SelectedItem as Project;
            if (item != null)
            {
                MessageBox.Show($"Viewing: {item.Name}\n{item.Beschreibung}");
            }
        }

        private void ApproveClick(object sender, RoutedEventArgs e)
        {
            var item = CollectionView.SelectedItem as Project;
            if (item != null)
            {
                DatabaseService db = new();
                db.UpdateProjectStatus(item, "Aktiv");
            }
            WeakReferenceMessenger.Default.Send(new Message() { Type = Message.MessageType.RefreshUI });
        }

        private void DeleteFromPending(object sender, RoutedEventArgs e)
        {
            var item = CollectionView.SelectedItem as Project;
            if (item != null)
            {
                DatabaseService db = new();
                db.DeleteFromPending(item);
            }
            WeakReferenceMessenger.Default.Send(new Message() { Type = Message.MessageType.RefreshUI });
        }
    }
}
