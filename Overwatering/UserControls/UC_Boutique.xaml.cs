using System.Windows;
using System.Windows.Controls;

namespace Overwatering
{
    public partial class UC_Boutique : UserControl
    {
        // Prix graines
        const int PRIX_MARGUERITE = 10;
        const int PRIX_ROSE = 20;
        const int PRIX_TOURNESOL = 30;

        public UC_Boutique()
        {
            InitializeComponent();
        }

        private void UC_Boutique_Loaded(object sender, RoutedEventArgs e)
        {
            MettreAJourAffichage();
        }

        // rafraîchir les textes (Argent et Stocks)
        private void MettreAJourAffichage()
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                TxtArgent.Text = mw.Argent.ToString();
                TxtStockDaisy.Text = "Possédé : " + mw.GrainesMarguerite;
                TxtStockRose.Text = "Possédé : " + mw.GrainesRose;
                TxtStockSun.Text = "Possédé : " + mw.GrainesTournesol;
            }
        }

        // Achat Marguerite
        private void BtnAcheterMarguerite_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw2) mw2.JouerSonBouton();
            if (Application.Current.MainWindow is MainWindow mw)
            {
                if (mw.Argent >= PRIX_MARGUERITE)
                {
                    mw.Argent -= PRIX_MARGUERITE;
                    mw.GrainesMarguerite++;
                    MettreAJourAffichage();
                }
                else
                {
                    MessageBox.Show("Pas assez d'argent !");
                }
            }
        }

        // Achat Rose
        private void BtnAcheterRose_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw2) mw2.JouerSonBouton();
            if (Application.Current.MainWindow is MainWindow mw)
            {
                if (mw.Argent >= PRIX_ROSE)
                {
                    mw.Argent -= PRIX_ROSE;
                    mw.GrainesRose++;
                    MettreAJourAffichage();
                }
                else MessageBox.Show("Pas assez d'argent !");
            }
        }

        // Achat Tournesol
        private void BtnAcheterTournesol_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw2) mw2.JouerSonBouton();
            if (Application.Current.MainWindow is MainWindow mw)
            {
                if (mw.Argent >= PRIX_TOURNESOL)
                {
                    mw.Argent -= PRIX_TOURNESOL;
                    mw.GrainesTournesol++;
                    MettreAJourAffichage();
                }
                else MessageBox.Show("Pas assez d'argent !");
            }
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw2) mw2.JouerSonBouton();
            if (Application.Current.MainWindow is MainWindow mw)
            { 
                mw.LancerJeu();
            }
        }
    }
}