using System.Windows;
using System.Windows.Controls;
namespace Overwatering
{
    public partial class UC_Credits : UserControl
    {
        public UC_Credits()
        {
            InitializeComponent();
        }

        private void butRetour_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw2) mw2.JouerSonBouton();
            if (Application.Current.MainWindow is MainWindow mw) mw.AfficheMenu();
        }
    }
}