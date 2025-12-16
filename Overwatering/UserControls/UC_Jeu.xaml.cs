using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Overwatering
{
    public enum TypeFleur
    {
        Rose,
        Tulipe,
        Lys
    }

    // 2. Représenter une commande de client
    public class CommandeClient
    {
        public TypeFleur FleurDemandee { get; private set; }
        public int QuantiteDemandee { get; private set; }

        public CommandeClient(TypeFleur fleur, int quantite = 2)
        {
            FleurDemandee = fleur;
            QuantiteDemandee = quantite;
        }

        public override string ToString()
        {
            return $"{QuantiteDemandee}x {FleurDemandee}";
        }
    }
    public partial class UC_Jeu : UserControl
    {
        // --- 1. VARIABLES ---
        private Random _random = new Random();
        private const double VITESSE = 5.0;
        private DispatcherTimer _timerClients;

        private bool _clientPresent = false;
        private CommandeClient _commandeActuelle;
        

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
        private List<System.Windows.Shapes.Rectangle> _zonesCollisions;
        public UC_Jeu()
        {
            InitializeComponent();
            InitialiserTimerClients();
            InitialiserCollisions();

            // Configuration du Timer (Boucle de jeu)
            // Utiliser CompositionTarget.Rendering pour des mises à jour synchronisées au rendu (plus fluide)
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }
        private void InitialiserCollisions()
        {
            _zonesCollisions = new List<System.Windows.Shapes.Rectangle>()
            {
                ColisionPuit,
                ColisionArbreGauche,
                ColisionArbreDroit,
                ColisionArbreHautGauche,
                ColisionArbreHautDroit,
                ColisionArbreBas
            };

            foreach (var collision in _zonesCollisions)
            {
                collision.Visibility = Visibility.Collapsed;
            }
            
        }

        private void InitialiserTimerClients()
        {
            _timerClients = new DispatcherTimer();
            _timerClients.Interval = TimeSpan.FromSeconds(5);
            _timerClients.Tick += TimerClients_Tick;
            _timerClients.Start();
        }

        private void TimerClients_Tick(object sender, EventArgs e)
        {
            if (!_clientPresent)
            {
                FaireApparaitreNouveauClient();
            }
        }

        private void FaireApparaitreNouveauClient()
        {
            _clientPresent = true;
            // ... (Logique d'affichage du client) ...
            ImgClient.Visibility = Visibility.Visible;

            _commandeActuelle = GenererCommandeAleatoire();
            TxtFleurDemandee.Text = $"Fleur: {_commandeActuelle.FleurDemandee}";
            BulleCommande.Visibility = Visibility.Visible;
        }

        private CommandeClient GenererCommandeAleatoire()
        {
            Array values = Enum.GetValues(typeof(TypeFleur));
            TypeFleur randomFleur = (TypeFleur)values.GetValue(_random.Next(values.Length));

            int randomQuantite = _random.Next(1, 4);
            return new CommandeClient(randomFleur, randomQuantite);
        }

        public void ServirClient()
        {
            if (_clientPresent && _commandeActuelle != null)
            {
                // TODO: Vérifier si le joueur a les bonnes fleurs en stock
                // Pour l'instant, on suppose que c'est bon et on fait partir le client

                MessageBox.Show($"Client servi avec {_commandeActuelle.ToString()}!");
                FairePartirClient();
            }
        }

        // NOUVEAU : Faire partir le client
        private void FairePartirClient()
        {
            _clientPresent = false;
            ImgClient.Visibility = Visibility.Collapsed;
            BulleCommande.Visibility = Visibility.Collapsed;
            _commandeActuelle = null; // Réinitialiser la commande
            // Le timer fera apparaître un nouveau client après son prochain déclenchement
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

            if (MenuPauseOverlay.Visibility == Visibility.Visible) return;

            // Position actuelle
            double currentX = Canvas.GetLeft(ImgJoueur);
            double currentY = Canvas.GetTop(ImgJoueur);

            // Position potentielle (nouvelle position)
            double potentialX = currentX;
            double potentialY = currentY;

            // Hauteur et Largeur du personnage (pour la zone de collision du joueur)
            double joueurWidth = ImgJoueur.Width;
            double joueurHeight = ImgJoueur.Height;
            bool aBouge = false;

            // 1. CALCUL DE LA NOUVELLE POSITION POTENTIELLE
            // (On utilise potentialX/Y au lieu de newX/Y)

            // ... Ton code pour déterminer la direction (ZQSD ou Flèches) ...
            // ... (Exemple pour Z) ...

            // Si TypeControle == "ZQSD":
            switch (e.Key)
            {
                case Key.Z: // Haut
                    potentialY -= VITESSE;
                    // ... (Mise à jour du sprite) ...
                    aBouge = true;
                    break;
                case Key.S: // Bas
                    potentialY += VITESSE;
                    // ... (Mise à jour du sprite) ...
                    aBouge = true;
                    break;
                    // ... (Q et D) ...
            }
            // ... (Logique pour les Flèches) ...


            // 2. VÉRIFICATION DE LA COLLISION
            if (aBouge)
            {


                // Définir les quatre points potentiels du joueur
                System.Windows.Point coinHautGauche = new System.Windows.Point(potentialX, potentialY);
                System.Windows.Point coinHautDroit = new System.Windows.Point(potentialX + joueurWidth, potentialY);
                System.Windows.Point coinBasGauche = new System.Windows.Point(potentialX, potentialY + joueurHeight);
                System.Windows.Point coinBasDroit = new System.Windows.Point(potentialX + joueurWidth, potentialY + joueurHeight);

                bool collisionTrouvee = false;

                foreach (Rectangle collision in _zonesCollisions)
                {
                    // Conversion du Rectangle XAML en un Rect WPF standard pour la vérification
                    // Attention : On doit utiliser la marge si les propriétés Canvas.Left/Top ne sont pas utilisées.
                    // La façon la plus fiable est de lire ses coordonnées réelles.

                    // On vérifie si l'obstacle est visible et a une taille
                    if (collision.Width > 0 && collision.Height > 0)
                    {
                        // Obtenir la position absolue (X, Y) du coin supérieur gauche de l'obstacle
                        double obstacleX = Canvas.GetLeft(collision);
                        double obstacleY = Canvas.GetTop(collision);

                        // Correction: Si vous utilisez la propriété Margin pour positionner, Canvas.GetLeft/Top sera NaN ou 0.
                        // SI TU UTILISES MARGIN, IL FAUT UTILISER LES COORDONNÉES DÉFINIES DANS InitialiserObstacles

                        // Pour utiliser la méthode Contains, il faut le Rect de l'obstacle :
                        Rect rectObstacle = new Rect(obstacleX, obstacleY, collision.Width, collision.Height);

                        // Vérification du chevauchement de n'importe quel point
                        if (rectObstacle.Contains(coinHautGauche) ||
                            rectObstacle.Contains(coinHautDroit) ||
                            rectObstacle.Contains(coinBasGauche) ||
                            rectObstacle.Contains(coinBasDroit))
                        {
                            collisionTrouvee = true;
                            break;
                        }
                    }
                }

                // 3. APPLICATION DU MOUVEMENT
                if (!collisionTrouvee)
                {
                    Canvas.SetLeft(ImgJoueur, potentialX);
                    Canvas.SetTop(ImgJoueur, potentialY);
                }

                e.Handled = true;
            }

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
            switch (e.Key)
            {
                case Key.E: // Exemple : Appuyer sur E pour interagir
                            // Vérifie si le joueur est proche du client
                    double joueurX = Canvas.GetLeft(ImgJoueur);
                    double joueurY = Canvas.GetTop(ImgJoueur);
                    double clientX = Canvas.GetLeft(ImgClient);
                    double clientY = Canvas.GetTop(ImgClient);

                    // Si le client est présent et le joueur est à proximité
                    if (_clientPresent &&
                        Math.Abs(joueurX - clientX) < 80 && // Distance arbitraire
                        Math.Abs(joueurY - clientY) < 80)
                    {
                        ServirClient(); // Appeler la méthode de service
                    }
                    break;
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