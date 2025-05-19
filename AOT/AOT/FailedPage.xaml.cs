using System.Windows;
using System.Windows.Controls;

namespace AOT
{
    /// <summary>
    /// Interaktionslogik für HomePage.xaml
    /// </summary>
    public partial class FailedPage : UserControl
    {
        readonly FailedPageViewModel _viewModel = new();
        public FailedPage()
        {
            DataContext = _viewModel;
            InitializeComponent();
        }
    }
}
