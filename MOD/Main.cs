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
        public void FixedUpdate()
        {
            MODULES.ONE_HIT.FixedUpdate();
        }
        public void Update()
        {
            
            if (Input.GetKey(Heal1.Value)) { ClientSend.PlayerHit((int)DAMAGE_VALUE.Value, GameManager.players[0].id, 9f, 0, base.transform.position); }
            if (Input.GetKey(Heal2.Value)) { ClientSend.PlayerHit(99, GameManager.players[1].id, 9f, 0, base.transform.position); }
            if (Input.GetKey(Hurt1.Value)) { ClientSend.RevivePlayer(GameManager.players[0].id); }
            if (Input.GetKey(Hurt2.Value)) { ClientSend.RevivePlayer(GameManager.players[LocalClient.instance.myId].id); }
            if (Input.GetKey(KeyCode.C))
            {
                if (lp_movement == null) { LocalPlayer_Search(); }
                lp_input.crouching = true;
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                if (lp_movement == null) { LocalPlayer_Search(); }
                lp_input.crouching = false; 
            } 
        }
        public static ConfigEntry<KeyCode> Heal1;
        public static ConfigEntry<KeyCode> Heal2;
        public static ConfigEntry<KeyCode> Hurt1;
        public static ConfigEntry<KeyCode> Hurt2; 
        public static ConfigEntry<float> DAMAGE_VALUE; 
        public void InitConfig()
        {
            Heal1 = Config.Bind("Player", "Heal1", KeyCode.F1, "Apply Settings");
            Heal2 = Config.Bind("Player", "Heal2", KeyCode.F2, "FREECAM");
            Hurt1 = Config.Bind("Player", "Hurt1", KeyCode.F3, "FLIGHT");
            Hurt2 = Config.Bind("Player", "Hurt2", KeyCode.F4, "FLIGHT");  
            DAMAGE_VALUE = Config.Bind("Player", "DAMAGE", 999f, new ConfigDescription("Flight Speed", new AcceptableValueRange<float>(1f, 9999999f)));
        }
    }
}