using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_game_AI
{
    class minimax
    {
        private int[] result = new int[3];
        // do clone
        private Dictionary<int, City> clonedDictionary = new Dictionary<int, City>(USA_City_details.Instance.getDictionary());

        public int[] minimaxDecisionMaker(Boolean[] maxPly, Boolean[] minPly)
        {
            Node<int> root = new Node<int>(0, 0);
            return minimaxDecision(root, clonedDictionary, maxPly, minPly);
        }
        private int[] minimaxDecision(Node<int> node, Dictionary<int, City> clonedDictionary, Boolean[] maxPly, Boolean[] minPly)
        {
            int tempID;
            List<Node<int>> maxNodes2list = new List<Node<int>>();

            for (int i = 0; i < 50; i++)
            {
                if (maxPly[i])
                {
                    Node<int> temp = new Node<int>(i, -1);
                    //if(USA_City_details.Instance.GetCity(temp.getData()).No_of_Soliders() > 1)
                    node.addChild(temp);
                }
            }

            foreach (Node<int> a in node.getChildren())
            {
                for (int j = 0; j < USA_City_details.Instance.GetCity(a.getData()).GetNeighbours().Count; j++)
                {
                    tempID = USA_City_details.Instance.GetCity(a.getData()).GetNeighbours().ElementAt(j);
                    Node<int> temp = new Node<int>(tempID, -1);
                    if (USA_City_details.Instance.GetCity(temp.getData()).No_of_Soliders() > 0)
                        a.addChild(temp);
                }

                foreach (Node<int> b in a.getChildren())
                {
                    for (int j = 0; j < USA_City_details.Instance.GetCity(b.getData()).GetNeighbours().Count; j++)
                    {
                        tempID = USA_City_details.Instance.GetCity(b.getData()).GetNeighbours().ElementAt(j);
                        Node<int> temp = new Node<int>(tempID, getHeuristic(tempID, minPly));
                        if (USA_City_details.Instance.GetCity(temp.getData()).No_of_Soliders() > 0)
                        {   b.addChild(temp);
                        maxNodes2list.Add(temp);
                            }
                    }
                }
            }

            // Calculating Heuristic ......


            List<Node<int>> minNodesList = maxNodes2list[1].getParent().getParent().getChildren();
            List<Node<int>> maxNodes1List = minNodesList[1].getParent().getParent().getChildren();
            List<int> IdsList = new List<int>();
            Boolean isHere = false;
            for (int k = 0; k < minNodesList.Count; k++)
            {
                int min = int.MaxValue;
                int tempMin, h = 0;

                for (int i = 0; i < maxNodes2list.Count(); i++)
                {
                    tempMin = maxNodes2list[i].getHeuristic();
                    if (tempMin < min && maxNodes2list[i].getHeuristic() != -1)
                    {
                        min = Math.Min(min, tempMin);
                        h = i;
                        isHere = true;
                    }
                }

                if (isHere)
                {
                    maxNodes2list[h].getParent().setHeuristic(min);
                    IdsList.Add(maxNodes2list[h].getParent().getData());
                    maxNodes2list.Remove(maxNodes2list[h]);
                }

                isHere = false;
            }

            isHere = false;
            IdsList.Clear();
            for (int k = 0; k < maxNodes1List.Count; k++)
            {
                int max = int.MinValue;
                int tempMax, h = 0;

                for (int i = 0; i < minNodesList.Count(); i++)
                {
                    tempMax = minNodesList[i].getHeuristic();
                    if (tempMax > max && minNodesList[i].getHeuristic() != -1)
                    {
                        max = Math.Max(max, tempMax);
                        h = i;
                        isHere = true;
                    }
                }

                if (isHere)
                {
                    minNodesList[h].getParent().setHeuristic(max);
                    IdsList.Add(minNodesList[h].getParent().getData());
                    minNodesList.Remove(maxNodes2list[h]);
                }

                isHere = false;
            }

            int minH = int.MaxValue;
            int tempMinH, l = 0;

            for (int i = 0; i < maxNodes1List.Count(); i++)
            {
                tempMinH = maxNodes1List[i].getHeuristic();
                if (tempMinH < minH)
                {
                    minH = Math.Min(minH, tempMinH);
                    l = i;
                }
            }

            maxNodes1List[l].getParent().setHeuristic(minH);
            Node<int> attackingNode = maxNodes1List[l];
            Node<int> attackedNode = new Node<int>(0, 0);

            foreach (Node<int> a in attackingNode.getChildren())
            {
                if (a.getHeuristic() == attackingNode.getHeuristic())
                {
                    attackedNode = a;
                }
            }

            int attackingCityTroops = USA_City_details.Instance.GetCity(attackingNode.getData()).No_of_Soliders();
            int attackedCityTroops = USA_City_details.Instance.GetCity(attackedNode.getData()).No_of_Soliders();
            int numberOfAttackingTroops = 0;
            if (!(attackingCityTroops < 2))
            {
                numberOfAttackingTroops = (attackingCityTroops % attackedCityTroops) + 1;
            }

            int[] resultArray = { attackingNode.getData(), attackedNode.getData(), numberOfAttackingTroops };
            return resultArray;
        }

        int getHeuristic(int id, Boolean[] minTroops)
        {
            int BSR, sumOfBST = 0;
            List<int> listOfNeighbours = USA_City_details.Instance.GetCity(id).GetNeighbours();
            for (int i = 0; i < listOfNeighbours.Count; i++)
            {
                if (minTroops[listOfNeighbours[i]])
                {
                    sumOfBST += USA_City_details.Instance.GetCity(listOfNeighbours[i]).No_of_Soliders();
                }
            }
            BSR = (sumOfBST) / (USA_City_details.Instance.GetCity(id).No_of_Soliders());
            return BSR;
        }



    }
}
