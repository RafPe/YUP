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
    }
}
