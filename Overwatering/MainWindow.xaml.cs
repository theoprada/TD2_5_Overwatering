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
            AfficheJeu();
        }

        private void AfficheJeu()
        {
            // crée et charge l'écran de démarrage
            UCJeu uc = new UCJeu();

            // associe l'écran au conteneur
            ZoneJeu.Content = uc;
            uc.butDemarrer.Click += AfficherChoixPerso;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window1 parametreWindow = new Window1();
            bool? rep = parametreWindow.ShowDialog();
        }
    }
}