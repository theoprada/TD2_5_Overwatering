using System.Windows;
using Overwatering.UserControls;
using System.Windows.Controls;

namespace Overwatering
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AfficheMenu();
        }

        private void AfficheMenu()
        {
            // création écran menu
            UC_Menu uc = new UC_Menu();
            FenetrePrincipal.Content = uc;

            //boutons menu
            uc.butJouer.Click += AfficherJeu;
            uc.butRegles.Click += AfficherRegles;

        }

        private void AfficherJeu(object sender, RoutedEventArgs e)
        {
            UC_Jeu uc = new UC_Jeu();
            FenetrePrincipal.Content = uc;
        }

        private void AfficherRegles(object sender, RoutedEventArgs e)
        {
            UC_Regles uc = new UC_Regles();
            FenetrePrincipal.Content = uc;
        }

    }
}