using System.Windows;
using System.Windows.Controls;

namespace Overwatering
{
    public partial class MainWindow : Window
    {
        // Variable globale pour retenir le choix du joueur (ZQSD par défaut)
        public string TypeControle { get; set; } = "ZQSD";

        // Variable pour le volume (0 à 100)
        public double VolumeJeu { get; set; } = 50;

        public MainWindow()
        {
            InitializeComponent();
            AfficheMenu();
        }

        // --- NAVIGATION ---
        public void ChangerEcran(UserControl nouvelEcran)
        {
            // On vide le contenu actuel avant de mettre le nouveau (optimisation mémoire)
            ContenuPrincipal.Content = null;
            ContenuPrincipal.Content = nouvelEcran;
        }

        public void AfficheMenu() => ChangerEcran(new UC_Menu());
        public void LancerJeu() => ChangerEcran(new UC_Jeu());
        public void AfficheRegles() => ChangerEcran(new UC_Regles());
        public void AfficheParametres() => ChangerEcran(new UC_Parametres()); // Plus besoin de logique complexe ici
        public void AfficheCredits() => ChangerEcran(new UC_Credits());
        public void AfficheGameOver() => ChangerEcran(new UC_GameOver());
    }
}