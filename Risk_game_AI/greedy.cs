using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_game_AI
{
    class greedy
    {
        private Dictionary<int, City> mymap = new Dictionary<int, City>();
        private bool[] myoccupy;
        private int countryNumber = 0;
        public greedy(int number)
        {

            this.countryNumber = number;
            myoccupy = new bool[number];

        }
        public int[] deploy(bool[] occupy)
        {
            if (countryNumber == 50)
                this.mymap = USA_City_details.Instance.getDictionary();
            else
            {
                //egypt
            }
            this.myoccupy = occupy;
            int size = 0;
            int[] result = new int[countryNumber];
            Array.Clear(result, 0, result.Length);
            for (int i = 0; i < countryNumber; i++)
            {
                if (myoccupy[i])
                    size++;
            }
            int[] array = new int[size];
            size = 0;
            for (int i = 0; i < countryNumber; i++)
            {
                if (myoccupy[i])
                {
                    array[size] = i;
                    size++;
                }
            }
            size = size / 3;
            if (size < 1)
                size = 3;
            for (int i = 0; i < size; i++)
            {
                Random random = new Random();
                int countryy = random.Next(0, array.Length);
                result[array[countryy]] = result[array[countryy]] + 1;
            }
            return result;

        }
        public int[] attack(bool[] occupy)
        {
            if (countryNumber == 50)
                this.mymap = USA_City_details.Instance.getDictionary();
            else
            {
                //egypt
            }
            this.myoccupy = occupy;
            double minHeu = int.MaxValue;
            int minEnemy = int.MaxValue;
            int id = -1;
            int idAttacked = 0;
            int[] result = new int[3];
            result[0] = 0;
            result[1] = 0;
            result[2] = 0;

            List<Heuristics> heu = new List<Heuristics>();
            heu = calculateHeuristic();
            for (int i = 0; i < heu.Count; i++)
            {
                if (heu.ElementAt(i).H < minHeu)
                {
                    minHeu = heu.ElementAt(i).H;
                    id = heu.ElementAt(i).CountryIndex;
                }
            }
            // Console.WriteLine(id + " " + minHeu);
            for (int i = 0; i < mymap[id].GetNeighbours().Count; i++)
            {
                if (!occupy[mymap[id].GetNeighbours().ElementAt(i)])
                {
                    if (mymap[mymap[id].GetNeighbours().ElementAt(i)].No_of_Soliders() < minEnemy)
                    {
                        minEnemy = mymap[mymap[id].GetNeighbours().ElementAt(i)].No_of_Soliders();
                        idAttacked = mymap[id].GetNeighbours().ElementAt(i);
                    }
                }
            }
            if (mymap[id].No_of_Soliders() > 1 && mymap[id].No_of_Soliders() > minEnemy)
            {
                result[0] = id;
                result[1] = idAttacked;
                if (mymap[id].No_of_Soliders() > 3)
                    result[2] = 3;
                else
                    result[2] = mymap[id].No_of_Soliders() - 1;
            }

            return result;
        }
        private List<Heuristics> calculateHeuristic()
        {
            double enemyArmy = 0;
            List<Heuristics> theResult = new List<Heuristics>();
            for (int i = 0; i < countryNumber; i++)
            {
                enemyArmy = 0;
                if (myoccupy[i])
                {

                    for (int j = 0; j < this.mymap[i].GetNeighbours().Count; j++)
                    {
                        if (!myoccupy[mymap[i].GetNeighbours().ElementAt(j)])
                            enemyArmy = enemyArmy + mymap[mymap[i].GetNeighbours().ElementAt(j)].No_of_Soliders();
                    }

                    Heuristics temp = new Heuristics();
                    temp.CountryIndex = i;
                    temp.H = (enemyArmy / (double)mymap[i].No_of_Soliders());
                    theResult.Add(temp);

                }
            }
            return theResult;
        }

    }
}
