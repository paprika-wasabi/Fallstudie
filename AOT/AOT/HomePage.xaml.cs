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
            string portfolioName = PortfolioComboBox.Text;

            WeakReferenceMessenger.Default.Send(new Message(Message.MessageType.Search, BudgetMin.Text, BudgetMax.Text, ProjectName.Text, isChecked, ProjectLeaderComboBox.Text, DepartmentComboBox.Text, ProjectTypeComboBox.Text, portfolioName, StatusComboBox.Text));
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

        public bool userAuthorized;
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var service = new DatabaseService();

            var options1 = await service.GetLeadersAsync();
            var options2 = await service.GetProjectTypesAsync();
            var options3 = await service.GetDepartmentsAsync();
            var options4 = await service.GetPortfoliosAsync();

            // Insert empty/default item at the top
            options1.Insert(0, new Leader { Name = "", LastName = "" });
            options2.Insert(0, new ProjectType { Type = "" });
            options3.Insert(0, new Department { Name = "" });
            options4.Insert(0, new Portfolio { Name = "" });

            ProjectLeaderComboBox.ItemsSource = options1;
            ProjectTypeComboBox.ItemsSource = options2;
            DepartmentComboBox.ItemsSource = options3;
            PortfolioComboBox.ItemsSource = options4;


            UserBlock.Text = AuthState.UserName;
            RoleBlock.Text = AuthState.Role;

            if (AuthState.Role == "Manager"){
                userAuthorized = true;
            }
      
            else
            {
                userAuthorized = false;
            }
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
                string[] kpiNames = new[]
                {
                    "Strategischer Beitrag",
                    "Wirtschaftlicher Nutzen",
                    "Dringlichkeit",
                    "Ressourceneffizienz",
                    "Risiko/Komplexität"
                };

                // KPIs mit Namen und Wert paaren, nur falls Liste vorhanden
                string kpiString = item.KPIList != null
                    ? string.Join("\n",
                        kpiNames
                            .Select((name, idx) =>
                                idx < item.KPIList.Count
                                    ? $"{name}: {item.KPIList[idx]}"
                                    : $"{name}: -"))
                    : string.Join("\n", kpiNames.Select(name => $"{name}: -"));

                MessageBox.Show(
                    $"Projektname: {item.Name}\n" +
                    $"Projektnummer: {item.Projektnummer}\n"+
                    $"Projektart: {item.Type}\n"+
                    $"Portfolio: {item.PortfolioName}\n\n\n"+
                    $"Pflichtprojekt: {item.Pflicht}\n"+
                    $"Begründung Pflicht: \n\n"+
                    $"Ausgangslage: {item.Ausgangslage}\n\n"+
                    $"Projektziele: {item.Projektziele}\n\n"+
                    $"Abgrenzungen: {item.Abgrenzungen}\n\n"+
                    $"Meilensteine: {item.Meilensteine}\n\n"+
                    $"Termine: {item.Termine}\n\n\n"+
                    $"Budget:\n"+
                    $"Personenaufwand Beschreibung: {item.Personenaufwand_Beschreibung}\n"+
                    $"Personenaufwand: {item.Personenaufwand}€\n\n"+
                    $"Sachmittel Beschreibung: {item.Sachmittel_Beschreibung}\n" +
                    $"Sachmittel: {item.Sachmittel}€\n" +
                    $"Budget gesamt: {item.Budget}€\n\n\n"+
                    $"Auftraggeber: {item.Auftraggeber}\n"+
                    $"Projektleiter: {item.Leader}\n"+
                    $"Abteilung: {item.Department}\n"+
                    $"Stakeholder: {item.Stakeholder}\n"+
                    $"Verteiler: {item.Verteiler}\n\n"+
                    $"KPIs:\n{kpiString}"
                );
            }

            
        }

        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            if (!userAuthorized)
            {
                // Option ist deaktiviert, zeige Fehlermeldung an
                MessageBox.Show("Sie sind nicht berechtigt, diese Aktion auszuführen.", "Zugriff verweigert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Setze den Projektstatus auf "Genehmigt"
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
            if (!userAuthorized)
            {
                // Option ist deaktiviert, zeige Fehlermeldung an
                MessageBox.Show("Sie sind nicht berechtigt, diese Aktion auszuführen.", "Zugriff verweigert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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
            if (!userAuthorized)
            {
                // Option ist deaktiviert, zeige Fehlermeldung an
                MessageBox.Show("Sie sind nicht berechtigt, diese Aktion auszuführen.", "Zugriff verweigert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Delete the document
            var item = CollectionView.SelectedItem as Project;
            if (item != null)
            {
                DatabaseService db = new();
                if (db.DeleteProject(item))
                {
                    WeakReferenceMessenger.Default.Send(new Message() { Type = Message.MessageType.RefreshUI });
                }
                else
                {
                    MessageBox.Show("An error occurred while deleting the project.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Aktuellen Prozesspfad holen
            var exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            // Neue Instanz starten
            System.Diagnostics.Process.Start(exePath);
            // Aktuelle Instanz beenden
            Application.Current.Shutdown();
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
