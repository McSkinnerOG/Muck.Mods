using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine;

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
        public GameObject Hp;

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
        public void FixedUpdate() { MODULES.ONE_HIT.FixedUpdate(); }

        public void Update()
        { 
            if (Input.GetKey(KeyCode_KillOthers.Value)) { ClientSend.PlayerHit((int)DAMAGE_VALUE.Value, GameManager.players[1].id, 9f, 0, base.transform.position); }
            if (Input.GetKey(KeyCode_KillMe.Value)) { ClientSend.PlayerHit(99, GameManager.players[LocalClient.instance.myId].id, 9f, 0, base.transform.position); }
            if (Input.GetKey(KeyCode_ReviveOthers.Value)) { ClientSend.RevivePlayer(GameManager.players[1].id); } 
            if (Input.GetKey(KeyCode_ReviveMe.Value)) { ClientSend.RevivePlayer(GameManager.players[LocalClient.instance.myId].id); }
            if (Input.GetKey(KeyCode.C))
            {
                if (lp_movement == null) { LocalPlayer_Search(); } lp_input.crouching = true;
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                if (lp_movement == null) { LocalPlayer_Search(); } lp_input.crouching = false; 
            }
            if (Input.GetKeyDown(KeyCode_Cheats.Value))
            {
                LocalPlayer_Search();
                lp_pstatus.hp = 999;
                lp_pstatus.maxHp = 999;
                lp_pstatus.stamina = 99999;
                lp_pstatus.maxStamina = 99999;
                lp_pstatus.shield = 1000;
                lp_pstatus.maxShield = 1000;
                lp_pstatus.hunger = 1000;
                lp_pstatus.currentSpeedArmorMultiplier = RUN_SPEED.Value;
                Hp = GameObject.Find("UI (1)/Gold/Day/otherHpUI/Players/PlayerStatus(Clone)/RawImage/Hp/ActualHp");
                if (Hp.gameObject == true)
                {
                    Destroy(Hp);
                }
            }
            if (Input.GetKeyDown(KeyCode_GRIEFER.Value))
            {
                var m = FindObjectsOfType<HitableMob>();
                foreach (HitableMob mob in m) { mob.hp = 0; }
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
        public static ConfigEntry<float> RUN_SPEED;
        public void InitConfig()
        { 
            DAMAGE_VALUE = Config.Bind("Player", "Damage Value", 999f, new ConfigDescription("Amount of dmg you will deal to Player", new AcceptableValueRange<float>(1f, 9999999f))); 
            KeyCode_Cheats = Config.Bind("Cheats", "Infinite Hp, Shield, Stam, No Hunger", KeyCode.F1, "LMFAO!");
            KeyCode_Sliding = Config.Bind("Cheats", "Sliding", KeyCode.C, "Sliding!");
            KeyCode_KillOthers = Config.Bind("Cheats", "Get sauced", KeyCode.F2, "Kill player");
            KeyCode_KillMe = Config.Bind("Cheats", "KMS", KeyCode.F2, "Kill player");
            KeyCode_ReviveMe = Config.Bind("Cheatz", "ReviveMe", KeyCode.F3, "Insta res");
            KeyCode_ReviveOthers = Config.Bind("Cheatz", "ReviveOthers", KeyCode.F4, "Insta res");
            KeyCode_GRIEFER = Config.Bind("Cheats", "Hehe", KeyCode.F5, "Set Mobs hp to infinity");
            RUN_SPEED = Config.Bind("Run speed", "Run Speed Slider", 1f, new ConfigDescription("Running Speed", new AcceptableValueRange<float>(0f, 1500f)));
        }
        #endregion
    }
}