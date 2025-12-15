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
    public partial class UC_Jeu : UserControl
    {
        public UC_Jeu()
        {
            InitializeComponent();
        }

        private void ButPause_Click(object sender, RoutedEventArgs e)
        {
            MenuPauseOverlay.Visibility = Visibility.Visible;
            // estEnPause = true; // Si tu gères la boucle du jeu
            // Si tu as la musique de jeu, tu devras faire Pause() ou Stop() ici
        }

        // 2. Reprendre le Jeu
        private void ButReprendre_Click(object sender, RoutedEventArgs e)
        {
            MenuPauseOverlay.Visibility = Visibility.Collapsed;
            // estEnPause = false; // Si tu gères la boucle du jeu
            // Si tu as la musique de jeu, tu devras faire Play() ici
        }

        // 3. Quitter le Jeu pour le Menu Principal
        private void ButQuitterMenu_Click(object sender, RoutedEventArgs e)
        {
            // 3.1 Trouver la fenêtre parente (MainWindow)
            Window parentWindow = Window.GetWindow(this);

            // 3.2 S'assurer que c'est la MainWindow et appeler la méthode de retour au menu
            if (parentWindow is MainWindow mainWindow)
            {
                // Utilise la méthode que tu as dans MainWindow (AfficherMenu() ou AfficherMenuPrincipal())
                mainWindow.AfficheMenu();
            }
        }
    }
}
