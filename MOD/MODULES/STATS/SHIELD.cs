using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MOD.MODULES.STATS
{
    public class SHIELD
    {
        public static ConfigEntry<float> Shield;
        public static ConfigEntry<int> ShieldMax;
        public static ConfigEntry<float> ShieldDrain;
        public static ConfigEntry<KeyCode> KeyCode_InfShield;
        public static ConfigEntry<KeyCode> KeyCode_SetShield;
        public static bool inf_Shield = false;
        public static void Update()
        {
            if (SceneManager.GetActiveScene().name == "GameAfterLobby")
            {
                if (Input.GetKeyDown(KeyCode_InfShield.Value)) { inf_Shield = !inf_Shield; }
                if (inf_Shield)
                {
                    if (Main.lp_pstatus != null) { Main.lp_pstatus.shield = ShieldMax.Value; }
                    else { Main.LocalPlayer_Search(); }
                }
                if (Input.GetKeyDown(KeyCode_SetShield.Value))
                {
                    if (Main.lp_pstatus != null)
                    {
                        Main.lp_pstatus.shield = Shield.Value;
                        Main.lp_pstatus.maxShield = ShieldMax.Value; 
                    }
                    else { Main.LocalPlayer_Search(); }
                }
            }
        }
    }
}
