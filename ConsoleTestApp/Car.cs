using System;
using System.Collections.Generic;
using System.Text;
using CoreCMS;
using System.Linq;

namespace ConsoleTestApp
{
    public sealed class Car : Content
    {
        private static Random _random;
        private static int _availableSegments = -1;
        private static int _availableManufatures = -1;

        public Segment Segment { get; set; }
        public Manufacture Manufacture { get; set; }
        public string PlateNumber { get; set; }
        public int Year { get; set; }

        public Car()
        {
            //lets initialize the static var if they were not initialize alreadys
            if (_random == null)
            {
                _random = new Random(78713214);
                _availableSegments = Enum.GetNames(typeof(Segment)).Length;
                _availableManufatures = Enum.GetNames(typeof(Manufacture)).Length;
            }

            //we are going to skip the the first element because they are "undefined" by definition
            Segment = (Segment)_random.Next(1, _availableSegments);
            Manufacture = (Manufacture)_random.Next(1, _availableManufatures);

            //plate demands a more complex calculations :D
            var plateNumeral = _random.Next(1, 10000).ToString("0000");
            var plateLetters = GetRandomPlateChar() + GetRandomPlateChar() + GetRandomPlateChar();
            PlateNumber = plateLetters + "-" + plateNumeral;

            //randomize a year
            Year = _random.Next(1960, DateTime.Now.Year);

            Name = Manufacture + " " + Segment + " '" + (Year % 100).ToString();
        }

        private string GetRandomPlateChar()
        {
            var availableChars = "abcdefghijklmniopqrstuvwxyz";
            return availableChars[_random.Next(0, availableChars.Length)] + "";
        }
    }
}
