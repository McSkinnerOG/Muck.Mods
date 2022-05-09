 
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
 
namespace MOD.MODULES.WORLD
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
            var trees = FindObjectsOfType<HitableTree>();
            var mobs = FindObjectsOfType<HitableMob>();
            var rocks = FindObjectsOfType<HitableRock>(); 
            // Set the Tree/Rock/Mob to 1 health |
            //for trees and rocks we also set the "compatibleItem" to a generic "Item" insted of "Pickaxe" or "Axe" etc
            foreach (HitableTree tree in trees) { tree.hp = 1; tree.compatibleItem = InventoryItem.ItemType.Item; tree.minTier = 0; }
            foreach (HitableMob mob in mobs) { mob.hp = 1; }
            foreach (HitableRock rock in rocks) { rock.hp = 1; rock.compatibleItem = InventoryItem.ItemType.Item; rock.minTier = 0; }
            // TODO: FILTER FOR TRADERS
            //foreach (HitableActor item in a)
            //{
            //    item.hp = 1; 
            //}
            Debug.Log("FINISHED ONE HIT FUNCTION CORRECTLY");
        } 
    }
}
