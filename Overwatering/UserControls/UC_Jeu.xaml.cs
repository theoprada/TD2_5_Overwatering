using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Overwatering
{
    public partial class UC_Jeu : UserControl
    {
        // --- 1. VARIABLES ---
        DispatcherTimer gameTimer = new DispatcherTimer();

        // Déplacement
        double vitesse = 2; // Reduced from 4 to 2 to slow movement
        double persoX = 375;
        double persoY = 200;

        // Touches actives
        bool haut, bas, gauche, droite;

        // Paramètres
        bool utiliseZQSD = true; // Sera chargé depuis MainWindow

        // Animation
        // On retient la dernière direction pour savoir quelle image afficher quand on s'arrête
        string direction = "Recule"; // Valeurs possibles: "Avance", "Recule", "Gauche", "Droite"
        int numImage = 1; // 1 ou 2 pour l'effet de marche
        int compteurTemps = 0; // Ralentisseur d'animation

        public UC_Jeu()
        {
            InitializeComponent();

            // Configuration du Timer (Boucle de jeu)
            // Utiliser CompositionTarget.Rendering pour des mises à jour synchronisées au rendu (plus fluide)
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            GameLoop(sender, e);
        }

        private void UC_Jeu_Loaded(object sender, RoutedEventArgs e)
        {
            // IMPORTANT : Donner le focus au jeu pour capter le clavier
            this.Focus();
            Keyboard.Focus(this);

            // Récupérer le choix des touches (ZQSD ou Flèches)
            if (Application.Current.MainWindow is MainWindow mw)
            {
                utiliseZQSD = (mw.TypeControle == "ZQSD");
            }

            // Afficher le perso tout de suite (à l'arrêt)
            MettreAJourSprite(true);
        }

        // --- 2. BOUCLE DE JEU ---
        private void GameLoop(object sender, EventArgs e)
        {
            // Si le menu pause est visible, on ne bouge pas
            if (MenuPauseOverlay.Visibility == Visibility.Visible) return;

            bool bouge = false;

            // Déplacement
            if (haut) { persoY -= vitesse; direction = "Avance"; bouge = true; }
            if (bas) { persoY += vitesse; direction = "Recule"; bouge = true; }
            if (gauche) { persoX -= vitesse; direction = "Gauche"; bouge = true; }
            if (droite) { persoX += vitesse; direction = "Droite"; bouge = true; }

            // Collisions avec les bords de l'écran (800x450)
            if (persoX < 0) persoX = 0;
            if (persoY < 0) persoY = 0;
            if (persoX > 750) persoX = 750; // Largeur fenêtre - Largeur perso
            if (persoY > 390) persoY = 390; // Hauteur fenêtre - Hauteur perso

            // Appliquer la position
            Canvas.SetLeft(ImgPerso, persoX);
            Canvas.SetTop(ImgPerso, persoY);

            // Gérer l'animation
            if (bouge)
            {
                compteurTemps++;
                // Changer d'image toutes les 10 frames (pour pas clignoter trop vite)
                if (compteurTemps > 10)
                {
                    compteurTemps = 0;
                    if (numImage == 1) numImage = 2; else numImage = 1; // Alterne 1 et 2
                    MettreAJourSprite(false); // false = il marche
                }
            }
            else
            {
                // Si on ne bouge pas, on remet l'image neutre
                MettreAJourSprite(true); // true = à l'arrêt
            }
        }

        // --- 3. GESTION DE L'IMAGE (SPRITE) ---
        private void MettreAJourSprite(bool estArrete)
        {
            // Construction du nom de fichier selon tes assets
            // Exemple : "persoAvance1.png" ou "persoAvanceNeutre.png"
            // Dossier : Assets/ImagesJeu/

            string suffixe;
            if (estArrete)
                suffixe = "Neutre";
            else
                suffixe = numImage.ToString(); // "1" ou "2"

            string nomFichier = "perso" + direction + suffixe + ".png";

            // Chemin complet pour WPF
            string chemin = $"/Assets/ImagesJeu/{nomFichier}";

            try
            {
                ImgPerso.Source = new BitmapImage(new Uri(chemin, UriKind.Relative));
            }
            catch
            {
                // Si une image manque, ça ne plante pas le jeu
            }
        }

        // --- 4. CLAVIER ---
        private void UC_Jeu_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Intercepter les touches de déplacement ici pour empêcher la navigation entre contrôles
            if (utiliseZQSD)
            {
                if (e.Key == Key.Z) { haut = true; e.Handled = true; }
                if (e.Key == Key.S) { bas = true; e.Handled = true; }
                if (e.Key == Key.Q) { gauche = true; e.Handled = true; }
                if (e.Key == Key.D) { droite = true; e.Handled = true; }
            }
            else
            {
                if (e.Key == Key.Up) { haut = true; e.Handled = true; }
                if (e.Key == Key.Down) { bas = true; e.Handled = true; }
                if (e.Key == Key.Left) { gauche = true; e.Handled = true; }
                if (e.Key == Key.Right) { droite = true; e.Handled = true; }
            }

            if (e.Key == Key.Escape)
            {
                TogglePause();
                e.Handled = true;
            }
        }

        private void UC_Jeu_KeyDown(object sender, KeyEventArgs e)
        {
            // Garde la compatibilité - traite aussi et marque handled
            if (e.Key == Key.Escape) { TogglePause(); e.Handled = true; }

            if (utiliseZQSD)
            {
                if (e.Key == Key.Z) { haut = true; e.Handled = true; }
                if (e.Key == Key.S) { bas = true; e.Handled = true; }
                if (e.Key == Key.Q) { gauche = true; e.Handled = true; }
                if (e.Key == Key.D) { droite = true; e.Handled = true; }
            }
            else // Flèches
            {
                if (e.Key == Key.Up) { haut = true; e.Handled = true; }
                if (e.Key == Key.Down) { bas = true; e.Handled = true; }
                if (e.Key == Key.Left) { gauche = true; e.Handled = true; }
                if (e.Key == Key.Right) { droite = true; e.Handled = true; }
            }
        }

        private void UC_Jeu_KeyUp(object sender, KeyEventArgs e)
        {
            if (utiliseZQSD)
            {
                if (e.Key == Key.Z) haut = false;
                if (e.Key == Key.S) bas = false;
                if (e.Key == Key.Q) gauche = false;
                if (e.Key == Key.D) droite = false;
            }
            else
            {
                if (e.Key == Key.Up) haut = false;
                if (e.Key == Key.Down) bas = false;
                if (e.Key == Key.Left) gauche = false;
                if (e.Key == Key.Right) droite = false;
            }

            // Marquer handled pour éviter navigation résiduelle
            e.Handled = true;
        }

        // --- 5. PAUSE ---
        private void TogglePause()
        {
            if (MenuPauseOverlay.Visibility == Visibility.Visible)
            {
                MenuPauseOverlay.Visibility = Visibility.Collapsed;
                this.Focus(); // Redonner le focus au jeu
                Keyboard.Focus(this);
            }
            else
            {
                MenuPauseOverlay.Visibility = Visibility.Visible;
            }
        }

        private void ButPause_Click(object sender, RoutedEventArgs e) => TogglePause();
        private void ButReprendre_Click(object sender, RoutedEventArgs e) => TogglePause();

        private void ButQuitterMenu_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw) mw.AfficheMenu();
        }
    }
}