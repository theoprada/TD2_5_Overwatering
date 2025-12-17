using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Overwatering
{
    public partial class MainWindow : Window
    {
        // variables accessibles depuis n'importe quel écran.
        public string TypeControle { get; set; } = "ZQSD";
        public double VolumeJeu { get; set; } = 50;

        public int GrainesMarguerite { get; set; } = 5;
        public int GrainesRose { get; set; } = 2;
        public int GrainesTournesol { get; set; } = 2;

        public int Argent { get; set; } = 0;
        public int Vies { get; set; } = 3;
        private MediaPlayer soundPlayer = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            AfficheMenu();
        }

        // permet de changer le contenu de la fenêtre
        public void ChangerEcran(UserControl nouvelEcran)
        {
            ContenuPrincipal.Content = null; // On vide l'écran actuel pour libérer de la mémoire
            ContenuPrincipal.Content = nouvelEcran; // On met le nouveau
        }

        // Raccourcis pour aller vers les différents écrans
        public void AfficheMenu() => ChangerEcran(new UC_Menu());

        public void LancerJeu() => ChangerEcran(new UC_Jeu());

        public void AfficheRegles() => ChangerEcran(new UC_Regles());
        public void AfficheParametres() => ChangerEcran(new UC_Parametres());
        public void AfficheCredits() => ChangerEcran(new UC_Credits());
        public void AfficheGameOver() => ChangerEcran(new UC_GameOver());
        public void AfficheBoutique() => ChangerEcran(new UC_Boutique());

        // sert  quand on recommence après un Game Over
        public void LancerNouvellePartie()
        {
            // Remise à zéro des stats
            Vies = 3;
            Argent = 0;
            GrainesMarguerite = 5;
            GrainesRose = 2;
            GrainesTournesol = 2;

            // Remise à zéro du jardin
            UC_Jeu.ResetDonneesStatic();
            ChangerEcran(new UC_Jeu());
        }

        public void JouerSonBouton()
        {
            try
            {
                soundPlayer.Open(new Uri("Assets/EffetsSons/InterfaceSound.mp3", UriKind.Relative));
                soundPlayer.Volume = VolumeJeu / 100.0;

                soundPlayer.Stop();
                soundPlayer.Play();
            }
            catch { }
        }

        public void JouerSonPiece()
        {
            try
            {
                MediaPlayer coinPlayer = new MediaPlayer();
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/EffetsSons/coinSound.mp3");
                coinPlayer.Open(new Uri(path));

                coinPlayer.Volume = VolumeJeu / 100.0;
                coinPlayer.Play();
            }
            catch { }
        }

        public void JouerSonRecolte()
        {
            try
            {
                MediaPlayer recoltePlayer = new MediaPlayer();
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/EffetsSons/RecolteFleur.mp3");
                recoltePlayer.Open(new Uri(path));
                recoltePlayer.Volume = VolumeJeu / 100.0;
                recoltePlayer.Play();
            }
            catch { }
        }
    }
}