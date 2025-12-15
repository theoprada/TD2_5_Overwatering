using System.Windows;

using System.Windows.Controls;
using System.Windows.Media;
namespace Overwatering
{
    public partial class MainWindow : Window
    {
        public string TypeControle { get; set; } = "ZQSD";
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private double volumeMusique = 0.5;
        public MainWindow()
        {
            InitializeComponent();
            AfficheMenu();
        }

        public void ChangerEcran(UserControl nouvelEcran)
        {
            ContenuPrincipal.Content = nouvelEcran;
        }


        public void AfficheMenu()
        {
            ChangerEcran(new UC_Menu());
        }

        public void LancerJeu()
        {
            ChangerEcran(new UC_Jeu());
        }

        public void AfficheRegles()
        {
            ChangerEcran(new UC_Regles());

        }

        public void AfficheParametres()
        {
            ChangerEcran(new UC_Parametres());

        }

        public void AfficheCredits()
        {
            ChangerEcran(new UC_Credits());

        }

        public void AfficheGameOver()
        {
            ChangerEcran(new UC_GameOver());
        }
    }
}