using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_game_AI.Astar
{
    class Astar
    {
        private static bool[] _boolArray;
        private static Dictionary<int, Country> _map;
        private static List<BstBsrStore> _bstBsrStores;
        private static List<Heuristics> _heuristicTable;

        public static int Draft(Dictionary<int, City> dict, bool[] boolArray, int newTroopNum)
        {
            Dictionary<int, Country> map = new Dictionary<int, Country>();
            Country temp;
            for (int i=0;i<dict.Count; i++)
            {
                temp = new Country(dict[i].GetNeighbours().ToArray(), dict[i].No_of_Soliders());
                map.Add(i, temp);
            }

            // init map & bool array
            _boolArray = new bool[boolArray.Length];
            _map = new Dictionary<int, Country>();

            // clone current map
            Array.Copy(boolArray, _boolArray, boolArray.Length);
            _map = map.ToDictionary(item => item.Key, item => item.Value);

            _bstBsrStores = Util.ExtractBsTsAndBsRsOfMap(_map, _boolArray);

            // heuristic of my candidate countries for attacking
            _heuristicTable = Util.ExtractHeuristicTable(_map, _boolArray, _bstBsrStores, false);


            if (_heuristicTable.Count != 0)
            {
                var myCountry = _map[_heuristicTable[0].CountryIndex];

                var myCountryNewTroopNum = myCountry.TroopsNum + newTroopNum;

                // Wadie, you should take this
            }

            return _heuristicTable[0].CountryIndex;
        }

        public static List<Attack> Attack(Dictionary<int, City> dict, bool[] boolArray)
        {
            Dictionary<int, Country> map = new Dictionary<int, Country>();
            Country temp;

            for (int i = 0; i < dict.Count; i++)
            {
                temp = new Country(dict[i].GetNeighbours().ToArray(), dict[i].No_of_Soliders());
                map.Add(i, temp);
            }

            // init map & bool array
            _boolArray = new bool[boolArray.Length];
            _map = new Dictionary<int, Country>();

            // clone current map
            Array.Copy(boolArray, _boolArray, boolArray.Length);
            _map = map.ToDictionary(item => item.Key, item => item.Value);

            _bstBsrStores = Util.ExtractBsTsAndBsRsOfMap(_map, _boolArray);

            // heuristic of my candidate countries for attacking
            _heuristicTable = Util.ExtractHeuristicTable(_map, _boolArray, _bstBsrStores, true);


            if (_heuristicTable.Count != 0)
            {
                var sandBox = new AstarSandBox(_boolArray, _map, _heuristicTable);
                var attacks = sandBox.Start();

                attacks.ForEach(
                    attack => Console.WriteLine(
                        "my: " + attack.MyCountryIndex +
                        ", His: " + attack.EnemyCountryIndex +
                        ", Attacking Army: " + attack.ArmyNum
                    )
                );

                return attacks;
            }
            else
            {
                var attacks = new List<Attack>();

                return attacks;
            }


        }
    }

    public class Country
    {
        public int[] Neighbors;
        public int TroopsNum;

        public Country(int[] neighbors, int troopsNum)
        {
            Neighbors = neighbors;
            TroopsNum = troopsNum;
        }
    }

    public class Heuristics
    {
        public int CountryIndex;
        public double H;
    }

    public class BstBsrStore
    {
        public int CountryIndex;
        public double BstValue;
        public double BsrValue;
    }
}
