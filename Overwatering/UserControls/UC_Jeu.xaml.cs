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
        private const double VITESSE = 5.0;
        public UC_Jeu()
        {
            InitializeComponent();
        }

        private void UC_Jeu_Loaded(object sender, RoutedEventArgs e)
        {
            // C'est cette ligne qui fait la magie !
            this.Focus();
        }

        private void UC_Jeu_KeyDown(object sender, KeyEventArgs e)
        {
            // Si la touche appuyée est "Escape" (Échap)
            if (e.Key == Key.Escape)
            {
                // On vérifie l'état actuel de l'overlay :
                if (MenuPauseOverlay.Visibility == Visibility.Visible)
                {
                    // Si le menu est ouvert, on le ferme (Reprendre)
                    ButReprendre_Click(null, null);
                }
                else
                {
                    // Si le menu est fermé, on l'ouvre (Pause)
                    ButPause_Click(null, null);
                }

                // Empêche l'événement de se propager plus loin (optionnel mais recommandé)
                e.Handled = true;
            }
            string typeControle = "ZQSD"; // Valeur par défaut si MainWindow est introuvable
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow is MainWindow mainWindow)
            {
                typeControle = mainWindow.TypeControle;
            }

            double newX = Canvas.GetLeft(ImgPerso);
            double newY = Canvas.GetTop(ImgPerso);

            bool aBouge = false; // Pour savoir si on a appuyé sur une touche de direction
            if (typeControle == "ZQSD")
            {
                switch (e.Key)
                {
                    case Key.Z: // Haut
                        newY -= VITESSE;
                        // ... Mise à jour du sprite HAUT ...
                        aBouge = true;
                        break;
                    case Key.S: // Bas
                        newY += VITESSE;
                        // ... Mise à jour du sprite BAS ...
                        aBouge = true;
                        break;
                    case Key.Q: // Gauche
                        newX -= VITESSE;
                        // ... Mise à jour du sprite GAUCHE ...
                        aBouge = true;
                        break;
                    case Key.D: // Droite
                        newX += VITESSE;
                        // ... Mise à jour du sprite DROITE ...
                        aBouge = true;
                        break;
                }
            }
            else if (typeControle == "Flèches directionnelles") // Si les flèches sont choisies
            {
                switch (e.Key)
                {
                    case Key.Up: // Haut
                        newY -= VITESSE;
                        // ... Mise à jour du sprite HAUT ...
                        aBouge = true;
                        break;
                    case Key.Down: // Bas
                        newY += VITESSE;
                        // ... Mise à jour du sprite BAS ...
                        aBouge = true;
                        break;
                    case Key.Left: // Gauche
                        newX -= VITESSE;
                        // ... Mise à jour du sprite GAUCHE ...
                        aBouge = true;
                        break;
                    case Key.Right: // Droite
                        newX += VITESSE;
                        // ... Mise à jour du sprite DROITE ...
                        aBouge = true;
                        break;
                }
            }

            // ... (Gestion de l'application de la position et de e.Handled) ...
            if (aBouge)
            {
                Canvas.SetLeft(ImgPerso, newX);
                Canvas.SetTop(ImgPerso, newY);
                e.Handled = true;
            }
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
            this.Focus();
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
