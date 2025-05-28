using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;

namespace AOT.ViewModel
{
    class DashboardViewModel
    {
        DatabaseService databaseService = new();
        public ISeries[] Series { get; set; }
        
        public DashboardViewModel()
        {
            Series =
            [
                //new PieSeries<double> { Values = new double[] { databaseService.CountActiveProjects() }, Name = "Active Projekte"},
                //new PieSeries<double> { Values = new double[] { databaseService.CountFailedProjects() }, Name = "Failed Projekte"},
                //new PieSeries<double> { Values = new double[] { databaseService.CountCompletedProjects() }, Name = "Completed Projekte" },
                
                // display all portfolios (Sparmassnahmen, Expansion, Digitalisierung, Umweltschutz)
                new PieSeries<double> { Values = new double[] { databaseService.CountActiveProjectsByPortfolio("Sparmassnahmen") }, Name = "Sparmassnahmen" },
                new PieSeries<double> { Values = new double[] { databaseService.CountActiveProjectsByPortfolio("Expansion") }, Name = "Expansion" },
                new PieSeries<double> { Values = new double[] { databaseService.CountActiveProjectsByPortfolio("Digitalisierung") }, Name = "Digitalisierung" },
                new PieSeries<double> { Values = new double[] { databaseService.CountActiveProjectsByPortfolio("Umweltschutz") }, Name = "Umweltschutz" },

            ];
        }
    }
}
