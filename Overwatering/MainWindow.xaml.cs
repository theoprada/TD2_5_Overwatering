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
            UCJeu uc = new UCJeu();

            // associe l'écran au conteneur
            ZoneJeu.Content = uc;
            uc.butJouer.Click += AfficherJeu;

        }

        private void AfficherJeu(object sender, RoutedEventArgs e)
        {
            UCJeu uc = new UCJeu();
            ZoneJeu.Content = uc;
        }

    }
}