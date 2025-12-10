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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Overwatering
{
    /// <summary>
    /// Logique d'interaction pour UCRegles.xaml
    /// </summary>
    public partial class UCRegles : UserControl
    {
            public UCRegles()
            {
                InitializeComponent();

                // Attacher l'événement au bouton de retour
                butRetour.Click += ButRetour_Click;
            }

            private void ButRetour_Click(object sender, RoutedEventArgs e)
            {
            // Lorsque l'utilisateur clique sur 'Retour au Menu',
            // on suppose que le parent est la MainWindow ou un ContentControl qui peut gérer le changement d'écran.
                Window parentWindow = Window.GetWindow(this);

            // Si le Parent est un ContentControl (comme ZoneJeu dans votre MainWindow), on peut faire :
                if (this.Parent is ContentControl parentContentControl)
                    {
                    // Exemple : Changer le contenu pour afficher le Menu principal
                    // Vous devrez remplacer UCMenuJeu par le nom exact de votre UserControl de menu
                        parentContentControl.Content = new UCJeu();
                    }
            }
    }
}
