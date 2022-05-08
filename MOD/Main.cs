using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Collections;

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
        public static ImpactDamage Dmg;



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
                Dmg = FindObjectOfType<ImpactDamage>();
            }
            if (lp_pmanager == null) { lp_pmanager = lp_movement.gameObject.GetComponent<PlayerManager>(); }
        }

        public void Awake() { InitConfig(); }

        public void FixedUpdate()
        {
            Modules.One_Hitter.FixedUpdate();
        }

        public void Update()
        {
            //Cheats
            if (Input.GetKeyDown(KeycodeCheats.Value))
            {
                LocalPlayer_Search();
                lp_pstatus.hp = 999;
                lp_pstatus.maxHp = 999;
                lp_pstatus.stamina = 99999;
                lp_pstatus.maxStamina = 99999;
                lp_pstatus.shield = 9999;
                lp_pstatus.maxShield = 9999;
                lp_pstatus.hunger = 9999;
                lp_pstatus.currentSpeedArmorMultiplier = RunSpd.Value;
            }
            //Kill your teammate, revive yourself :)
            if (Input.GetKey(Keycodekillu.Value)) { ClientSend.PlayerHit(9999,GameManager.players[1].id, 9999, 9999, base.transform.position); }
            if (Input.GetKey(KeycodeRev.Value)) {  ClientSend.RevivePlayer(GameManager.players[0].id); }
            if (Input.GetKeyDown(KeyCodeGetFucked.Value))
            {
                var m = FindObjectsOfType<HitableMob>();
                foreach (HitableMob mob in m)
                {
                    mob.hp = 0;
                }
            }
        }

        #region[Bepinex Config Entries]
        public static ConfigEntry<KeyCode> KeycodeCheats;
        public static ConfigEntry<KeyCode> Keycodekillu;
        public static ConfigEntry<KeyCode> KeycodeRev;
        public static ConfigEntry<KeyCode> KeyCodeGetFucked;
        public static ConfigEntry<float> RunSpd;
        #endregion 

        public void InitConfig()
        {
            KeycodeCheats = Config.Bind("Cheats", "Infinite Hp, Shield, Stam, No Hunger", KeyCode.F1, "LMFAO!");
            Keycodekillu = Config.Bind("Cheats", "Get sauced", KeyCode.F2, "Kill player");
            KeycodeRev = Config.Bind("Cheatz", "Revive", KeyCode.F3, "Insta res");
            KeyCodeGetFucked = Config.Bind("Cheats", "Hehe", KeyCode.F4, "Set Mobs hp to infinit");
            RunSpd = Config.Bind("Run speed", "Run Speed Slider", 1f, new ConfigDescription("Running Speed", new AcceptableValueRange<float>(0f, 1500f)));
        }
    }
} 