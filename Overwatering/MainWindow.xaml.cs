using System.Windows;
using System.Windows.Controls;

namespace Overwatering
{
    public partial class MainWindow : Window
    {
        // --- DONNÉES DU JOUEUR (Le "Coffre-fort") ---
        // Ces variables sont ici pour être accessibles depuis n'importe quel écran.
        public string TypeControle { get; set; } = "ZQSD";
        public double VolumeJeu { get; set; } = 50;

        public int GrainesMarguerite { get; set; } = 5;
        public int GrainesRose { get; set; } = 2;
        public int GrainesTournesol { get; set; } = 2;

        public int Argent { get; set; } = 0;
        public int Vies { get; set; } = 3;

        public MainWindow()
        {
            InitializeComponent();
            // Au lancement, on affiche le menu principal
            AfficheMenu();
        }

        // --- NAVIGATION (Le "GPS") ---
        // Cette méthode permet de changer le contenu de la fenêtre (Menu -> Jeu -> Boutique...)
        public void ChangerEcran(UserControl nouvelEcran)
        {
            ContenuPrincipal.Content = null; // On vide l'écran actuel pour libérer de la mémoire
            ContenuPrincipal.Content = nouvelEcran; // On met le nouveau
        }

        // Raccourcis pour aller vers les différents écrans
        public void AfficheMenu() => ChangerEcran(new UC_Menu());

        // Note : À chaque appel, on crée un "nouveau" jeu, mais les données static
        // à l'intérieur de UC_Jeu conserveront l'état des plantes.
        public void LancerJeu() => ChangerEcran(new UC_Jeu());

        public void AfficheRegles() => ChangerEcran(new UC_Regles());
        public void AfficheParametres() => ChangerEcran(new UC_Parametres());
        public void AfficheCredits() => ChangerEcran(new UC_Credits());
        public void AfficheGameOver() => ChangerEcran(new UC_GameOver());
        public void AfficheBoutique() => ChangerEcran(new UC_Boutique());

        // Cette méthode sert UNIQUEMENT quand on recommence après un Game Over
        public void LancerNouvellePartie()
        {
            // 1. Remise à zéro des stats du joueur
            Vies = 3;
            Argent = 0; // Ou une somme de départ si tu veux (ex: 50)
            GrainesMarguerite = 5;
            GrainesRose = 2;
            GrainesTournesol = 2;

            // 2. Remise à zéro du jardin (Appel de la méthode qu'on vient de créer)
            UC_Jeu.ResetDonneesStatic();

            // 3. On lance l'écran de jeu
            ChangerEcran(new UC_Jeu());
        }
    }
}