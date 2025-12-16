using System;
using System.Collections.Generic;

namespace Overwatering
{
    public enum Stade { Graine, Pousse, Adulte, Fanee, Pourrie }
    public enum TypeFleur { Marguerite, Rose, Tournesol }

    public class Fleur
    {
        public TypeFleur Type { get; set; }
        public Stade StadeActuel { get; set; } = Stade.Graine;
        public double NiveauEau { get; set; } = 50;
        public int TempsCroissance { get; set; } = 0;

        public Fleur(TypeFleur type)
        {
            Type = type;
        }
    }

    public class Client
    {
        public List<TypeFleur> Commande { get; set; } = new List<TypeFleur>();
        public int Patience { get; set; } = 1000; // Le temps avant qu'il parte

        // Générateur de nombres aléatoires (static pour être unique pour tous les clients)
        private static Random generateur = new Random();

        public Client()
        {
            // 1. On tire un nombre au hasard entre 0, 1 et 2
            // Array.GetValue permet de récupérer les valeurs de l'enum TypeFleur
            var valeurs = Enum.GetValues(typeof(TypeFleur));
            TypeFleur fleurAleatoire = (TypeFleur)valeurs.GetValue(generateur.Next(valeurs.Length));

            // 2. On ajoute cette fleur à la commande
            Commande.Add(fleurAleatoire);
        }
    }
}