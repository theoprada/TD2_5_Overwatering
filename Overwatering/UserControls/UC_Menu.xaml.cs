using System.Windows;
using System.Windows.Controls;

namespace Overwatering
{
    public partial class UC_Menu : UserControl
    {
        public UC_Menu()
        {
            InitializeComponent();
        }
        

        private void butQuitterJeu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}