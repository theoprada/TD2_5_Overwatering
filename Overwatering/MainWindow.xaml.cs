using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Overwatering
{
    /// <summary>
    /// Logique d'interaction pour UCMenuJeu.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AfficheDemarrage();
        }

        private void AfficheDemarrage()
        {
            // crée et charge l'écran de démarrage
            UC_Menu uc = new UC_Menu();

            // associe l'écran au conteneur
            MainWindow.Content = uc;
            uc.butJouer.Click += AfficherJeu;
            uc.butRegles.Click += AfficherRegles;

        }

        private void AfficherJeu(object sender, RoutedEventArgs e)
        {
            UC_Menu uc = new UC_Menu();
            MainWindow.Content = uc;
        }

        private void AfficherRegles(object sender, RoutedEventArgs e)
        {
            UC_Regles uc = new UC_Regles();
            MainWindow.Content = uc;
        }

    }
}