using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseAPI.Script
{
    //Script to generate a random Bird.
    public class GenerateBirds
    {
        private static Random rnd = new Random();
        private static string[] species = new string[] { "Amsel", "Spatz", "Meise", "Rotkelchen", "Schwalben", "Fink", "Specht", "Taube", "Kuckuck", "Kraehe", "Blaukelchen", "Kuckuck", "Drossel", "Star", "Rabe", "Storch", "Zaunkoenig", "Falke" };
        private static string[] strassen = new string[] { "Hauptstraße", "Schulstraße", "Gartenstraße", "Bahnhofstraße", "Dorfstraße" };
        private static string[] boxen = new string[] { "Vogelhaus", "Hecke", "Dach", "Garten", "Balkon", "Baum" };
        private static string[] orientation = new string[] { "North", "South", "East", "West" };


        public static Bird randomBird(int Id)
        {
            return new Bird() { Species = species[rnd.Next(0, species.Length)], Adress = strassen[rnd.Next(0, strassen.Length)], BoxKind = boxen[rnd.Next(0, boxen.Length - 1)], NumberChicks = rnd.Next(1, 4), Plz = rnd.Next(7000, 8000), NestDate = DateTime.Now, Temperature = rnd.Next(20, 35), Longitude = Decimal.Parse((rnd.NextDouble() * 7 + 6) + ""), Latitude = Decimal.Parse((rnd.NextDouble() * 6 + 47) + ""), Compass = orientation[rnd.Next(1, orientation.Length)], Id = Id };
        }

    }
}
