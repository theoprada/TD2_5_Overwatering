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
    /// Logique d'interaction pour UC_Regles.xaml
    /// </summary>
    public partial class UC_Regles : UserControl
    {
            public UC_Regles()
            {
                InitializeComponent();

                // Attacher l'événement au bouton de retour
                butRetour.Click += ButRetour_Click;
            }

            private void ButRetour_Click(object sender, RoutedEventArgs e)
            {
            
                Window parentWindow = Window.GetWindow(this);

            
                if (parentWindow is MainWindow mainWindow)
                {
                
                    mainWindow.AfficheMenu();
                }
            }
    }
}
