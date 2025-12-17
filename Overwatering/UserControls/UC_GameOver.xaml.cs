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
            if (Application.Current.MainWindow is MainWindow mw2) mw2.JouerSonBouton();
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.LancerNouvellePartie();
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw2) mw2.JouerSonBouton();
            if (Application.Current.MainWindow is MainWindow mw) mw.AfficheMenu();
        }
    }
}