using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_game_AI
{
    class City
    {
        int noOfSoldiers = 0;
        string name;
        List<int> neighbours;
        Color color;
        Point circle_center;

       public City( string name , List<int> neighbours , Color color , Point circle)
        {
            this.neighbours = neighbours;
            this.name = name;
            this.color = color;
            circle_center = circle;
        }

        public string GetName ()
        {
            return name;
        }
        public Color GetColor()
        {
            return color;
        }
        public Point Get_circle_center ()
        {
            return circle_center;
        }
        public int No_of_Soliders ()
        {
            return noOfSoldiers;
        }

        public void Set_No_of_Soliders(int n)
        {
            noOfSoldiers = n;
        }

        public List<int> GetNeighbours ()
        {
            return neighbours;
        }

    }
}
