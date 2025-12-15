using System.Windows;
using System.Windows.Controls;

namespace Overwatering
{
    public partial class UC_Parametres : UserControl
    {
        public string ControleChoisi { get; set; }
        public event EventHandler<Tuple< string>> ParametresSauvegardes;
        public UC_Parametres()
        {
            InitializeComponent();

        }
        public void UCParametres_Loaded(string controleActuel)
        {
            if (controleActuel != null)
            {
                foreach (ComboBoxItem item in ComboControles.Items)
                {
                    if (item.Tag.ToString() == controleActuel)
                    {
                        ComboControles.SelectedItem = item;
                        break;
                    }
                }
            }
    
        }

private void ButValider_Click(object sender, RoutedEventArgs e)
        {
            // ... Récupération du volume ...

            // Récupération du choix du ComboBox
            var selectedItem = (ComboBoxItem)ComboControles.SelectedItem;
            ControleChoisi = selectedItem.Tag.ToString(); // Stocke "ZQSD" ou "ARROWS"

            if (ParametresSauvegardes != null)
            {
                // Envoie du volume et du type de contrôle
                ParametresSauvegardes.Invoke(this, new Tuple< string>(ControleChoisi));
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