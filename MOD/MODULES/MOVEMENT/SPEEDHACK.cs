using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace MOD.MODULES.MOVEMENT
{
    public class SPEEDHACK : MonoBehaviour
    {
        public static ConfigEntry<float> RUN_SPEED;
        public static bool speedhack = false;
        public static void Update()
        {
            if (speedhack)
            {
                Main.LocalPlayer_Search(); 
                Main.lp_pstatus.currentSpeedArmorMultiplier = RUN_SPEED.Value;
            } else { return; }
        }
    }
}
