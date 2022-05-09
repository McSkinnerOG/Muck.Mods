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
    public class HUNGER
    { 
        public static ConfigEntry<float> Hunger;
        public static ConfigEntry<float> HungerMax;
        public static ConfigEntry<float> HungerDrain;
        public static ConfigEntry<KeyCode> KeyCode_InfHunger;
        public static ConfigEntry<KeyCode> KeyCode_SetHunger;
        public static bool inf_hunger = false;
        public static void Update()
        {
            if (SceneManager.GetActiveScene().name == "GameAfterLobby")
            {
                if (Input.GetKeyDown(KeyCode_InfHunger.Value)) { inf_hunger = !inf_hunger; }
                if (inf_hunger)
                { 
                    if (Main.lp_pstatus != null) { Main.lp_pstatus.hunger = HungerMax.Value; }
                    else { Main.LocalPlayer_Search(); }
                }
                if (Input.GetKeyDown(KeyCode_SetHunger.Value))
                {
                    if (Main.lp_pstatus != null)
                    {
                        Main.lp_pstatus.hunger = Hunger.Value;
                        Main.lp_pstatus.maxHunger = HungerMax.Value;
                        Main.lp_pstatus.hungerDrainRate = HungerDrain.Value;
                    }
                    else { Main.LocalPlayer_Search(); }
                }
            }
        }
    }
}
