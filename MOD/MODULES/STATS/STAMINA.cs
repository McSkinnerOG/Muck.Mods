using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace MOD.MODULES.STATS
{
    public class STAMINA
    {
        public static ConfigEntry<float> Stamina;
        public static ConfigEntry<float> StaminaMax;
        public static ConfigEntry<float> StaminaDrain;
        public static ConfigEntry<KeyCode> KeyCode_InfStamina;
        public static ConfigEntry<KeyCode> KeyCode_SetStamina;
        public static bool inf_stamina = false;
        public static void Update()
        {
            if (SceneManager.GetActiveScene().name == "GameAfterLobby")
            { 
                if (Input.GetKeyDown(KeyCode_InfStamina.Value)) { inf_stamina = !inf_stamina; }
                if (inf_stamina)
                {
                    // if the localplayer status referance "Main.lp_pstatus" is not null we set stamina to max stamina
                    if (Main.lp_pstatus != null) { Main.lp_pstatus.stamina = StaminaMax.Value; }
                    else { Main.LocalPlayer_Search(); }
                }
                if (Input.GetKeyDown(KeyCode_SetStamina.Value))
                {
                    if (Main.lp_pstatus != null)
                    {
                        Main.lp_pstatus.stamina = Stamina.Value;
                        Main.lp_pstatus.maxStamina = StaminaMax.Value;
                        Main.lp_pstatus.staminaDrainRate = StaminaDrain.Value;
                    }
                    else { Main.LocalPlayer_Search(); }
                } 
            } 
        } 
    }
}
