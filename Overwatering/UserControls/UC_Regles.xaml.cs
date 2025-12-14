using System.Windows;
using System.Windows.Controls;

namespace Overwatering
{
    public partial class UC_Regles : UserControl
    {
        public UC_Regles()
        {
            InitializeComponent();
        }

        private void butRetour_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);

            // ÉTAPE 2 : Vérifier et appeler la méthode de MainWindow
            if (parentWindow is MainWindow mainWindow)
            {
                // APPEL CORRIGÉ : Demander à la MainWindow d'afficher le menu principal
                // Cette méthode (que nous avons rendue public) recrée et recâble TOUS les boutons.
                mainWindow.AfficheMenu();
            }
        }

      
    }
}