using Microsoft.Win32;
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

namespace DotNet8.Wpf.OpenFolderDialog
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var openFolderDialog = new Microsoft.Win32.OpenFolderDialog()
            {
                Title = "Select folder to open ...",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            };

            string folderName = "";
            var result = openFolderDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                folderName = openFolderDialog.FolderName;
                MessageBox.Show(folderName);
            }
        }
    }
}