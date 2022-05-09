using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace MOD.MODULES
{
    public class GOD : MonoBehaviour
    {
        public static bool god = false;
        public static ConfigEntry<float> HP;
        public static void Update()
        {
            if (god)
            {
                // if our "lp_pstatus" reference can find required component 
                // we set our "maxHp" equal to the sum of "hp + maxShield"
                if (Main.lp_pstatus != null) 
                { 
                    // HP (Current Health) = (Max HP - Max Shield);
                    Main.lp_pstatus.hp = (int)HP.Value; 
                    // Set 'HP' and 'Shield' to their individual maximums
                    Main.lp_pstatus.shield = Main.lp_pstatus.maxShield;
                    // Maximum Health = (HP + Maximum Shield)
                    // NB: Max Shield is same as Shield
                    Main.lp_pstatus.maxHp = (int)HP.Value + Main.lp_pstatus.maxShield;
                }
                // if our "lp_pstatus" reference can NOT find required component we do a search for it so next cycle the code runs correctly.
                else { Main.LocalPlayer_Search(); }
            } 
        } 
    }
}
