using System.Windows;
using System.Windows.Controls;

namespace Overwatering
{
    public partial class UC_Parametres : UserControl
    {
        public UC_Parametres()
        {
            InitializeComponent();
            ChargerDonnees();
        }

        private void ChargerDonnees()
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                sliderVolume.Value = mw.VolumeJeu;
                if (mw.TypeControle == "ZQSD")
                    ComboControles.SelectedIndex = 0;
                else
                    ComboControles.SelectedIndex = 1;
            }
        }

        // Sauvegarder et Quitter
        private void SauvegarderEtQuitter()
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                // Sauvegarde du volume
                mw.VolumeJeu = sliderVolume.Value;

                // Sauvegarde des touches
                if (ComboControles.SelectedIndex == 0)
                    mw.TypeControle = "ZQSD";
                else
                    mw.TypeControle = "FLECHES";
                mw.AfficheMenu();
            }
        }

        // Bouton Valider
        private void ButValider_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw2) mw2.JouerSonBouton();
            SauvegarderEtQuitter();
        }

        // Bouton Retour
        private void butRetour_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw2) mw2.JouerSonBouton();
            SauvegarderEtQuitter();
        }

        private void UCParametres_Loaded(object sender, RoutedEventArgs e) { }
    }
}