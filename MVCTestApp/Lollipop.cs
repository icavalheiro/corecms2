using CoreCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCTestApp
{
    public class Lollipop : Content
    {
        public static string[] AvailableColors = new string[]
        {
            "ORANGE",
            "YELLOW",
            "BLUE",
            "GREEN",
            "RED",
            "BLUE",
            "RED-DEVIL",
            "RED-CHERY"
        };

        public static string[] AvailableFlavours = new string[]
        {
            "CHERY",
            "CARAMEL",
            "STRAWBERY",
            "ORANGE",
            "GRAPE",
            "ROSE",
            "DEVILS-KISS"
        };

        public string Color;
        public string Flavor;

        public Lollipop()
        {
            var random = new Random();
            Color = AvailableColors[random.Next(0, AvailableColors.Length)];
            Flavor = AvailableFlavours[random.Next(0, AvailableFlavours.Length)];
            Name = $"Lollipop {Color} {Flavor}";
        }
    }
}
