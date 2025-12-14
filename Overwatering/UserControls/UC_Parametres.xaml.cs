using System.Windows;
using System.Windows.Controls;

namespace Overwatering
{
    public partial class UC_Parametres : UserControl
    {
        public UC_Parametres()
        {
            InitializeComponent();
        }

        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw) mw.AfficheMenu();
        }
    }
}