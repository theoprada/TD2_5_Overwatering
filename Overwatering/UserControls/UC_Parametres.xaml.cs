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

        // 1. Lire les infos sauvegardées
        private void ChargerDonnees()
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                // On remet le slider au bon endroit
                sliderVolume.Value = mw.VolumeJeu;

                // On remet la liste déroulante sur le bon choix
                if (mw.TypeControle == "ZQSD")
                    ComboControles.SelectedIndex = 0;
                else
                    ComboControles.SelectedIndex = 1;
            }
        }

        // 2. Sauvegarder et Quitter
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

                // Retour au menu
                mw.AfficheMenu();
            }
        }

        // Bouton Valider
        private void ButValider_Click(object sender, RoutedEventArgs e)
        {
            SauvegarderEtQuitter();
        }

        // Bouton Retour
        private void butRetour_Click(object sender, RoutedEventArgs e)
        {
            SauvegarderEtQuitter();
        }

        // (Laisse la méthode Loaded vide ou supprime-la du XAML si elle t'embête)
        private void UCParametres_Loaded(object sender, RoutedEventArgs e) { }
    }
}