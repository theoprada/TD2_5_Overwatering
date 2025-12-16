using System;
using System.Windows;
using System.Windows.Controls;
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

        public CommandeClient(TypeFleur fleur, int quantite = 1)
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
        private List<Rect> _zonesCollisions;

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
            _zonesCollisions = new List<Rect>();

            // 1. ZONES NON JOUABLES (BORDURES DE LIGNE BLEUE)
            // Bordure Haut (Arbres)
            _zonesCollisions.Add(new Rect(0, 0, 640, 120));
            // Bordure Gauche (Arbres)
            _zonesCollisions.Add(new Rect(0, 120, 100, 360));
            // Bordure Droite (Arbres)
            _zonesCollisions.Add(new Rect(540, 120, 100, 360));
            // Bordure Bas (Arbres/Buissons)
            _zonesCollisions.Add(new Rect(0, 420, 640, 60));


            // 2. OBSTACLES INTERNES
            // Puits (Rond brun)
            _zonesCollisions.Add(new Rect(480, 380, 60, 45));

            // Champ Labouré (Grande zone de culture - Blocage total)
            _zonesCollisions.Add(new Rect(180, 200, 280, 180));

            // Terrain Jaune (Petite zone de culture - Blocage total)
            _zonesCollisions.Add(new Rect(480, 200, 120, 180));
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
                // Créer le rectangle potentiel du joueur à sa future position
                Rect rectJoueurPotentiel = new Rect(
                    potentialX,
                    potentialY,
                    joueurWidth,
                    joueurHeight);

                bool collisionTrouvee = false;

                foreach (Rect zoneBloquante in _zonesCollisions)
                {
                    // La méthode IntersectsWith vérifie si deux rectangles se chevauchent
                    if (rectJoueurPotentiel.IntersectsWith(zoneBloquante))
                    {
                        collisionTrouvee = true;
                        break; // Pas besoin de vérifier les autres zones
                    }
                }

                // 3. APPLICATION DU MOUVEMENT
                if (!collisionTrouvee)
                {
                    // SI AUCUNE COLLISION : On met à jour la position
                    Canvas.SetLeft(ImgJoueur, potentialX);
                    Canvas.SetTop(ImgJoueur, potentialY);
                }
                else
                {
                    // SI COLLISION : Le mouvement est annulé (les positions restent currentX/Y)
                    // Tu peux ajouter ici un son de "bump" si tu veux.
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