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
        public const string MODNAME = "MOD", AUTHOR = "", GUID = AUTHOR + "_" + MODNAME, VERSION = "1.0.0.1"; 
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
        public void FixedUpdate() { MODULES.ONE_HIT.FixedUpdate(); MODULES.FREE_CHEST.FixedUpdate(); }
        public void Update()
        {
            // Only run our stuff while INGAME
            if (SceneManager.GetActiveScene().name == "GameAfterLobby")
            {
                // Call other classes Updates
                MODULES.GOD.Update();
                if (Input.GetKey(KeyCode_KillOthers.Value)) { ClientSend.PlayerHit((int)DAMAGE_VALUE.Value, GameManager.players[1].id, (int)DAMAGE_VALUE.Value, 0, base.transform.position); }
                if (Input.GetKey(KeyCode_KillMe.Value)) { ClientSend.PlayerHit((int)DAMAGE_VALUE.Value, GameManager.players[LocalClient.instance.myId].id, (int)DAMAGE_VALUE.Value, 0, base.transform.position); }
                if (Input.GetKey(KeyCode_ReviveOthers.Value)) { ClientSend.RevivePlayer(GameManager.players[1].id); }
                if (Input.GetKey(KeyCode_ReviveMe.Value)) { ClientSend.RevivePlayer(GameManager.players[LocalClient.instance.myId].id); }
                if (Input.GetKey(KeyCode_Sliding.Value))
                {
                    if (lp_movement == null) { LocalPlayer_Search(); }
                    lp_input.crouching = true;
                }
                if (Input.GetKeyUp(KeyCode_Sliding.Value)) { if (lp_movement == null) { LocalPlayer_Search(); } lp_input.crouching = false; }
                if (Input.GetKeyDown(KeyCode_Cheats.Value))
                {
                    LocalPlayer_Search();
                    MODULES.GOD.god = !MODULES.GOD.god;
                    lp_pstatus.stamina = 99999;
                    lp_pstatus.maxStamina = 99999;
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
        public static ConfigEntry<KeyCode> KeyCode_Sliding;
        public static ConfigEntry<KeyCode> KeyCode_KillOthers;
        public static ConfigEntry<KeyCode> KeyCode_KillMe;
        public static ConfigEntry<KeyCode> KeyCode_ReviveOthers;
        public static ConfigEntry<KeyCode> KeyCode_ReviveMe;
        public static ConfigEntry<KeyCode> KeyCode_GRIEFER;
        
        public static ConfigEntry<string> playername;
        public void InitConfig()
        { 
            DAMAGE_VALUE = Config.Bind("Cheats", "DAMAGE", 999f, new ConfigDescription("Flight Speed", new AcceptableValueRange<float>(1f, 9999999f))); 
            KeyCode_Cheats = Config.Bind("Cheats", "Infinite Hp, Shield, Stam, No Hunger", KeyCode.F1, "LMFAO!"); 
            KeyCode_KillOthers = Config.Bind("Cheats", "KillOthers", KeyCode.F2, "Kill OnlinePlayer");
            KeyCode_KillMe = Config.Bind("Cheats", "KillMe", KeyCode.F3, "Kill LocalPlayer");
            KeyCode_ReviveMe = Config.Bind("Cheats", "ReviveMe", KeyCode.F4, "Insta res");
            KeyCode_ReviveOthers = Config.Bind("Cheats", "ReviveOthers", KeyCode.F5, "Insta res");
            KeyCode_GRIEFER = Config.Bind("Cheats", "Hehe", KeyCode.F6, "Set Mobs hp to infinity");
            KeyCode_Sliding = Config.Bind("Cheats", "Sliding", KeyCode.C, "Sliding!"); 
            MODULES.GOD.HP = Config.Bind("Cheats", "HP", 100f, new ConfigDescription("Player HP", new AcceptableValueRange<float>(1f, 100000f)));
            MODULES.SPEEDHACK.RUN_SPEED = Config.Bind("Cheats", "Run Speed Slider", 1f, new ConfigDescription("Running Speed", new AcceptableValueRange<float>(0f, 1500f)));  
            MODULES.ONE_HIT.KeyCode_ONE_HIT = Config.Bind("Cheats", "ONEHIT", KeyCode.O, "ONEHIT!");
            MODULES.FREE_CHEST.KeyCode_FREE_CHEST = Config.Bind("Cheats", "FREECHEST", KeyCode.P, "FREECHEST!"); 
        }
        #endregion
    }
}