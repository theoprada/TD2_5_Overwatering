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

        private void ButRetour_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.AfficheMenu();
            }
        }
    }
}