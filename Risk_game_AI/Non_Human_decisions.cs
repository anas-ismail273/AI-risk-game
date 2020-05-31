using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_game_AI
{
    class Non_Human_decisions
    {
        Game_state gs = Game_state.Instance;
        passiveAgent passive;
        pacifistAgent pacifist;
        agressiveAgent agressive;
        greedy greedy;
        minimax minimax;
        game_play gp;
        game_controller c;


        public Non_Human_decisions ()
        {
            gp = new game_play();

            passive = new passiveAgent(gs.no_of_map_cities());
            pacifist = new pacifistAgent(gs.no_of_map_cities());
            agressive = new agressiveAgent(gs.no_of_map_cities());
            greedy = new greedy(gs.no_of_map_cities());
            minimax = new minimax();
            c = new game_controller();
        }
        public bool Attack ()
        {
            int[] result = new int[3];
            bool temp = false;
            bool ret = false;
            bool flag = true;

            if (gs.getThisTurnAgent() == 1)   // passive
            {
                // no action
                return false;
            }
            else if (gs.getThisTurnAgent() == 2)   // pacifist
            {
                while (flag)
                {
                    result = pacifist.attack(gs.WithMeArray());
                    if (result[2] == 0)
                        flag = false;
                    else
                    temp = gp.attack(result[0], result[1], result[2], gs.is_p1_turn());
                }

            }
            else if (gs.getThisTurnAgent() == 3)   // aggressive
            {
                while (flag)
                {
                    result = agressive.attack(gs.WithMeArray());
                    if (result[2] == 0)
                        flag = false;
                    else
                        temp = gp.attack(result[0], result[1], result[2], gs.is_p1_turn());
                }
            }
            else if (gs.getThisTurnAgent() == 4)   // greedy
            {
                while (flag)
                {
                    result = greedy.attack(gs.WithMeArray());
                    if (result[2] == 0)
                        flag = false;
                    else
                        temp = gp.attack(result[0], result[1], result[2], gs.is_p1_turn());
                }
            }
            else if (gs.getThisTurnAgent() == 5)   // A*
            {
                List<Astar.Attack> attacks = Astar.Astar.Attack(USA_City_details.Instance.getDictionary(), gs.WithMeArray());
                for (int i=0; i<attacks.Count; i++)
                    temp = gp.attack(attacks[i].MyCountryIndex, attacks[i].EnemyCountryIndex, attacks[i].ArmyNum, gs.is_p1_turn());
            }
            else if (gs.getThisTurnAgent() == 7)  //minmax
            {

                while (flag)
                {
                    result = minimax.minimaxDecisionMaker(gs.WithMeArray(), gs.NotWithMeArray());
                    if (result[2] == 0)
                        flag = false;
                    else
                        temp = gp.attack(result[0], result[1], result[2], gs.is_p1_turn());
                }
            }

            for (int i = 0; i < 3; i++)
                Console.WriteLine(result[i] + "-");

            Console.WriteLine();



            return ret ;
        }

        public void Deploy ()
        {
            int[] result = new int[gs.no_of_map_cities()];
            int no;
            if (gs.getThisTurnAgent() == 1)   // passive
            {
                result = passive.play(gs.WithMeArray());
            }

            if (gs.getThisTurnAgent() == 2)   // pacifist
            {
                result = pacifist.deploy(gs.WithMeArray());
            }
            if (gs.getThisTurnAgent() == 3)   // aggressive
            {
                result = agressive.deploy(gs.WithMeArray());
            }
            if (gs.getThisTurnAgent() == 4)   // greedy
            {
                result = greedy.deploy(gs.WithMeArray());
            }
            /* if (gs.getThisTurnAgent() == 5)   // A*
             {
                 no = Astar.Astar.Draft(USA_City_details.Instance.getDictionary(), gs.WithMeArray(), c.CalculateDeployTroops());
                 gp.deploy(no, c.CalculateDeployTroops());
                 return;
             }*/
            if (gs.getThisTurnAgent() == 5)   // A*
            {
                result = greedy.deploy(gs.WithMeArray());
            }
            if (gs.getThisTurnAgent() == 7)   // minmax
            {
                result = greedy.deploy(gs.WithMeArray());
            }

            for (int i = 0; i < gs.no_of_map_cities(); i++)
                gp.deploy(i, result[i]);
        }
    }
}
