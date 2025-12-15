using System.Windows;
using System.Windows.Controls;

namespace Overwatering
{
    public partial class UC_Parametres : UserControl
    {
        public UC_Parametres()
        {
            InitializeComponent();
        }

        private void UCParametres_Loaded(object sender, RoutedEventArgs e)
        {
            // Récupère les paramètres depuis la MainWindow et initialise les contrôles
            if (Application.Current.MainWindow is MainWindow mw)
            {
                sliderVolume.Value = mw.VolumeJeu;

                // Détermine l'index du ComboBox selon le TypeControle
                if (mw.TypeControle == "ZQSD")
                    ComboControles.SelectedIndex = 0;
                else
                    ComboControles.SelectedIndex = 1;
            }
        }

        private void ButValider_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                // Enregistre le volume
                mw.VolumeJeu = sliderVolume.Value;

                // Enregistre le type de contrôle (Tag des ComboBoxItem)
                if (ComboControles.SelectedItem is ComboBoxItem cbi && cbi.Tag != null)
                {
                    mw.TypeControle = cbi.Tag.ToString();
                }

                // Retour au menu
                mw.AfficheMenu();
            }
        }

        private void butRetour_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.AfficheMenu();
            }
        }
    }
}