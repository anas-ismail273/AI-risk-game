using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_game_AI.Astar
{
    public class Node
    {
        public int CountryIndex;

        public double G = 0.0;
        public double H = 0.0;

        public Node(int countryIndex, double h, double g)
        {
            CountryIndex = countryIndex;
            G = g;
            H = h;
        }

        public static bool IsSolved(bool[] boolArray) => boolArray.All(item => item == true);
        public double F() => G + H;
    }
}
