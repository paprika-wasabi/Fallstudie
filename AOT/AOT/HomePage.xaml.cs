using AOT.Models;
using AOT.View;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Text;

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
                    $"Begründung Pflicht: {item.BegründungPflicht}\n\n"+
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
                    $"KPIs:\n{kpiString}\n\n"+
                    $"Erstellt von: {item.Creator}\n"+
                    $"Erstellt am: {item.Date}"
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

            // The logged in user is not authorized his own projects, check if the createor attribute matches the logged in user
            var item = CollectionView.SelectedItem as Project;
            if (item != null && item.Creator == AuthState.UserName)
            {
                MessageBox.Show("Sie sind nicht berechtigt, diesen Projektstatus zu ändern.", "Zugriff verweigert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            // Setze den Projektstatus auf "Genehmigt"
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

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected project
            var project = CollectionView.SelectedItem as Project;
            if (project == null)
            {
                MessageBox.Show("Bitte wählen Sie ein Projekt zum Exportieren aus.", "Kein Projekt ausgewählt", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Configure save file dialog
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Projekt als CSV speichern",
                Filter = "CSV-Dateien|*.csv",
                DefaultExt = ".csv",
                FileName = $"{project.Name.Replace(" ", "_")}.csv"
            };

            // Show save dialog
            if (saveFileDialog.ShowDialog() != true)
                return;

            try
            {
                // Create CSV content
                string csvContent = $@"Projektname;{project.Name}
                Projektnummer;{project.Projektnummer}
                Projektart;{project.Type}
                Portfolio;{project.PortfolioName}
                Pflichtprojekt;{project.Pflicht}
                Begründung Pflicht;{project.BegründungPflicht}
                Ausgangslage;{project.Ausgangslage}
                Projektziele;{project.Projektziele}
                Abgrenzungen;{project.Abgrenzungen}
                Meilensteine;{project.Meilensteine}
                Termine;{project.Termine}
                Personenaufwand Beschreibung;{project.Personenaufwand_Beschreibung}
                Personenaufwand (€);{project.Personenaufwand}
                Sachmittel Beschreibung;{project.Sachmittel_Beschreibung}
                Sachmittel (€);{project.Sachmittel}
                Budget gesamt (€);{project.Budget}
                Auftraggeber;{project.Auftraggeber}
                Projektleiter;{project.Leader}
                Abteilung;{project.Department}
                Stakeholder;{project.Stakeholder}
                Verteiler;{project.Verteiler}
                Strategischer Beitrag;{(project.KPIList != null && project.KPIList.Count > 0 ? project.KPIList[0] : "")}
                Wirtschaftlicher Nutzen;{(project.KPIList != null && project.KPIList.Count > 1 ? project.KPIList[1] : "")}
                Dringlichkeit;{(project.KPIList != null && project.KPIList.Count > 2 ? project.KPIList[2] : "")}
                Ressourceneffizienz;{(project.KPIList != null && project.KPIList.Count > 3 ? project.KPIList[3] : "")}
                Risiko/Komplexität;{(project.KPIList != null && project.KPIList.Count > 4 ? project.KPIList[4] : "")}
                Erstellt von;{project.Creator}
                Erstellt am;{project.Date}";

                // Write to file
                File.WriteAllText(saveFileDialog.FileName, csvContent, Encoding.UTF8);
                MessageBox.Show("Projekt erfolgreich exportiert!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Export: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
