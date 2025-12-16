using System;
using System.Diagnostics;
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
        // --- CONSTANTES ---
        private const int JardinRows = 3;
        private const int JardinCols = 3;
        private const int PanierSize = 4;

        // --- VARIABLES STATIQUES ---
        private static Fleur?[,] jardin = new Fleur?[JardinRows, JardinCols];
        private static Fleur?[] panier = new Fleur?[PanierSize];
        private static Client? clientActuel = null;

        // --- MOTEUR ---
        private readonly DispatcherTimer gameTimer = new DispatcherTimer();
        private readonly Button?[,] boutonsJardin = new Button?[JardinRows, JardinCols];

        // --- PHYSIQUE ---
        private double persoX = 375, persoY = 200;
        private bool haut, bas, gauche, droite;
        private bool utiliseZQSD = true;

        // Inertie
        private double velX = 0.0, velY = 0.0;
        private double maxSpeed = 240.0;
        private double acceleration = 2000.0;
        private double damping = 8.0;

        // Rendu
        private Stopwatch stopwatch;
        private TimeSpan lastFrameTime;
        private bool isRunning = true;
        private TranslateTransform? imgTransform;

        // --- GAMEPLAY ---
        private readonly Rect zoneBoutique = new Rect(550, 50, 150, 150);
        private string outilEnMain = "Main";
        private string direction = "Recule";
        private int numImage = 1;
        private int compteurTempsAnim = 0;
        private int tempsApparitionClient = 0;

        public UC_Jeu()
        {
            InitializeComponent();

            // 1. Timer Logique (0.1s)
            gameTimer.Interval = TimeSpan.FromMilliseconds(100);
            gameTimer.Tick += GameLogicTick;

            // 2. Timer Rendu (60 FPS)
            stopwatch = Stopwatch.StartNew();
            lastFrameTime = stopwatch.Elapsed;
            CompositionTarget.Rendering += OnRendering;

            // Nettoyage
            Unloaded += (s, e) => {
                CompositionTarget.Rendering -= OnRendering;
                gameTimer.Stop();
            };
        }

        private void UC_Jeu_Loaded(object sender, RoutedEventArgs e)
        {
            // Focus pour clavier
            this.Focus();
            Keyboard.Focus(this);

            if (Application.Current.MainWindow is MainWindow mw)
            {
                utiliseZQSD = (mw.TypeControle == "ZQSD");
                if (txtArgent != null) txtArgent.Text = mw.Argent.ToString();
                if (txtVies != null) txtVies.Text = mw.Vies.ToString();
            }

            imgTransform = new TranslateTransform();
            if (ImgPerso != null)
            {
                ImgPerso.RenderTransform = imgTransform;
                Canvas.SetLeft(ImgPerso, 0);
                Canvas.SetTop(ImgPerso, 0);
            }

            InitialiserGrilleGraphique();
            RestaurerAffichageJardin();
            RestaurerAffichagePanier();
            RestaurerClient();
            MettreAJourSprite(true);

            gameTimer.Start();
        }

        // --- PHYSIQUE & RENDU ---
        private void OnRendering(object? sender, EventArgs e)
        {
            if (!isRunning || (MenuPauseOverlay != null && MenuPauseOverlay.Visibility == Visibility.Visible)) return;

            var now = stopwatch.Elapsed;
            double dt = (now - lastFrameTime).TotalSeconds;
            if (dt > 0.05) dt = 0.05;
            lastFrameTime = now;

            // Inputs
            double inputX = 0, inputY = 0;
            if (haut) inputY -= 1;
            if (bas) inputY += 1;
            if (gauche) inputX -= 1;
            if (droite) inputX += 1;

            // Physique
            double inputLength = Math.Sqrt(inputX * inputX + inputY * inputY);
            double targetVX = 0, targetVY = 0;
            if (inputLength > 0)
            {
                targetVX = (inputX / inputLength) * maxSpeed;
                targetVY = (inputY / inputLength) * maxSpeed;
            }

            velX += (targetVX - velX) * (acceleration * dt / maxSpeed) * 5;
            velY += (targetVY - velY) * (acceleration * dt / maxSpeed) * 5;

            persoX += velX * dt;
            persoY += velY * dt;

            // Collisions
            if (persoX < 0) { persoX = 0; velX = 0; }
            if (persoY < 0) { persoY = 0; velY = 0; }
            if (persoX > 750) { persoX = 750; velX = 0; }
            if (persoY > 390) { persoY = 390; velY = 0; }

            // Mise à jour visuelle
            if (imgTransform != null)
            {
                imgTransform.X = persoX;
                imgTransform.Y = persoY;
            }

            // Interactions
            VerifierEntreeBoutique();

            // Animation
            bool bouge = (Math.Abs(velX) > 10.0 || Math.Abs(velY) > 10.0);
            if (bouge)
            {
                if (Math.Abs(velX) > Math.Abs(velY)) direction = velX > 0 ? "Droite" : "Gauche";
                else direction = velY > 0 ? "Recule" : "Avance";

                compteurTempsAnim++;
                if (compteurTempsAnim > 8)
                {
                    compteurTempsAnim = 0;
                    numImage = (numImage == 1) ? 2 : 1;
                    MettreAJourSprite(false);
                }
            }
            else
            {
                MettreAJourSprite(true);
            }
        }

        // --- LOGIQUE (Timer) ---
        private void GameLogicTick(object? sender, EventArgs e)
        {
            if (MenuPauseOverlay != null && MenuPauseOverlay.Visibility == Visibility.Visible) return;

            // A. Croissance des plantes
            for (int i = 0; i < JardinRows; i++)
            {
                for (int j = 0; j < JardinCols; j++)
                {
                    var f = jardin[i, j];
                    if (f != null)
                    {
                        f.TempsCroissance++;

                        if (f.StadeActuel == Stade.Graine && f.TempsCroissance > 30)
                        {
                            f.StadeActuel = Stade.Pousse;
                        }
                        else if (f.StadeActuel == Stade.Pousse && f.TempsCroissance > 80)
                        {
                            f.StadeActuel = Stade.Adulte;
                        }
                        // CORRECTION 2 : La fleur fane si on attend trop (ici 400 ticks = 40 secondes)
                        else if (f.StadeActuel == Stade.Adulte && f.TempsCroissance > 400)
                        {
                            f.StadeActuel = Stade.Fanee;
                        }

                        MettreAJourImagePlante(i, j, f);
                    }
                }
            }

            // B. Gestion du Client
            if (clientActuel == null)
            {
                tempsApparitionClient++;
                if (tempsApparitionClient > 50)
                {
                    GenererClient();
                    tempsApparitionClient = 0;
                }
            }
            else
            {
                clientActuel.Patience -= 10;

                if (clientActuel.Patience <= 0)
                {
                    clientActuel = null;
                    RestaurerClient();

                    if (Application.Current.MainWindow is MainWindow mw)
                    {
                        mw.Vies--;
                        if (txtVies != null) txtVies.Text = mw.Vies.ToString();

                        if (mw.Vies <= 0)
                        {
                            gameTimer.Stop();
                            isRunning = false;
                            mw.AfficheGameOver();
                        }
                    }
                }
                VerifierVente();
            }
        }

        // --- INTERACTIONS ---
        private void CaseJardin_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not Point p) return;
            int x = (int)p.X;
            int y = (int)p.Y;

            var f = jardin[x, y];
            if (Application.Current.MainWindow is not MainWindow mw) return;

            // 1. PLANTER
            if (f == null)
            {
                if (outilEnMain == "Graine_Marguerite" && mw.GrainesMarguerite > 0)
                {
                    jardin[x, y] = new Fleur(TypeFleur.Marguerite);
                    mw.GrainesMarguerite--;
                }
                else if (outilEnMain == "Graine_Rose" && mw.GrainesRose > 0)
                {
                    jardin[x, y] = new Fleur(TypeFleur.Rose);
                    mw.GrainesRose--;
                }
                else if (outilEnMain == "Graine_Tournesol" && mw.GrainesTournesol > 0)
                {
                    jardin[x, y] = new Fleur(TypeFleur.Tournesol);
                    mw.GrainesTournesol--;
                }
                MettreAJourImagePlante(x, y, jardin[x, y]);
            }
            // 2. RECOLTER
            else if (f.StadeActuel == Stade.Adulte)
            {
                AjouterAuPanier(f);
                jardin[x, y] = null;
                MettreAJourImagePlante(x, y, null);
            }
            // 3. NETTOYER (Si Morte)
            else if (f.StadeActuel == Stade.Fanee || f.StadeActuel == Stade.Pourrie)
            {
                jardin[x, y] = null;
                MettreAJourImagePlante(x, y, null);
            }
        }

        // --- HELPERS ---
        private void InitialiserGrilleGraphique()
        {
            if (GrilleJardin == null) return;
            GrilleJardin.Children.Clear();
            for (int i = 0; i < JardinRows; i++)
            {
                for (int j = 0; j < JardinCols; j++)
                {
                    var btn = new Button
                    {
                        Background = Brushes.Transparent,
                        BorderBrush = Brushes.Transparent,
                        Tag = new Point(i, j)
                    };
                    btn.Focusable = false; // Empêche le focus des boutons
                    btn.Click += CaseJardin_Click;
                    btn.Content = new Image();
                    boutonsJardin[i, j] = btn;
                    GrilleJardin.Children.Add(btn);
                }
            }
        }

        private void RestaurerAffichageJardin()
        {
            for (int i = 0; i < JardinRows; i++)
                for (int j = 0; j < JardinCols; j++)
                    MettreAJourImagePlante(i, j, jardin[i, j]);
        }

        private void MettreAJourImagePlante(int x, int y, Fleur? f)
        {
            if (boutonsJardin[x, y]?.Content is not Image img) return;
            if (f == null) { img.Source = null; return; }

            string nom = "";
            if (f.StadeActuel == Stade.Graine) nom = "seedpousse.png";
            else if (f.StadeActuel == Stade.Fanee || f.StadeActuel == Stade.Pourrie)
            {
                nom = f.Type == TypeFleur.Marguerite ? "daisyfanee.png" : f.Type == TypeFleur.Rose ? "rosefanee.png" : "sunflowerfanee.png";
            }
            else
            {
                string type = f.Type == TypeFleur.Rose ? "rose" : f.Type == TypeFleur.Marguerite ? "daisy" : "sunflower";
                string stade = f.StadeActuel == Stade.Pousse ? "pousse" : "adulte";
                nom = type + stade + ".png";
            }

            try { img.Source = new BitmapImage(new Uri($"/Assets/ImagesJeu/ImagesFleurs/{nom}", UriKind.Relative)); } catch { }
        }

        private void AjouterAuPanier(Fleur f)
        {
            for (int i = 0; i < panier.Length; i++)
            {
                if (panier[i] == null)
                {
                    panier[i] = f;
                    MettreAJourSlotPanier(i);
                    break;
                }
            }
            VerifierVente();
        }

        private void MettreAJourSlotPanier(int index)
        {
            var imgSlot = this.FindName($"slotPanier{index}") as Image;
            if (imgSlot == null) return;
            Fleur? f = panier[index];

            if (f == null) imgSlot.Source = null;
            else
            {
                string nom = f.Type == TypeFleur.Rose ? "roseadulte.png" : f.Type == TypeFleur.Marguerite ? "daisyadulte.png" : "sunfloweradulte.png";
                try { imgSlot.Source = new BitmapImage(new Uri($"/Assets/ImagesJeu/ImagesFleurs/{nom}", UriKind.Relative)); } catch { }
            }
        }

        private void RestaurerAffichagePanier()
        {
            for (int i = 0; i < PanierSize; i++) MettreAJourSlotPanier(i);
        }

        private void GenererClient()
        {
            clientActuel = new Client();
            RestaurerClient();
        }

        private void RestaurerClient()
        {
            if (clientActuel == null)
            {
                if (imgClient != null) imgClient.Visibility = Visibility.Collapsed;
                if (bulleCommande != null) bulleCommande.Visibility = Visibility.Collapsed;
                return;
            }

            if (imgClient != null) imgClient.Visibility = Visibility.Visible;
            if (bulleCommande != null) bulleCommande.Visibility = Visibility.Visible;

            var demande = clientActuel.Commande[0];
            string nom = demande == TypeFleur.Rose ? "roseadulte.png" : demande == TypeFleur.Marguerite ? "daisyadulte.png" : "sunfloweradulte.png";
            if (imgCommandeFleur != null)
            {
                try { imgCommandeFleur.Source = new BitmapImage(new Uri($"/Assets/ImagesJeu/ImagesFleurs/{nom}", UriKind.Relative)); } catch { }
            }
        }

        private void VerifierVente()
        {
            if (clientActuel == null) return;
            var demandee = clientActuel.Commande[0];

            for (int i = 0; i < panier.Length; i++)
            {
                if (panier[i] != null && panier[i].Type == demandee)
                {
                    panier[i] = null;
                    MettreAJourSlotPanier(i);

                    if (Application.Current.MainWindow is MainWindow mw)
                    {
                        mw.Argent += 15;
                        if (txtArgent != null) txtArgent.Text = mw.Argent.ToString();
                    }

                    clientActuel = null;
                    RestaurerClient();
                    break;
                }
            }
        }

        private void VerifierEntreeBoutique()
        {
            var rectJoueur = new Rect(persoX, persoY, 50, 60);
            if (rectJoueur.IntersectsWith(zoneBoutique))
            {
                isRunning = false;
                if (Application.Current.MainWindow is MainWindow mw) mw.AfficheBoutique();
            }
        }

        private void MettreAJourSprite(bool estArrete)
        {
            var suffixe = estArrete ? "Neutre" : numImage.ToString();
            var chemin = $"/Assets/ImagesJeu/perso{direction}{suffixe}.png";
            try { if (ImgPerso != null) ImgPerso.Source = new BitmapImage(new Uri(chemin, UriKind.Relative)); } catch { }
        }

        // --- INPUTS ---

        // La logique d'appui sur les touches
        private void TraiterTouche(Key key, bool estEnfoncee)
        {
            // Outils
            if (estEnfoncee)
            {
                if (key == Key.D1) { outilEnMain = "Main"; if (txtOutil != null) txtOutil.Text = "Outil : ✋ Main"; }
                if (key == Key.D2) { outilEnMain = "Graine_Marguerite"; if (txtOutil != null) txtOutil.Text = "Outil : 🌱 Marguerite"; }
                if (key == Key.D3) { outilEnMain = "Graine_Rose"; if (txtOutil != null) txtOutil.Text = "Outil : 🌹 Rose"; }
                if (key == Key.D4) { outilEnMain = "Graine_Tournesol"; if (txtOutil != null) txtOutil.Text = "Outil : 🌻 Tournesol"; }
            }

            // Mouvement ZQSD
            if (utiliseZQSD)
            {
                if (key == Key.Z) haut = estEnfoncee;
                if (key == Key.S) bas = estEnfoncee;
                if (key == Key.Q) gauche = estEnfoncee;
                if (key == Key.D) droite = estEnfoncee;
            }
            else // Mouvement FLÈCHES
            {
                if (key == Key.Up) haut = estEnfoncee;
                if (key == Key.Down) bas = estEnfoncee;
                if (key == Key.Left) gauche = estEnfoncee;
                if (key == Key.Right) droite = estEnfoncee;
            }
        }

        // On utilise PreviewKeyDown pour être sûr d'attraper les flèches avant qu'elles ne changent le focus
        private void UC_Jeu_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { TogglePause(); e.Handled = true; return; }

            // CORRECTION 1 : Si c'est une flèche, on dit à WPF "Stop, je gère" (Handled = true)
            // Cela empêche le focus de changer et corrige le bug du déplacement infini
            if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right)
            {
                TraiterTouche(e.Key, true);
                e.Handled = true;
            }
            else
            {
                // Pour ZQSD ou autre, on laisse faire
                TraiterTouche(e.Key, true);
            }
        }

        private void UC_Jeu_KeyUp(object sender, KeyEventArgs e)
        {
            TraiterTouche(e.Key, false);
        }

        // --- PAUSE ---
        private void TogglePause()
        {
            if (MenuPauseOverlay == null) return;
            if (MenuPauseOverlay.Visibility == Visibility.Visible)
            {
                MenuPauseOverlay.Visibility = Visibility.Collapsed;
                isRunning = true;
                gameTimer.Start();
                lastFrameTime = stopwatch.Elapsed;
                Focus();
            }
            else
            {
                MenuPauseOverlay.Visibility = Visibility.Visible;
                isRunning = false;
                gameTimer.Stop();
            }
        }

        private void ButPause_Click(object? sender, RoutedEventArgs e) => TogglePause();
        private void ButReprendre_Click(object? sender, RoutedEventArgs e) => TogglePause();
        private void ButQuitterMenu_Click(object? sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw) mw.AfficheMenu();
        }

        // --- RESET ---
        public static void ResetDonneesStatic()
        {
            jardin = new Fleur?[JardinRows, JardinCols];
            panier = new Fleur?[PanierSize];
            clientActuel = null;
        }
    }
}