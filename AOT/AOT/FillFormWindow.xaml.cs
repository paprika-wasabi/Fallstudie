using AOT.Models;
using AOT.View;
using CommunityToolkit.Mvvm.Messaging;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace AOT
{
    /// <summary>
    /// Interaktionslogik für FillFormWindow.xaml
    /// </summary>
    public partial class FillFormWindow : Window
    {
        private int KPI01;
        private int KPI02;
        private int KPI03;
        private int KPI04;
        private int KPI05;
        private bool KPI06;

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
                KPI = CalulateKPIScore(),
                Date = DateTime.Now.ToString("dd-MM-yyyy"),
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

            WeakReferenceMessenger.Default.Send(new Message() { Type = Message.MessageType.RefreshUI });
        }

        private float CalulateKPIScore()
        {
            return (((KPI01 * 30) + (KPI02 * 25) + (KPI03 * 15) + (KPI04 * 10) + (KPI05 * 10)) * 20 ) / 90;
        }

        private int ConvertToInteger(object sender)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null)
            {
                string value = selected.Content.ToString();
                return int.Parse(value);
            }
            else
            {
                throw new InvalidOperationException("Missing Value");
            }
        }

        private void KPI01_Checked(object sender, RoutedEventArgs e)
        {
            KPI01 = ConvertToInteger(sender as RadioButton);
        }

        private void KPI02_Checked(object sender, RoutedEventArgs e)
        {
            KPI02 = ConvertToInteger(sender as RadioButton);
        }

        private void KPI03_Checked(object sender, RoutedEventArgs e)
        {
            KPI03 = ConvertToInteger(sender as RadioButton);
        }

        private void KPI04_Checked(object sender, RoutedEventArgs e)
        {
            KPI04 = ConvertToInteger(sender as RadioButton);
        }

        private void KPI05_Checked(object sender, RoutedEventArgs e)
        {
            KPI05 = ConvertToInteger(sender as RadioButton);
        }

        private void KPI06_Checked(object sender, RoutedEventArgs e)
        {
            if (Ja.IsChecked == true)
            {
                KPI06 = true;
            }
            else
            {
                KPI06 = false;
            }
        }
    }
}
