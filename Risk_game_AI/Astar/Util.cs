using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_game_AI.Astar
{
    public class Util
    {
        public static List<BstBsrStore> ExtractBsTsAndBsRsOfMap(Dictionary<int, Country> map, bool[] boolArray)
        {
            var bsts = new List<BstBsrStore>();

            for (var i = 0; i < boolArray.Length; i++)
            {
                if (boolArray[i] && map[i].TroopsNum > 1)
                {
                    var myCountry = map[i];
                    var myCountryNeighbors = myCountry.Neighbors;

                    var bstValueOfi = 0;

                    foreach (var neighbor in myCountryNeighbors)
                    {
                        if (!boolArray[neighbor])
                            bstValueOfi += map[neighbor].TroopsNum;
                    }

                    var store = new BstBsrStore
                    {
                        CountryIndex = i,
                        BstValue = bstValueOfi,
                        BsrValue = bstValueOfi / (double)map[i].TroopsNum,
                    };

                    bsts.Add(store);
                }
            }

            return bsts;
        }

        public static List<Heuristics> ExtractHeuristicTable(
            Dictionary<int, Country> map,
            bool[] boolArray,
            List<BstBsrStore> bstBsrStores,
            bool truncate)
        {
            if (bstBsrStores.Count == 0) return new List<Heuristics>(0);

            var initialHeuristicTable = new List<Heuristics>();

            foreach (var store in bstBsrStores)
            {
                var myCountry = map[store.CountryIndex];
                var numOfEnemyNeighbors = 0;

                foreach (var neighborIndex in myCountry.Neighbors)
                    if (!boolArray[neighborIndex])
                        numOfEnemyNeighbors++;

                initialHeuristicTable.Add(new Heuristics
                {
                    CountryIndex = store.CountryIndex,
                    H = store.BsrValue + numOfEnemyNeighbors,
                });
            }

            if (truncate)
            {
                var heuristicTableAverage = initialHeuristicTable.Average(item => item.H);

                var truncatedHeuristicTable1 = initialHeuristicTable.Where(
                    heuristics => heuristics.H <= heuristicTableAverage
                ).ToList();

                var truncatedHeuristicTable2 = truncatedHeuristicTable1.Where(
                    heuristics => (int)heuristics.H != 0
                ).ToList();

                var sortedHeuristicTable = truncatedHeuristicTable2.OrderBy(item => item.H).ToList();

                return sortedHeuristicTable;
            }
            else
            {
                var sortedHeuristicTable = initialHeuristicTable.OrderBy(item => item.H).ToList();
                return sortedHeuristicTable;
            }
        }
    }
}
