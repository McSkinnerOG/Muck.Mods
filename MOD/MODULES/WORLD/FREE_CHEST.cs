using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine; 
namespace MOD.MODULES.WORLD
{
    public class FREE_CHEST : MonoBehaviour
    {
        public static int time = 0;
        public static bool Auto = false;
        public static ConfigEntry<KeyCode> KeyCode_FREE_CHEST;
        public static void FixedUpdate()
        { 
            if (Auto == true)
            {
                time++; Debug.Log(time);
                if (time >= 145) { Unlock(); time = 0; return; }
            } 
        }
        public static void Update() { if (Input.GetKeyDown(KeyCode_FREE_CHEST.Value)) { Auto = !Auto; } }
        public static void Unlock()
        {
            var x = FindObjectsOfType<LootContainerInteract>(); 
            foreach (LootContainerInteract item in x) { item.basePrice = 0; } 
            Debug.Log("FINISHED FREE CHEST FUNCTION CORRECTLY");
        } 
    }
}
