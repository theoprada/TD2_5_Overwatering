using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Overwatering
{
    /// <summary>
    /// Logique d'interaction pour ParametresWindow.xaml
    /// </summary>
    public partial class ParametresWindow : Window
    {
        // Propriété pour récupérer la valeur finale
        public double VolumeChoisi { get; private set; }

        // Constructeur : on lui passe le volume actuel pour positionner le slider
        public ParametresWindow(double volumeActuel)
        {
            InitializeComponent();
            sliderVolume.Value = volumeActuel;
        }

        private void butConfirmer_Click(object sender, RoutedEventArgs e)
        {
            // On sauvegarde la valeur du slider
            VolumeChoisi = sliderVolume.Value;

            // DialogResult = true ferme la fenêtre et dit à la MainWindow "C'est validé !"
            this.DialogResult = true;
        }

        private void butAnnuler_Click(object sender, RoutedEventArgs e)
        {
            // On ferme sans valider
            this.DialogResult = false;
        }
    }
}
