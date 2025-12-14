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


        public void AfficheMenu()
        {

            // création écran menu
            UC_Menu uc = new UC_Menu();
            ContenuPrincipal.Content = uc;

            //boutons menu
            uc.butJouer.Click += AfficherJeu;
            uc.butRegles.Click += AfficheRegles;
            uc.butCredit.Click += AfficheCredits;
            uc.butParametre.Click += AfficheParametres;

        }

        private void ButParametre_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AfficherJeu(object sender, RoutedEventArgs e) { 

            UC_Jeu uc = new UC_Jeu();
            ContenuPrincipal.Content = uc;
        }


        public void AfficheRegles(object sender, RoutedEventArgs e)
        {
            UC_Regles uc = new UC_Regles();
            ContenuPrincipal.Content = uc;
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


        public void AfficheCredits(object sender, RoutedEventArgs e)
        {
            UC_Credits uc = new UC_Credits();
            ContenuPrincipal.Content = uc;
        }

    }
}