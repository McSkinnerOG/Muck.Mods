﻿using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using System.Reflection; 
namespace MOD
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Main : BaseUnityPlugin
    {
        #region[Declarations] 
        public const string MODNAME = "MOD", AUTHOR = "", GUID = AUTHOR + "_" + MODNAME, VERSION = "1.0.0.0"; 
        internal readonly ManualLogSource log;
        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;
        #endregion
        public void Start() { harmony.PatchAll(assembly); }
        public Main()
        {
            log = Logger; harmony = new Harmony(GUID);
            assembly = Assembly.GetExecutingAssembly();
            modFolder = Path.GetDirectoryName(assembly.Location);
        }


        public static PlayerMovement lp_movement;
        public static PlayerInput lp_input;
        public static PlayerManager lp_pmanager;
        public static PlayerStatus lp_pstatus;
        public static HitableActor lp_hittable;
        public static DetectInteractables lp_interact; 
        public static PingController lp_pingController;

        public static PlayerMovement op_movement;
        public static PlayerInput op_input;
        public static PlayerManager op_pmanager;
        public static PlayerStatus op_pstatus;
        public static HitableActor op_hittable;
        public static DetectInteractables op_interact;
        public static PingController op_pingController;



        //All
        public static PlayerStatus[] All_Player_Status;
        public static OnlinePlayer[] Player_List;

        public void GetPlayerByID(int id)
        {
            Player_List = FindObjectsOfType<OnlinePlayer>();
            foreach (OnlinePlayer op in Player_List)
            { 
                op_pmanager = op.GetComponent<PlayerManager>();
                if (op_pmanager.id == id)
                {
                    //do stuff
                    op_hittable = op.GetComponent<HitableActor>();
                    op_pmanager = op.GetComponent<PlayerManager>();
                    return;
                }
            }
        } 
        public void OnlinePlayer_Search()
        {
            Player_List = FindObjectsOfType<OnlinePlayer>();
            foreach (OnlinePlayer op in Player_List)
            {
                op_hittable = op.GetComponent<HitableActor>();
                op_pmanager = op.GetComponent<PlayerManager>();
            }
        }
        public void LocalPlayer_Search()
        {
            if (lp_movement == null)
            {
                lp_movement = FindObjectOfType<PlayerMovement>();
                lp_input = FindObjectOfType<PlayerInput>();

                lp_pstatus = FindObjectOfType<PlayerStatus>();
                lp_hittable = FindObjectOfType<HitableActor>();
                lp_interact = FindObjectOfType<DetectInteractables>();
                lp_pingController = FindObjectOfType<PingController>();
            }
            if (lp_pmanager == null) { lp_pmanager = lp_movement.gameObject.GetComponent<PlayerManager>(); }
        }

        public void Update()
        {

        } 
    }
} 