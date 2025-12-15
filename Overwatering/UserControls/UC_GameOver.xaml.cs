using System.Windows;
using System.Windows.Controls;

namespace Overwatering
{
    public partial class UC_GameOver : UserControl
    {
        public UC_GameOver()
        {
            InitializeComponent();
        }

        private void Rejouer_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw) mw.LancerJeu();
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw) mw.AfficheMenu();
        }
    }
}