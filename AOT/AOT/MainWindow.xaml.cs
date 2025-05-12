using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AOT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenForm_Click(object sender, RoutedEventArgs e)
        {
            FillFormWindow formWindow = new();
            formWindow.Owner = this; // optional: set owner to block main window
            formWindow.ShowDialog(); // or use Show() if you want it non-modal
        }
    }
}