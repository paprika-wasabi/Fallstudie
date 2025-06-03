using AOT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AOT
{
    /// <summary>
    /// Interaktionslogik für PreviewWindow.xaml
    /// </summary>
    public partial class PreviewWindow : Window
    {
        private Project project { get; set; }
        private PdfFile pdfFile { get; set; }

        public PreviewWindow(Project item)
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
            project = item;

            // KPI-Formatierung
            string[] kpiNames = new[]
            {
            "Strategischer Beitrag",
            "Wirtschaftlicher Nutzen",
            "Dringlichkeit",
            "Ressourceneffizienz",
            "Risiko/Komplexität"
        };

            var kpiFormatted = project.KPIList != null
                ? kpiNames.Select((name, idx) =>
                    idx < project.KPIList.Count ? $"{name}: {project.KPIList[idx]}" : $"{name}: -").ToList()
                : kpiNames.Select(name => $"{name}: -").ToList();

            // Datenbindung vorbereiten
            DataContext = new
            {
                project.Name,
                project.Projektnummer,
                project.Type,
                project.PortfolioName,
                project.Pflicht,
                project.BegründungPflicht,
                project.Ausgangslage,
                project.Projektziele,
                project.Abgrenzungen,
                project.Meilensteine,
                project.Termine,
                project.Personenaufwand_Beschreibung,
                project.Personenaufwand,
                project.Sachmittel_Beschreibung,
                project.Sachmittel,
                project.Budget,
                project.Auftraggeber,
                project.Leader,
                project.Department,
                project.Stakeholder,
                project.Verteiler,
                project.Creator,
                project.Date,
                KPIFormatted = kpiFormatted
            };
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadPdfFile();
        }

        private void ShowPdf_Click(object sender, RoutedEventArgs e)
        {
            string tempFile = System.IO.Path.GetTempFileName().Replace(".tmp", ".pdf");
            File.WriteAllBytes(tempFile, pdfFile.Data);

            // Open in default PDF viewer:
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = tempFile,
                UseShellExecute = true
            });
        }

        public async Task LoadPdfFile()
        {
            DatabaseService service = new DatabaseService();
            pdfFile = await service.GetPdfFileById(project.PdfObjectId);
            if (pdfFile == null)
            {
                ShowPfdButton.Visibility = Visibility.Collapsed;
                return;
            }
        }
    }
}
