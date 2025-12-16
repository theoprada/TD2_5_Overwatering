using System.Windows;
using System.Windows.Controls;

namespace Overwatering
{

    public partial class UC_Menu : UserControl
    {
        public UC_Menu()
        {
            InitializeComponent();
        }

        private void btnJouer_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                // On appelle la méthode qui remet à zéro les vies et le jardin
                mw.LancerNouvellePartie();
            }
        }

        private void btnRegles_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw) mw.AfficheRegles();
        }

        private void btnParametres_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw) mw.AfficheParametres();
        }

        private void btnCredits_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw) mw.AfficheCredits();
        }

        private void btnQuitter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}