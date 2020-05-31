using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_game_AI
{
    class agressiveAgent
    {
        private Dictionary<int, City> mymap = new Dictionary<int, City>();
        private bool[] myoccupy;
        private int countryNumber = 0;
        public agressiveAgent(int number)
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
            int[] result = new int[countryNumber];
            int[] temp = new int[countryNumber];
            int army = 0;
            Array.Clear(result, 0, result.Length);
            Array.Clear(temp, 0, temp.Length);
            for (int i = 0; i < countryNumber; i++)
            {
                if (myoccupy[i])
                {
                    army++;
                    temp[i] = mymap[i].No_of_Soliders();
                }
            }
            army = army / 3;
            if (army < 1) army = 3;
            for (int i = 0; i < army; i++)
            {

                result[Array.IndexOf(temp, temp.Max())] = result[Array.IndexOf(temp, temp.Max())] + 1;
                temp[Array.IndexOf(temp, temp.Max())] = temp[Array.IndexOf(temp, temp.Max())] + 1;
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
            int[] result = new int[3];

            int[,] possibleAttack = new int[500, 2];
            Array.Clear(result, 0, result.Length);
            // Array.Clear(temp, 0, temp.Length);

            int count = 0;
            for (int i = 0; i < countryNumber; i++)
            {

                if (myoccupy[i])
                {

                    possibleAttack[count, 0] = i;
                    //Console.WriteLine("element = " + i);
                    possibleAttack[count, 1] = 0;
                    //Console.WriteLine(mymap[i].GetNeighbours().Count);
                    for (int j = 0; j < mymap[i].GetNeighbours().Count; j++)
                    {
                        possibleAttack[count, 0] = i;
                        possibleAttack[count, 1] = mymap[i].GetNeighbours().ElementAt(j);

                        Console.WriteLine(mymap[i].GetNeighbours().ElementAt(j));
                        count++;
                    }
                    //Console.WriteLine("loop");
                }
            }
            possibleAttack[count, 0] = -1;


            count = 0;
            int maxArmy = 0, idMaxArmy = 0, attackFromId = 0, maxAttackArmy = 0;
            while (possibleAttack[count, 0] != -1)
            {

                if (mymap[possibleAttack[count, 1]].No_of_Soliders() > maxArmy)
                {

                    maxArmy = mymap[possibleAttack[count, 1]].No_of_Soliders();
                    idMaxArmy = possibleAttack[count, 1];
                    attackFromId = possibleAttack[count, 0];

                }
                count++;
            }

            for (int i = 0; i < mymap[idMaxArmy].GetNeighbours().Count; i++)
            {

                if (myoccupy[mymap[idMaxArmy].GetNeighbours().ElementAt(i)])
                {
                    Console.WriteLine("flag");
                    if (mymap[mymap[idMaxArmy].GetNeighbours()[i]].No_of_Soliders() > maxAttackArmy)
                    {
                        maxAttackArmy = mymap[mymap[idMaxArmy].GetNeighbours()[i]].No_of_Soliders();
                        attackFromId = mymap[idMaxArmy].GetNeighbours()[i];

                    }
                }
            }


            if (mymap[attackFromId].No_of_Soliders() > 1)
            {
                result[0] = attackFromId;
                result[1] = idMaxArmy;
                if (mymap[attackFromId].No_of_Soliders() > 3)
                    result[2] = 3;
                else
                    result[2] = mymap[attackFromId].No_of_Soliders() - 1;
                return result;
            }
            else
            {
                return result;
            }
        }
    }

}
