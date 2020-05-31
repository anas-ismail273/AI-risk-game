using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_game_AI.Astar
{
    public class AstarSandBox
    {
        private bool[] _initBoolArray;
        private Dictionary<int, Country> _initMap;
        private List<Heuristics> _heuristicTable;

        private List<Node> _nodes = new List<Node>();

        private List<Node> _frontier = new List<Node>();
        private List<Node> _visited = new List<Node>();
        private List<Attack> _attacks = new List<Attack>();

        private int _depth = 0;

        public AstarSandBox(bool[] initBoolArray, Dictionary<int, Country> initMap, List<Heuristics> heuristicTable)
        {
            _initBoolArray = initBoolArray;
            _initMap = initMap;
            _heuristicTable = heuristicTable;
        }

        public List<Attack> Start()
        {
            BuildNodes();
            _frontier.Clear();

            var initialState = _nodes.ElementAt(0);
            _nodes.RemoveAt(0);

            _frontier.Add(initialState);

            while (_frontier.Count != 0)
            {
                if (_depth == 8) break;

                var currentNode = _frontier.ElementAt(0);
                _frontier.RemoveAt(0);

                Attack(currentNode);

                if (Node.IsSolved(_initBoolArray))
                {
                    break;
                }

                _depth++;
                AddNodesToFrontier();
            }

            return _attacks;
        }

        private void AddNodesToFrontier()
        {
            foreach (var node in _nodes)
            {
                var newNode = new Node(
                    countryIndex: node.CountryIndex,
                    h: node.H,
                    g: node.G
                );

                if (!IsInVisited(newNode) && !IsInFrontier(newNode))
                {
                    _frontier.Add(newNode);
                }
                else if (IsInFrontier(newNode))
                {
                    DecreaseNode(newNode);
                }
            }

            _nodes.Clear();
            _frontier = _frontier.OrderBy(node => node.F()).ToList();
        }

        private bool IsInVisited(Node givenNode)
        {
            foreach (var node in _visited)
                if (node.CountryIndex == givenNode.CountryIndex)
                    return true;

            return false;
        }

        private bool IsInFrontier(Node givenNode)
        {
            foreach (var node in _frontier)
                if (node.CountryIndex == givenNode.CountryIndex)
                    return true;

            return false;
        }

        private void DecreaseNode(Node givenNode)
        {
            foreach (var frNode in _frontier)
            {
                if (frNode.CountryIndex == givenNode.CountryIndex)
                {
                    if (givenNode.F() < frNode.F())
                    {
                        _frontier.Remove(frNode);
                        _frontier.Add(givenNode);
                    }
                }
            }
        }

        private void BuildNodes()
        {
            foreach (var heuristic in _heuristicTable)
            {
                var node = new Node(
                    countryIndex: heuristic.CountryIndex,
                    h: heuristic.H,
                    g: 0.0
                );

                _nodes.Add(node);
            }

            _nodes = _nodes.OrderBy(node => node.H).ToList();
        }

        private void Attack(Node attackingNode)
        {
            var bestAttack = ExtractBestAttackOfNode(attackingNode);

            if (bestAttack == null || _initMap[bestAttack.MyCountryIndex].TroopsNum == 1) return;

            var attack = new Attack
            {
                MyCountryIndex = bestAttack.MyCountryIndex,
                EnemyCountryIndex = bestAttack.EnemyCountryIndex
            };

            var myCountry = _initMap[bestAttack.MyCountryIndex];
            var enemyCountry = _initMap[bestAttack.EnemyCountryIndex];

            if (myCountry.TroopsNum > 3)
            {
                attack.ArmyNum = 3;

                if (enemyCountry.TroopsNum <= 2)
                {
                    // 1) subtract attacking army from your countryTroops
                    _initMap[bestAttack.MyCountryIndex].TroopsNum -= attack.ArmyNum;

                    // 2) set TroopsNum = attacking army in the enemyCountry
                    _initMap[bestAttack.EnemyCountryIndex].TroopsNum = attack.ArmyNum;

                    // 3) update the bool Array and but enemy country to true
                    _initBoolArray[bestAttack.EnemyCountryIndex] = true;

                    // 4) add node to visited
                    _visited.Add(attackingNode);

                    // 5) add this attack to attacks list
                    _attacks.Add(attack);

                    // 6) calc H for the conquered country
                    var bstsAndBsrsStores = Util.ExtractBsTsAndBsRsOfMap(_initMap, _initBoolArray);
                    var tempHeuristicTable = Util.ExtractHeuristicTable(
                        _initMap,
                        _initBoolArray,
                        bstsAndBsrsStores,
                        false
                    );

                    var hofConqueredCountry = tempHeuristicTable.Find(
                        heuristics => heuristics.CountryIndex == bestAttack.EnemyCountryIndex
                    );

                    // 7) create new node with conquered countries, g = g(attackingNode) + 1
                    var newNode = new Node(
                        bestAttack.EnemyCountryIndex,
                        hofConqueredCountry.H,
                        attackingNode.G + 1
                    );

                    // 8) add this node to nodes list
                    _nodes.Add(newNode);
                }
                else
                {
                    // 1) enemy will decrease 2 by two and yours will not decrease
                    _initMap[bestAttack.EnemyCountryIndex].TroopsNum -= 2;

                    // 3) update g of my country + 1 -> attacking node
                    attackingNode.G++;

                    // 4) add it back to frontier
                    _frontier.Add(attackingNode);

                    // 5) sort frontier according to F()
                    _frontier = _frontier.OrderBy(node => node.F()).ToList();

                    // 6) add this attack to attacks
                    _attacks.Add(attack);
                }
            }
            else
            {
                if (myCountry.TroopsNum == 3) attack.ArmyNum = 2;
                else return;

                if (enemyCountry.TroopsNum <= 2)
                {
                    // 1) subtract attacking army from your countryTroops
                    _initMap[bestAttack.MyCountryIndex].TroopsNum -= attack.ArmyNum;

                    // 2) set TroopsNum = attacking army in the enemyCountry
                    _initMap[bestAttack.EnemyCountryIndex].TroopsNum = attack.ArmyNum;

                    // 3) update the bool Array and but enemy country to true
                    _initBoolArray[bestAttack.EnemyCountryIndex] = true;

                    // 4) add node to visited
                    _visited.Add(attackingNode);

                    // 5) add this attack to attacks list
                    _attacks.Add(attack);

                    // 6) calc H for the conquered country
                    var bstsAndBsrsStores = Util.ExtractBsTsAndBsRsOfMap(_initMap, _initBoolArray);
                    var tempHeuristicTable = Util.ExtractHeuristicTable(
                        _initMap,
                        _initBoolArray,
                        bstsAndBsrsStores,
                        false
                    );

                    var hofConqueredCountry = tempHeuristicTable.Find(
                        heuristics => heuristics.CountryIndex == bestAttack.EnemyCountryIndex
                    );

                    // 7) create new node with conquered countries, g = g(attackingNode) + 1
                    var newNode = new Node(
                        bestAttack.EnemyCountryIndex,
                        hofConqueredCountry.H,
                        attackingNode.G + 1
                    );

                    // 8) add this node to nodes list
                    _nodes.Add(newNode);
                }
                else if (enemyCountry.TroopsNum == 3)
                {
                    // 1) enemy will decrease 2 by two and yours will not decrease
                    _initMap[bestAttack.EnemyCountryIndex].TroopsNum -= 2;

                    // 3) update g of my country + 1 -> attacking node
                    attackingNode.G++;

                    // 4) add it back to frontier
                    _frontier.Add(attackingNode);

                    // 5) sort frontier according to F()
                    _frontier = _frontier.OrderBy(node => node.F()).ToList();

                    // 6) add this attack to attacks
                    _attacks.Add(attack);
                }
            }
        }

        private PossibleAttack ExtractBestAttackOfNode(Node node)
        {
            var possibleAttacks = new List<PossibleAttack>();
            var myCountry = _initMap[node.CountryIndex];

            // pic all possible attack for that country
            foreach (var neighborIndex in myCountry.Neighbors)
            {
                if (!_initBoolArray[neighborIndex])
                {
                    var possibleAttack = new PossibleAttack
                    {
                        MyCountryIndex = node.CountryIndex,
                        EnemyCountryIndex = neighborIndex
                    };

                    possibleAttacks.Add(possibleAttack);
                }
            }

            // pick min
            possibleAttacks = possibleAttacks.OrderBy(
                possibleAttack => _initMap[possibleAttack.EnemyCountryIndex].TroopsNum
            ).ToList();

            return possibleAttacks.Count != 0 ? possibleAttacks[0] : null;
        }
    }

    public class Attack
    {
        public int MyCountryIndex;
        public int EnemyCountryIndex;
        public int ArmyNum;
    }

    class PossibleAttack
    {
        public int MyCountryIndex, EnemyCountryIndex;
    }
}
