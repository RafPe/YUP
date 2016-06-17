using System.Windows;
using System.Windows.Input;

namespace YUP.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            titleBar.MouseLeftButtonDown += (o, e) => DragMove();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
