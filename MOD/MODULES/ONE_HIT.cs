using System;
using System.Collections;
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
        public static void FixedUpdate()
        {
            time++;
            Debug.Log(time);
            if (time >= 100)
            { 
                OneHit();
                time = 0;
                return;
            } 
        } 
        public static void OneHit()
        {
            var x = FindObjectsOfType<LootContainerInteract>();
            var t = FindObjectsOfType<HitableTree>();
            var m = FindObjectsOfType<HitableMob>();
            var r = FindObjectsOfType<HitableRock>();
            var a = FindObjectsOfType<HitableActor>();
            foreach (LootContainerInteract item in x)
            {
                item.basePrice = 0;
            }
            foreach (HitableTree item in t)
            {
                item.hp = 1;
            }
            foreach (HitableMob item in m)
            {
                item.hp = 1;
            }
            foreach (HitableRock item in r)
            {
                item.hp = 1;
            }
            foreach (HitableActor item in a)
            {
                item.hp = 1; 
            }
            Debug.Log("FINISHED ONE HIT FUNCTION CORRECTLY");
        } 
    }
}
