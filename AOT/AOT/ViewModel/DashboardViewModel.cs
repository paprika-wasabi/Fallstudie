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
                new PieSeries<double> { Values = new double[] { databaseService.CountActiveProjects() }, Name = "Active Projekte"},
                new PieSeries<double> { Values = new double[] { databaseService.CountFailedProjects() }, Name = "Failed Projekte"},
                new PieSeries<double> { Values = new double[] { databaseService.CountCompletedProjects() }, Name = "Completed Projekte" },
            ];
        }
    }
}
