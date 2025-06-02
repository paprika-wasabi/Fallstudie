using AOT.Models;
using AOT.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Configuration;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public FillFormWindow()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
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

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (!decimal.TryParse(textBox.Text, out _))
            {
                MessageBox.Show("Please enter a valid decimal number.");
                textBox.Focus();
            }
        }

        private void SubmitForm_Click(object sender, RoutedEventArgs e)
        {
            var pflicht = "";
            var status = "Aktiv";

            if (IsPflicht.IsChecked == true)
            {
                pflicht = "Ja";
                status = "Ausstehend";


            }
            decimal b;

            if (decimal.TryParse(BudgetBox.Text, out decimal budget))
            {
                b = budget;
            }

            Project project = new Project()
            {
                Budget = budget,
                Name = NameBox.Text,
                Leader = ProjectLeaderComboBox.Text,
                Department = DepartmentName.Text,
                Type = ProjectTypeComboBox.Text,
                PortfolioName = PortfolioComboBox.Text,
                Member = MemberBox.Text,
                KPI = CalulateKPIScore(),
                Date = DateTime.Now.ToString("dd-MM-yyyy"),
                Pflicht = pflicht,
                Status = status,
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

        private async void LeaderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProjectLeaderComboBox.SelectedItem is Leader selectedLeader)
            {
                DatabaseService service = new DatabaseService();
                int id = selectedLeader.DepartmentId;
                DepartmentName.Text = (await service.GetDepartmentByIdAsync(id)).Name;
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var service = new DatabaseService();
            var options1 = await service.GetLeadersAsync();
            var options2 = await service.GetProjectTypesAsync();
            var options3 = await service.GetPortfoliosAsync();
            ProjectLeaderComboBox.ItemsSource = options1;
            ProjectTypeComboBox.ItemsSource = options2;
            PortfolioComboBox.ItemsSource = options3;
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
    }
}
