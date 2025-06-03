using AOT.Models;
using AOT.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Win32;
using MongoDB.Driver;
using System.Configuration;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using 

namespace AOT
{
    /// <summary>
    /// Interaktionslogik für FillFormWindow.xaml
    /// </summary>
    public partial class FillFormWindow : Window
    {
        private int Strategischer_Beitrag;
        private int Wirtschaftlicher_Nutzen;
        private int Dringlichkeit;
        private int Ressourceneffizienz;
        private int Risiko_Komplexitaet;

        private int budget;


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


        private void BudgetBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // If the BudgetPersonalaufwandBox is not empty and the BudgetSachmittelBox is not empty, calculate the total budget
            if (!string.IsNullOrEmpty(BudgetPersonenaufwandBox.Text) && !string.IsNullOrEmpty(BudgetSachmittelBox.Text))
            {
                if (decimal.TryParse(BudgetPersonenaufwandBox.Text, out decimal personalaufwand) && decimal.TryParse(BudgetSachmittelBox.Text, out decimal sachmittel))
                {
                    BudgetBox.Text = (personalaufwand + sachmittel).ToString();
                }
                else
                {
                    MessageBox.Show("Bitte geben Sie gültige Zahlen für Personalaufwand und Sachmittel ein.", "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

        }

        private void IsPflicht_Checked(object sender, RoutedEventArgs e)
        {
            // Set focusable to true for the BegründungPflichtBox when the checkbox is checked
            BegründungPflichtBox.Focusable = true;

        }

        private void IsPflicht_Unchecked(object sender, RoutedEventArgs e)
        {
            // Set focusable to false for the BegründungpflichtBox when the checkbox is unchecked
            BegründungPflichtBox.Focusable = false;
        }

        private async void UploadPDF_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                Title = "Select a PDF file"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var fileBytes = File.ReadAllBytes(openFileDialog.FileName);
                var pdfDoc = new PdfFile
                {
                    FileName = Path.GetFileName(openFileDialog.FileName),
                    Data = fileBytes
                };

                var client = new MongoClient("mongodb://localhost:27017");
                var database = client.GetDatabase("your_database_name");
                var collection = database.GetCollection<PdfFile>("pdf_files");

                await collection.InsertOneAsync(pdfDoc);

                MessageBox.Show("PDF uploaded successfully.");
            }
        }

        private void SubmitForm_Click(object sender, RoutedEventArgs e)
        {
            var pflicht = "";
            var status = "Neu";
            int pflichtKPIPoint = 0;

            if (IsPflicht.IsChecked == true)
            {
                pflicht = "Ja";
                status = "Neu";
                pflichtKPIPoint = 20;


            }
            decimal b;

            if (decimal.TryParse(BudgetBox.Text, out decimal budget))
            {
                b = budget;
            }

            if(pflicht == "Ja" && string.IsNullOrEmpty(BegründungPflichtBox.Text))
{
                MessageBox.Show("Bitte geben Sie eine Begründung für die Pflichtangabe ein.", "Fehlende Begründung", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (UnterschriftCheckBox.IsChecked == false)
            {
                MessageBox.Show("Bitte bestätigen Sie die Unterschrift.", "Fehlende Unterschrift", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(Strategischer_Beitrag == 0 || Wirtschaftlicher_Nutzen == 0 || Dringlichkeit == 0 || Ressourceneffizienz == 0 || Risiko_Komplexitaet == 0)
            {
                MessageBox.Show("Bitte bewerten Sie alle KPIs.", "Fehlende Bewertung", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }   


            Project project = new Project()
            {
                Name = NameBox.Text,
                Projektnummer = NumberBox.Text,
                Type = ProjectTypeComboBox.Text,
                PortfolioName = PortfolioComboBox.Text,
                Pflicht = pflicht,
                BegründungPflicht = BegründungPflichtBox.Text,
                Ausgangslage = AusgangslageBox.Text,
                Projektziele = ZieleBox.Text,
                Abgrenzungen = AbgrenzungenBox.Text,
                Meilensteine = MeilensteineBox.Text,
                Termine = TermineBox.Text,

                Personenaufwand_Beschreibung = PersonenaufwandBeschreibungBox.Text,
                Personenaufwand = BudgetPersonenaufwandBox.Text,
                Sachmittel_Beschreibung= SachmittelBeschreibungBox.Text,
                Sachmittel = BudgetSachmittelBox.Text,
                Budget = budget,

                Auftraggeber = AuftraggeberBox.Text,

                Leader = ProjectLeaderComboBox.Text,
                Department = DepartmentName.Text,

                Member = MemberBox.Text,
                Stakeholder = StakeholderBox.Text,
                Verteiler = VerteilerBox.Text,

                KPI = CalulateKPIScore() + pflichtKPIPoint,
                KPIList = new List<int> {Strategischer_Beitrag, Wirtschaftlicher_Nutzen, Dringlichkeit, Ressourceneffizienz, Risiko_Komplexitaet },
                Date = DateTime.Now.ToString("dd-MM-yyyy"),
                
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

            // Risiko_Komplexitaet is inverted
            Risiko_Komplexitaet = 5 + 1 - Risiko_Komplexitaet;
            return ((((Strategischer_Beitrag * 30) + (Wirtschaftlicher_Nutzen * 25) + (Dringlichkeit * 15) + (Ressourceneffizienz * 10) + (Risiko_Komplexitaet * 10)) / 90 ) * 20);
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
            Strategischer_Beitrag = ConvertToInteger(sender as RadioButton);
        }

        private void KPI02_Checked(object sender, RoutedEventArgs e)
        {
            Wirtschaftlicher_Nutzen = ConvertToInteger(sender as RadioButton);
        }

        private void KPI03_Checked(object sender, RoutedEventArgs e)
        {
            Dringlichkeit = ConvertToInteger(sender as RadioButton);
        }

        private void KPI04_Checked(object sender, RoutedEventArgs e)
        {
            Ressourceneffizienz = ConvertToInteger(sender as RadioButton);
        }

        private void KPI05_Checked(object sender, RoutedEventArgs e)
        {
            Risiko_Komplexitaet = ConvertToInteger(sender as RadioButton);
        }
    }
}
