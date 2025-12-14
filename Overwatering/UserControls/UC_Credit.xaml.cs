using System.Windows;
using System.Windows.Controls;

namespace Overwatering
{
    public partial class UC_Credits : UserControl
    {
        public UC_Credits()
        {
            InitializeComponent();
        }

        private void butRetourCredit_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow is MainWindow mainWindow)
            {
                // APPEL CORRIGÉ : Demander à la MainWindow d'afficher le menu principal
                // Cette méthode (que nous avons rendue public) recrée et recâble TOUS les boutons.
                mainWindow.AfficheMenu();
            }
        }
    }
}