using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MOD.MODULES.MOVEMENT
{
    public class FLIGHT
    {

        static bool flyMode = false;

        public static void Update()
        {
            if (flyMode)
            {
                float d = 125f;
                Vector3 position = Main.lp_movement.transform.position + new Vector3(0f, 0.00012f, 0f);

                if (Input.GetKey(KeyCode.Mouse1) && flyMode)
                {
                    Quaternion rotation = Camera.main.transform.rotation;
                    rotation.x = 0f;
                    rotation.z = 0f;
                    Main.lp_movement.transform.rotation = rotation;
                }
                if (Input.GetKey(KeyCode.LeftShift) && flyMode)
                {
                    d = 450;
                }
                if (Input.GetKey(KeyCode.LeftAlt) && flyMode)
                {
                    d = 15f;
                }
                if (Input.GetKey(KeyCode.W))
                {
                    position = (Main.lp_movement.transform.position += Main.lp_movement.transform.forward * Time.deltaTime * d);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    position = (Main.lp_movement.transform.position -= Main.lp_movement.transform.forward * Time.deltaTime * d);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    position = (Main.lp_movement.transform.position -= Main.lp_movement.transform.right * Time.deltaTime * d);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    position = (Main.lp_movement.transform.position += Main.lp_movement.transform.right * Time.deltaTime * d);
                }
                if (Input.GetKey(KeyCode.Q))
                {
                    position = (Main.lp_movement.transform.position += Main.lp_movement.transform.up * Time.deltaTime * d);
                }
                if (Input.GetKey(KeyCode.E))
                {
                    position = (Main.lp_movement.transform.position -= Main.lp_movement.transform.up * Time.deltaTime * d);
                }
                Main.lp_movement.transform.position = position;
                Main.lp_movement.transform.rotation = Quaternion.LookRotation(Main.lp_movement.transform.forward);
                Rigidbody getRigidbody = Main.lp_movement.GetComponentInParent<Rigidbody>();
                getRigidbody.velocity = Vector3.zero;
                getRigidbody.angularVelocity = Vector3.zero;
                float num = 2f;
                float num2 = 2f;
                float yAngle = num * Input.GetAxis("Mouse X");
                float num3 = num2 * Input.GetAxis("Mouse Y");
                Main.lp_movement.transform.Rotate(-num3, yAngle, 0f);
            }
            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                Main.LocalPlayer_Search();
                flyMode = !flyMode;
            }
        }
    }
}
