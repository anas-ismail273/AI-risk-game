using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_game_AI
{
    class Game_state
    {
        private int turn;
        private int phase;
        private int map;
        private bool[] P1Agents ;
        private bool[] P2Agents;

        // map == 1 (USA)  , map == 2 (Egypt)

        private Game_state()
        {
            turn = 1;
            phase = 1;
            map = 1;
            P1Agents = new bool[8];
            for (int i = 0; i < 8; i++)
                P1Agents[i] = false;
            P2Agents = new bool[8];
            for (int i = 0; i < 8; i++)
                P2Agents[i] = false;
        }

        public string Get_Turn_name ()
        {
            if (turn == 1)
                return "Player1\nTurn";
            else
                return "Player2\nTurn";
        }

        public int GetTurn()
        {
            return turn;
        }

        public int GetPhase()
        {
            return phase;
        }
        public void ChangeTurn ()
        {
            if (turn == 2)
                turn = 1;
            else
                turn = 2;
        }

        public void ChangePhase()
        {
            if (phase == 2)
                phase = 1;
            else
                phase = 2;
        }

        public void SetMap (int Map)
        {
            this.map = Map;
        }

        public bool WithMe (int id)
        {
            if (turn == 1 && map == 1)
                return USA_City_details.Instance.with_P1[id];
            else 
                return USA_City_details.Instance.with_P2[id];
        }

        public bool[] WithMeArray()
        {
            if (turn == 1 && map == 1)
                return USA_City_details.Instance.with_P1;
            else
                return USA_City_details.Instance.with_P2;
        }

        public bool[] NotWithMeArray()
        {
            if (turn == 1 && map == 1)
                return USA_City_details.Instance.with_P2;
            else
                return USA_City_details.Instance.with_P1;
        }

        public bool is_p1_turn ()
        {
            if (turn == 1)
                return true;
            else
                return false;
        }

        public int no_of_map_cities ()
        {
            if (map == 1)
                return 50;
            else
                return 26;
        }

        public Color GetPrimaryColor ()
        {
            if (turn == 1)
                return ColorTranslator.FromHtml("#ec1c24");
            else
                return ColorTranslator.FromHtml("#3f48cc");
        }

        public Color GetSecondaryColor()
        {
            if (turn == 1)
                return ColorTranslator.FromHtml("#fc8181");
            else
                return ColorTranslator.FromHtml("#6584ff");
        }

        public void SetAgents(int p1 , int p2)
        {
            P1Agents[p1] = true;
            P2Agents[p2] = true;
        }

        public int getThisTurnAgent ()
        {
            if (turn == 1)
            {
                for (int i = 0; i < 8; i++)
                    if (P1Agents[i] == true)
                        return i;
                     return 0;
            }
            else
                for (int i = 0; i < 8; i++)
                    if (P2Agents[i] == true)
                        return i;
                     return 0;

        }
        private static Lazy<Game_state> instance = new Lazy<Game_state>(() => new Game_state());

        public static Game_state Instance => instance.Value;
    }
}
