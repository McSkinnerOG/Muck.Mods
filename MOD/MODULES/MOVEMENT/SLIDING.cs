using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MOD.MODULES.MOVEMENT
{
    public class SLIDING
    {
        public static ConfigEntry<KeyCode> KeyCode_Sliding;
        public static void Update()
        {
            if (SceneManager.GetActiveScene().name == "GameAfterLobby")
            {
                if (Input.GetKey(KeyCode_Sliding.Value))
                {
                    if (Main.lp_movement == null) { Main.LocalPlayer_Search(); }
                    Main.lp_input.crouching = true;
                }
                if (Input.GetKeyUp(KeyCode_Sliding.Value)) { if (Main.lp_movement == null) { Main.LocalPlayer_Search(); } Main.lp_input.crouching = false; }
            }
        }
    }
}
