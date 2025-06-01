using System.Configuration;
using System.Data;
using System.Windows;

namespace AOT
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Prevent auto-shutdown when LoginWindow closes
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();

            if (loginWindow.IsAuthenticated)
            {
                try
                {
                    var mainWindow = new MainWindow();
                    this.MainWindow = mainWindow;
                    mainWindow.Show();

                    // Important: reset ShutdownMode AFTER showing MainWindow
                    this.ShutdownMode = ShutdownMode.OnMainWindowClose;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to start MainWindow: " + ex.Message);
                }
            }
            else
            {
                Shutdown();
            }
        }
    }

}
