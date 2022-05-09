 
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
 
namespace MOD.MODULES
{
    public class ONE_HIT : MonoBehaviour
    {
        public static int time = 0;
        public static bool Auto = false;
        public static ConfigEntry<KeyCode> KeyCode_ONE_HIT;
        public static void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode_ONE_HIT.Value)) { Auto = !Auto; }
            if (Auto == true)
            {
                //Each frame +1 to the 'time' var
                time++; Debug.Log(time);
                //When time equals 145 or over run the OneHit() function
                //then we set the 'time' var back to 0 and end it all cleanly with return
                if (time >= 145) { OneHit(); time = 0; return; }
            }
        } 
        public static void OneHit()
        { 
            var t = FindObjectsOfType<HitableTree>();
            var m = FindObjectsOfType<HitableMob>();
            var r = FindObjectsOfType<HitableRock>(); 
            foreach (HitableTree item in t) { item.hp = 1; item.minTier = 0; }
            foreach (HitableMob item in m) { item.hp = 1; }
            foreach (HitableRock item in r) { item.hp = 1; }
            // TODO: FILTER FOR TRADERS
            //foreach (HitableActor item in a)
            //{
            //    item.hp = 1; 
            //}
            Debug.Log("FINISHED ONE HIT FUNCTION CORRECTLY");
        } 
    }
}
