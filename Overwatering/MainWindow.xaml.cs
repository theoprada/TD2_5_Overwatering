using System.Windows;
using System.Windows.Controls;

namespace Overwatering
{
    public partial class MainWindow : Window
    {
        double volumeMusique = 0.5;
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

            //boutons menu
            uc.butJouer.Click += AfficherJeu;
            uc.butRegles.Click += AfficherRegles;
            uc.butCredit.Click += AfficherCredit;
            uc.butParametre.Click += AfficheParametres;

        public void AfficheRegles()
        {
            ChangerEcran(new UC_Regles());
        }

        private void ButParametre_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AfficherJeu(object sender, RoutedEventArgs e)
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

        public void AfficheParametres(object sender, RoutedEventArgs e)
        {
            // 1. On crée la fenêtre en lui envoyant le volume actuel
            ParametresWindow fenetreParam = new ParametresWindow(volumeMusique);

            // 2. On l'affiche et on attend la réponse (true si Confirmer, false si Annuler)
            bool? resultat = fenetreParam.ShowDialog();

            // 3. Si l'utilisateur a cliqué sur "Confirmer"
            if (resultat == true)
            {
                // On récupère la nouvelle valeur
                volumeMusique = fenetreParam.VolumeChoisi;

                // ICI : Applique le volume à ton lecteur de musique si tu en as un
                // exemple : monMediaPlayer.Volume = volumeMusique;

                MessageBox.Show($"Volume changé à : {volumeMusique * 100}%"); // Juste pour tester
            }

        }
    }
}