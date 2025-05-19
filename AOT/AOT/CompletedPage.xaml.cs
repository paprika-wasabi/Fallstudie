using System.Windows;
using System.Windows.Controls;

namespace AOT
{
    /// <summary>
    /// Interaktionslogik für HomePage.xaml
    /// </summary>
    public partial class CompletedPage : UserControl
    {
        readonly CompletedPageViewModel _viewModel = new();

        public CompletedPage()
        {
            DataContext = _viewModel;
            InitializeComponent();
        }
    }
}
