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
    public class GOD : MonoBehaviour
    {
        public static bool god = false;
        public static ConfigEntry<float> HP;
        public static ConfigEntry<KeyCode> KeyCode_Godmode;
        public static void Update()
        {
            if (SceneManager.GetActiveScene().name == "GameAfterLobby")
            {
                if (Input.GetKeyDown(KeyCode_Godmode.Value)) { god = !god; }
                if (god)
                {
                    if (Main.lp_pstatus != null)
                    {
                        // if our "lp_pstatus" reference can find required component 
                        // we set our "maxHp" equal to the sum of "hp + maxShield"
                        // hp being whatever our custom value is thanks to setting a new override for it with configs
                        Main.lp_pstatus.hp = (int)HP.Value;
                        Main.lp_pstatus.shield = Main.lp_pstatus.maxShield;
                        Main.lp_pstatus.maxHp = (int)HP.Value + Main.lp_pstatus.maxShield;
                    }
                    else { Main.LocalPlayer_Search(); }
                    // if our "lp_pstatus" reference can NOT find required component
                    // we do a search for it so next cycle the code runs correctly. 
                }
            }
        } 
    }
}
