using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_game_AI
{
    class game_play
    {
        public bool attack (int attacker , int attacked , int no_of_troops_succeded , bool is_attacker_p1)
        {
            var d = USA_City_details.Instance;
            int attacking_troops = d.GetCity(attacker).No_of_Soliders();
            int attacked_troops = d.GetCity(attacked).No_of_Soliders();


            if (attacked_troops <= no_of_troops_succeded && attacked_troops <3)
            {
                d.GetCity(attacker).Set_No_of_Soliders( attacking_troops - no_of_troops_succeded);
                d.GetCity(attacked).Set_No_of_Soliders(no_of_troops_succeded);
                if (is_attacker_p1)
                {
                    d.with_P1[attacked] = true;
                    d.with_P2[attacked] = false;
                }
                else
                {
                    d.with_P2[attacked] = true;
                    d.with_P1[attacked] = false;
                }

                return true;
                
            }
            else
            {
                if (no_of_troops_succeded<=2)
                d.GetCity(attacked).Set_No_of_Soliders(attacked_troops - no_of_troops_succeded);
                else
                    d.GetCity(attacked).Set_No_of_Soliders(attacked_troops - 2);

                return false;
            }

        }

        public void deploy (int city_id , int no_of_soldiers)
        {
            USA_City_details.Instance.GetCity(city_id).Set_No_of_Soliders(USA_City_details.Instance.GetCity(city_id).No_of_Soliders() + no_of_soldiers);
        }
    }
}
