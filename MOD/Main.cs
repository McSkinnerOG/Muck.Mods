using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib; 
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MOD
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Main : BaseUnityPlugin
    {
        #region[Declarations] 
        public const string MODNAME = "MOD", AUTHOR = "", GUID = AUTHOR + "_" + MODNAME, VERSION = "1.0.0.2"; 
        internal readonly ManualLogSource log;
        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;
        #endregion
        public void Start() { harmony.PatchAll(assembly); InitConfig(); }
        public Main()
        {
            log = Logger; harmony = new Harmony(GUID);
            assembly = Assembly.GetExecutingAssembly();
            modFolder = Path.GetDirectoryName(assembly.Location);
        }

        #region[LOCALPLAYER]
        public static PlayerMovement lp_movement;
        public static PlayerInput lp_input;
        public static PlayerManager lp_pmanager;
        public static PlayerStatus lp_pstatus;
        public static HitableActor lp_hittable;
        public static DetectInteractables lp_interact; 
        public static PingController lp_pingController;
        #endregion
        #region[ONLINEPLAYER]
        public static PlayerMovement op_movement;
        public static PlayerInput op_input;
        public static PlayerManager op_pmanager;
        public static PlayerStatus op_pstatus;
        public static HitableActor op_hittable;
        public static DetectInteractables op_interact;
        public static PingController op_pingController;
        #endregion 
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
                    op_hittable = op.GetComponent<HitableActor>();
                    op_pmanager = op.GetComponent<PlayerManager>();
                    return;
                }
            }
        }
        public void GetPlayerByName(string username)
        {
            Player_List = FindObjectsOfType<OnlinePlayer>();
            foreach (OnlinePlayer op in Player_List)
            {
                op_pmanager = op.GetComponent<PlayerManager>();
                if (op_pmanager.username == username)
                {
                    op_hittable = op.GetComponent<HitableActor>();
                    op_pmanager = op.GetComponent<PlayerManager>();
                    return;
                }
            }
        }
        public void OnlinePlayer_Search()
        {
            GetPlayerByID(0);
            Player_List = FindObjectsOfType<OnlinePlayer>();
            foreach (OnlinePlayer op in Player_List)
            {
                op_hittable = op.GetComponent<HitableActor>();
                op_pmanager = op.GetComponent<PlayerManager>();
            }
        }
        public static void LocalPlayer_Search()
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
        public void FixedUpdate() 
        {
            // Only run our stuff while INGAME
            if (SceneManager.GetActiveScene().name == "GameAfterLobby")
            {
                MODULES.WORLD.ONE_HIT.FixedUpdate();
                MODULES.WORLD.FREE_CHEST.FixedUpdate();
            }
        }
        public void Update()
        {
            // Only run our stuff while INGAME
            if (SceneManager.GetActiveScene().name == "GameAfterLobby")
            {
                // Call other classes Updates
                MODULES.STATS.GOD.Update();
                MODULES.STATS.HUNGER.Update();
                MODULES.STATS.SHIELD.Update();
                MODULES.STATS.STAMINA.Update();
                MODULES.MOVEMENT.SLIDING.Update();
                MODULES.MOVEMENT.SPEEDHACK.Update();
                MODULES.MOVEMENT.TELEPORT.Update();
                //if (Input.GetKey(KeyCode_KillOthers.Value)) { ClientSend.PlayerHit((int)DAMAGE_VALUE.Value, GameManager.players[1].id, (int)DAMAGE_VALUE.Value, 0, base.transform.position); }
                //if (Input.GetKey(KeyCode_KillMe.Value)) { ClientSend.PlayerHit((int)DAMAGE_VALUE.Value, GameManager.players[LocalClient.instance.myId].id, (int)DAMAGE_VALUE.Value, 0, base.transform.position); }
                //if (Input.GetKey(KeyCode_ReviveOthers.Value)) { ClientSend.RevivePlayer(GameManager.players[1].id); }
                //if (Input.GetKey(KeyCode_ReviveMe.Value)) { ClientSend.RevivePlayer(GameManager.players[LocalClient.instance.myId].id); } 
                if (Input.GetKeyDown(KeyCode_Cheats.Value))
                {
                    LocalPlayer_Search(); 
                    lp_pstatus.shield = 9999;
                    lp_pstatus.maxShield = 9999;
                    lp_pstatus.hunger = 9999;
                }
                if (Input.GetKeyDown(KeyCode_GRIEFER.Value))
                {
                    var m = FindObjectsOfType<HitableMob>(); 
                    foreach (HitableMob mob in m) { mob.hp = 0; }
                }
            } 
        }
        #region[Bepinex Config Entries] 
        public static ConfigEntry<float> DAMAGE_VALUE; 
        public static ConfigEntry<KeyCode> KeyCode_Cheats; 
        //public static ConfigEntry<KeyCode> KeyCode_KillOthers;
        //public static ConfigEntry<KeyCode> KeyCode_KillMe;
        //public static ConfigEntry<KeyCode> KeyCode_ReviveOthers;
        //public static ConfigEntry<KeyCode> KeyCode_ReviveMe;
        public static ConfigEntry<KeyCode> KeyCode_GRIEFER;
        
        public static ConfigEntry<string> playername;
        public void InitConfig()
        {
            KeyCode_Cheats = Config.Bind("Cheats", "Infinite Hp, Shield, Stam, No Hunger", KeyCode.F1, "LMFAO!");
            KeyCode_GRIEFER = Config.Bind("Cheats", "Hehe", KeyCode.F6, "Set Mobs hp to infinity");
            DAMAGE_VALUE = Config.Bind("Cheats", "DAMAGE", 999f, new ConfigDescription("Flight Speed", new AcceptableValueRange<float>(1f, 9999999f)));
            //KeyCode_KillOthers = Config.Bind("Cheats", "KillOthers", KeyCode.F2, "Kill OnlinePlayer");
            //KeyCode_KillMe = Config.Bind("Cheats", "KillMe", KeyCode.F3, "Kill LocalPlayer");
            //KeyCode_ReviveMe = Config.Bind("Cheats", "ReviveMe", KeyCode.F4, "Insta res");
            //KeyCode_ReviveOthers = Config.Bind("Cheats", "ReviveOthers", KeyCode.F5, "Insta res");

            //STAMINA
            MODULES.STATS.STAMINA.Stamina = Config.Bind("Cheats", "Stamina", 999f, new ConfigDescription("Current/Active Stamina Value", new AcceptableValueRange<float>(1f, 9999999f))); 
            MODULES.STATS.STAMINA.StaminaMax = Config.Bind("Cheats", "Stamina Max", 999f, new ConfigDescription("Maximum Stamina Value", new AcceptableValueRange<float>(1f, 9999999f))); 
            MODULES.STATS.STAMINA.StaminaDrain = Config.Bind("Cheats", "Stamina Drain", 999f, new ConfigDescription("Stamina Drain Amount Over Time", new AcceptableValueRange<float>(0f, 1000)));
            MODULES.STATS.STAMINA.KeyCode_InfStamina = Config.Bind("Cheats", "Infinite Stamina", KeyCode.Backspace, "Infinite Stamina!");
            MODULES.STATS.STAMINA.KeyCode_SetStamina = Config.Bind("Cheats", "Set Stamina", KeyCode.Backspace, "Set Stamina!");
            //HUNGER
            MODULES.STATS.HUNGER.Hunger = Config.Bind("Cheats", "Hunger", 999f, new ConfigDescription("Current/Active Hunger Value", new AcceptableValueRange<float>(1f, 9999999f)));
            MODULES.STATS.HUNGER.HungerMax = Config.Bind("Cheats", "Hunger Max", 999f, new ConfigDescription("Maximum Hunger Value", new AcceptableValueRange<float>(1f, 9999999f)));
            MODULES.STATS.HUNGER.HungerDrain = Config.Bind("Cheats", "Hunger Drain", 999f, new ConfigDescription("Hunger Drain Amount Over Time", new AcceptableValueRange<float>(0f, 1000)));
            MODULES.STATS.HUNGER.KeyCode_InfHunger = Config.Bind("Cheats", "Infinite Hunger", KeyCode.Backspace, "Infinite Hunger!");
            MODULES.STATS.HUNGER.KeyCode_SetHunger = Config.Bind("Cheats", "Set Hunger", KeyCode.Backspace, "Set Hunger!");
            //SHIELD 
            MODULES.STATS.SHIELD.Shield = Config.Bind("Cheats", "Shield", 999f, new ConfigDescription("Current/Active Hunger Value", new AcceptableValueRange<float>(1f, 9999999f)));
            MODULES.STATS.SHIELD.ShieldMax = Config.Bind("Cheats", "Shield Max", 999, new ConfigDescription("Maximum Hunger Value", new AcceptableValueRange<float>(1f, 9999999f))); 
            MODULES.STATS.SHIELD.KeyCode_InfShield = Config.Bind("Cheats", "Infinite Shield", KeyCode.Backspace, "Infinite Shield!");
            MODULES.STATS.SHIELD.KeyCode_SetShield = Config.Bind("Cheats", "Set Shield", KeyCode.Backspace, "Set Shield!");
            //GOD
            MODULES.STATS.GOD.HP = Config.Bind("Cheats", "HP", 100f, new ConfigDescription("Player HP", new AcceptableValueRange<float>(1f, 100000f)));
            MODULES.STATS.GOD.KeyCode_Godmode = Config.Bind("Cheats", "GODMODE", KeyCode.G, "GODMODE");
            //MOVEMENT
            MODULES.MOVEMENT.SLIDING.KeyCode_Sliding = Config.Bind("Cheats", "Sliding", KeyCode.C, "Sliding!");
            MODULES.MOVEMENT.SPEEDHACK.RUN_SPEED = Config.Bind("Cheats", "Run Speed Slider", 1f, new ConfigDescription("Running Speed", new AcceptableValueRange<float>(0f, 1500f)));  
            MODULES.WORLD.ONE_HIT.KeyCode_ONE_HIT = Config.Bind("Cheats", "ONEHIT", KeyCode.O, "ONEHIT!");
            MODULES.WORLD.FREE_CHEST.KeyCode_FREE_CHEST = Config.Bind("Cheats", "FREECHEST", KeyCode.P, "FREECHEST!"); 
        }
        #endregion
    }
}