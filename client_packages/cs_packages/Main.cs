using System;
using System.Collections.Generic;
using RAGE;
using RAGE.Elements;
using static RAGE.Game.Ped;
using static RAGE.Game.Entity;
using static RAGE.Game.Player;
using static RAGE.Game.Ai;
using static RAGE.Game.Pad;
using static RAGE.Game.Weapon;
using static RAGE.Game.Ui;
using static RAGE.Game.Misc;
using static RAGE.Game.Object;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;

namespace ProjectClient
{
    public enum WeaponHash : uint
    {
        Sniperrifle = 100416529,
        Fireextinguisher = 101631238,
        Compactlauncher = 125959754,
        Snowball = 126349499,
        Vintagepistol = 137902532,
        Combatpdw = 171789620,
        Heavysniper_mk2 = 177293209,
        Heavysniper = 205991906,
        Autoshotgun = 317205821,
        Microsmg = 324215364,
        Wrench = 419712736,
        Pistol = 453432689,
        Pumpshotgun = 487013001,
        Appistol = 584646201,
        Ball = 600439132,
        Molotov = 615608432,
        CeramicPistol = 727643628,
        Smg = 736523883,
        Stickybomb = 741814745,
        Petrolcan = 883325847,
        Stungun = 911657153,
        Stone_hatchet = 940833800,
        Assaultrifle_mk2 = 961495388,
        Heavyshotgun = 984333226,
        Minigun = 1119849093,
        Golfclub = 1141786504,
        Raycarbine = 1198256469,
        Flaregun = 1198879012,
        Flare = 1233104067,
        Grenadelauncher_smoke = 1305664598,
        Hammer = 1317494643,
        Pumpshotgun_mk2 = 1432025498,
        Combatpistol = 1593441988,
        Gusenberg = 1627465347,
        Compactrifle = 1649403952,
        Hominglauncher = 1672152130,
        Nightstick = 1737195953,
        Marksmanrifle_mk2 = 1785463520,
        Railgun = 1834241177,
        Sawnoffshotgun = 2017895192,
        Smg_mk2 = 2024373456,
        Bullpuprifle = 2132975508,
        Firework = 2138347493,
        Combatmg = 2144741730,
        Carbinerifle = 2210333304,
        Crowbar = 2227010557,
        Bullpuprifle_mk2 = 2228681469,
        Snspistol_mk2 = 2285322324,
        Flashlight = 2343591895,
        Proximine = 2381443905,
        NavyRevolver = 2441047180,
        Dagger = 2460120199,
        Grenade = 2481070269,
        Poolcue = 2484171525,
        Bat = 2508868239,
        Specialcarbine_mk2 = 2526821735,
        Doubleaction = 2548703416,
        Pistol50 = 2578377531,
        Knife = 2578778090,
        Mg = 2634544996,
        Bullpupshotgun = 2640438543,
        Bzgas = 2694266206,
        Unarmed = 2725352035,
        Grenadelauncher = 2726580491,
        Musket = 2828843422,
        Advancedrifle = 2937143193,
        Raypistol = 2939590305,
        Rpg = 2982836145,
        Rayminigun = 3056410471,
        Pipebomb = 3125143736,
        HazardCan = 3126027122,
        Minismg = 3173288789,
        Snspistol = 3218215474,
        Pistol_mk2 = 3219281620,
        Assaultrifle = 3220176749,
        Specialcarbine = 3231910285,
        Revolver = 3249783761,
        Marksmanrifle = 3342088282,
        Revolver_mk2 = 3415619887,
        Battleaxe = 3441901897,
        Heavypistol = 3523564046,
        Knuckle = 3638508604,
        Machinepistol = 3675956304,
        Combatmg_mk2 = 3686625920,
        Marksmanpistol = 3696079510,
        Machete = 3713923289,
        Switchblade = 3756226112,
        Assaultshotgun = 3800352039,
        Dbshotgun = 4019527611,
        Assaultsmg = 4024951519,
        Hatchet = 4191993645,
        Bottle = 4192643659,
        Carbinerifle_mk2 = 4208062921,
        Parachute = 4222310262,
        Smokegrenade = 4256991824
    }

    public enum CarbinRifleMk2Components : uint
    {
        COMPONENT_CARBINERIFLE_MK2_CLIP_01 = 0x4C7A391E,
        COMPONENT_CARBINERIFLE_MK2_CLIP_02 = 0x5DD5DBD5,
        COMPONENT_CARBINERIFLE_MK2_CLIP_TRACER = 0x1757F566,
        COMPONENT_CARBINERIFLE_MK2_CLIP_INCENDIARY = 0x3D25C2A7,
        COMPONENT_CARBINERIFLE_MK2_CLIP_ARMORPIERCING = 0x255D5D57,
        COMPONENT_CARBINERIFLE_MK2_CLIP_FMJ = 0x44032F11,
        COMPONENT_AT_AR_AFGRIP_02 = 0x9D65907A,
        COMPONENT_AT_AR_FLSH = 0x7BC4CDDC,
        COMPONENT_AT_SIGHTS = 0x420FD713,
        COMPONENT_AT_SCOPE_MACRO_MK2 = 0x49B2945,
        COMPONENT_AT_SCOPE_MEDIUM_MK2 = 0xC66B6542,
        COMPONENT_AT_AR_SUPP = 0x837445AA,
        COMPONENT_AT_MUZZLE_01 = 0xB99402D4,
        COMPONENT_AT_MUZZLE_02 = 0xC867A07B,
        COMPONENT_AT_MUZZLE_03 = 0xDE11CBCF,
        COMPONENT_AT_MUZZLE_04 = 0xEC9068CC,
        COMPONENT_AT_MUZZLE_05 = 0x2E7957A,
        COMPONENT_AT_MUZZLE_06 = 0x347EF8AC,
        COMPONENT_AT_MUZZLE_07 = 0x4DB62ABE,
        COMPONENT_AT_CR_BARREL_01 = 0x833637FF,
        COMPONENT_AT_CR_BARREL_02 = 0x8B3C480B,
        COMPONENT_CARBINERIFLE_MK2_CAMO = 0x4BDD6F16,
        COMPONENT_CARBINERIFLE_MK2_CAMO_02 = 0x406A7908,
        COMPONENT_CARBINERIFLE_MK2_CAMO_03 = 0x2F3856A4,
        COMPONENT_CARBINERIFLE_MK2_CAMO_04 = 0xE50C424D,
        COMPONENT_CARBINERIFLE_MK2_CAMO_05 = 0xD37D1F2F,
        COMPONENT_CARBINERIFLE_MK2_CAMO_06 = 0x86268483,
        COMPONENT_CARBINERIFLE_MK2_CAMO_07 = 0xF420E076,
        COMPONENT_CARBINERIFLE_MK2_CAMO_08 = 0xAAE14DF8,
        COMPONENT_CARBINERIFLE_MK2_CAMO_09 = 0x9893A95D,
        COMPONENT_CARBINERIFLE_MK2_CAMO_10 = 0x6B13CD3E,
        COMPONENT_CARBINERIFLE_MK2_CAMO_IND_01 = 0xDA55CD3F
    }

    public class cVector3
    {
        public cVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float X;
        public float Y;
        public float Z;
    }

    public class CWeaponObject
    {
        public string Key;
        public uint Hash;
        public int Ammo;
        public int EntityID = -1;
        public string[] Components;
        public float Pos_X;
        public float Pos_Y;
        public float Pos_Z;
        public bool Streamed = false;
    }

    public struct CPropData
    {
        public int Index;
        public int TextureId;
    }

    public class Main : Events.Script
    {
        public static bool ReloadEnabled = true;
        public static uint CurrentWeapon = 0;
        public static int Milliseconds = 0;
        public static int Seconds = 0;
        public static List<CWeaponObject> StreamedWeaponObjects = new List<CWeaponObject>();
        
        public Main()
        {
            Events.CallRemote("ClientLoaded");

            RAGE.Game.Streaming.RequestModel(GetHashKey("w_at_ar_supp"));

            Events.OnEntityCreated += EntityCreated;
            Events.OnEntityStreamIn += OnEntityStreamIn;

            Nametags.Enabled = false;
            Events.Tick += OnTick;
            Events.Tick += OnSecond;
            Events.Tick += RequestModels;
            Events.Add("PlayerQuit", OnPlayerQuit);
            Events.Add("ClearPedProps", ClearPedProps);
            Events.Add("SetPedProp", SetPedProp);
            Events.Add("SetPedPropSync", SetPedPropSync);
            Events.Add("SetPedProps", SetPedProps);
            Events.Add("SetPedPropsSync", SetPedPropsSync);
            Events.Add("ShowWeaponComponents", ShowWeaponComponents);
            Events.Add("SetWeaponComponent", SetWeaponComponent);
            Events.Add("SetWeaponComponentSync", SetWeaponComponentSync);
            Events.Add("SetWeaponAmmo", SetWeaponAmmo);
            Events.Add("SetPedCurrentWeapon", SetPedCurrentWeapon);
            Events.Add("DropPedWeapon", DropPedWeapon);
            Events.Add("DropPedWeaponSync", DropPedWeaponSync);
            Events.Add("AddWeaponObjectToStreamList", AddWeaponObjectToStreamList);
            Events.Add("AddWeaponObjectsToStreamList", AddWeaponObjectsToStreamList);
            Events.Add("ClearWeaponObjectList", ClearWeaponObjectList);

            Events.Add("SetPedHairColor", SetPedHairColor);
        }

        private void OnPlayerQuit(object[] args)
        {
            ClearWeaponObjectList(new object[0]);
        }

        private void ClearWeaponObjectList(object[] args)
        {
            StreamedWeaponObjects.ToList().ForEach((obj) =>
            {
                SetEntityCoords(obj.EntityID, obj.Pos_X, obj.Pos_Y, obj.Pos_Z - 500.0f, false, false, false, false);
                SetEntityVisible(obj.EntityID, false, false);
                FreezeEntityPosition(obj.EntityID, true);

                obj.Streamed = false;
            });

            StreamedWeaponObjects.Clear();

            Events.CallRemote("PlayerReadyToQuit");
        }

        private void AddWeaponObjectsToStreamList(object[] args)
        {
            List<CWeaponObject> _StreamedWeaponObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CWeaponObject>>(args[0].ToString());

            _StreamedWeaponObjects.ForEach((obj) =>
            {

                int weapObject = CreateWeaponObject(obj.Hash, 1, Player.LocalPlayer.Position.X, Player.LocalPlayer.Position.Y, Player.LocalPlayer.Position.Z, true, 0.0f, 0, 0, 0);

                for (int i = 0; i < obj.Components.Length; i++)
                {
                    GiveWeaponComponentToWeaponObject(weapObject, GetHashKey(obj.Components[i]));
                }

                SetEntityCoords(weapObject, obj.Pos_X, obj.Pos_Y, obj.Pos_Z, false, false, false, false);
                SetEntityVisible(weapObject, false, false);
                FreezeEntityPosition(weapObject, true);

                obj.Streamed = false;
                obj.EntityID = weapObject;

                StreamedWeaponObjects.Add(obj);
            });

            Events.CallRemote("AllWeaponObjectsLoaded");
        }

        private void SetWeaponComponentSync(object[] args)
        {
            object[] _args = Newtonsoft.Json.JsonConvert.DeserializeObject<object[]>(args[1].ToString());
            Entities.Players.GetAtRemote(Convert.ToUInt16(_args[2])).GiveWeaponComponentTo(Convert.ToUInt32(_args[0]), GetHashKey(_args[1].ToString()));
        }

        private void SetWeaponAmmo(object[] args)
        {

            uint ammoType = GetPedAmmoTypeFromWeapon(Player.LocalPlayer.Handle, CurrentWeapon);

            SetAmmoInClip(Player.LocalPlayer.Handle, CurrentWeapon, 0);
            SetPedAmmoByType(Player.LocalPlayer.Handle, (int)ammoType, 0);
            SetPedAmmoByType(Player.LocalPlayer.Handle, (int)ammoType, Convert.ToInt32(args[0]));
        }

        private void SetPedPropsSync(object[] args)
        {
            Dictionary<int, CPropData> props = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<int, CPropData>>(args[1].ToString());

            foreach (KeyValuePair<int, CPropData> entry in props)
            {
                Entities.Players.GetAtRemote(Convert.ToUInt16(args[0])).SetPropIndex(entry.Key, entry.Value.Index, entry.Value.TextureId, true);
            }
        }

        private void OnEntityStreamIn(Entity entity)
        {
            Chat.Output($"Streamed entity {entity.Type} with network id {entity.RemoteId} and local entity id: {entity.Id}");

            if (entity.Type == RAGE.Elements.Type.Player)
            {
                Events.CallRemote("GetPlayerPropsForSync", entity.RemoteId);
            }
        }

        private async void EntityCreated(Entity entity)
        {
            Chat.Output($"Created entity {entity.Type} with network id {entity.RemoteId}");

            if (entity.Type == RAGE.Elements.Type.Object)
            {
                
            }
        }

        private void AddWeaponObjectToStreamList(object[] args)
        {
            CWeaponObject obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CWeaponObject>(args[0].ToString());
            if (StreamedWeaponObjects.FirstOrDefault(x => x.Key == obj.Key) == null)
            {

                int weapObject = CreateWeaponObject(obj.Hash, 1, obj.Pos_X, obj.Pos_Y, obj.Pos_Z - 500.0f, true, 0.0f, 0, 0, 0);

                for (int i = 0; i < obj.Components.Length; i++)
                {
                    GiveWeaponComponentToWeaponObject(weapObject, GetHashKey(obj.Components[i]));
                }

                SetEntityVisible(weapObject, false, false);
                FreezeEntityPosition(weapObject, true);

                obj.Streamed = false;
                obj.EntityID = weapObject;

                StreamedWeaponObjects.Add(obj);
            } 
        }

        private int OnSecondVal = -1;
        private void OnSecond(List<Events.TickNametagData> nametags)
        {
            if(OnSecondVal == -1 || OnSecondVal != Seconds)
            {

                StreamedWeaponObjects.Where(x => x.Streamed == true).ToList().ForEach((obj) => 
                {
                    if (RAGE.Game.Utils.Vdist(Player.LocalPlayer.Position.X, Player.LocalPlayer.Position.Y, Player.LocalPlayer.Position.Z, obj.Pos_X, obj.Pos_Y, obj.Pos_Z) > 10.0f)
                    {
                        /*MarkObjectForDeletion(obj.EntityID);
                        DeleteObject(ref obj.EntityID);*/

                        SetEntityCoords(obj.EntityID, obj.Pos_X, obj.Pos_Y, obj.Pos_Z - 500.0f, false, false, false, false);
                        SetEntityVisible(obj.EntityID, false, false);
                        FreezeEntityPosition(obj.EntityID, true);

                        obj.Streamed = false;

                        Chat.Output($"Deleted wo key: {obj.Key}");
                    }
                });

                StreamedWeaponObjects.Where(x => x.Streamed == false).ToList().ForEach((obj) =>
                {
                    if (RAGE.Game.Utils.Vdist(Player.LocalPlayer.Position.X, Player.LocalPlayer.Position.Y, Player.LocalPlayer.Position.Z, obj.Pos_X, obj.Pos_Y, obj.Pos_Z) < 10.0f)
                    {
                        for (int i = 0; i < obj.Components.Length; i++)
                        {
                            GiveWeaponComponentToWeaponObject(obj.EntityID, GetHashKey(obj.Components[i]));
                        }

                        SetEntityCoords(obj.EntityID, obj.Pos_X, obj.Pos_Y, obj.Pos_Z, false, false, false, false);
                        FreezeEntityPosition(obj.EntityID, false);
                        SetEntityRotation(obj.EntityID, 90.0f, 0.0f, 0.0f, 0, true);
                        SetEntityDynamic(obj.EntityID, true);
                        SetEntityHasGravity(obj.EntityID, true);
                        SetEntityRecordsCollisions(obj.EntityID, true);
                        SetEntityVelocity(obj.EntityID, 0.0f, 0.0f, -0.2f);
                        SetEntityVisible(obj.EntityID, true, false);

                        obj.Streamed = true;

                        Chat.Output($"Streamed wo key: {obj.Key}");
                    }
                });

                OnSecondVal = Seconds;
            }
        }
        private List<uint> loadedAssets = new List<uint>();
        private bool allAssetsLoaded = false;
        private void RequestModels(List<Events.TickNametagData> nametags)
        {
            if (allAssetsLoaded) return;
            foreach (WeaponHash weapon in Enum.GetValues(typeof(WeaponHash)))
            {
                if(!HasWeaponAssetLoaded((uint)weapon))
                {
                    RequestWeaponAsset((uint)weapon, 31, 0);
                }
                else if(!loadedAssets.Contains((uint)weapon))
                {
                    loadedAssets.Add((uint)weapon);

                    if (!allAssetsLoaded && loadedAssets.Count == 94)
                    {
                        allAssetsLoaded = true;
                        Events.CallRemote("AllAssetsLoaded");
                    }
                }
            }
        }

        public static bool isInHouseEditor = false;
        private void OnTick(List<Events.TickNametagData> nametags)
        {
            Milliseconds = DateTime.Now.Millisecond;
            Seconds = DateTime.Now.Second;

            for (int i = 1; i < 23; i++)
            {
                HideHudComponentThisFrame(i);
            }

            if (ReloadEnabled && IsPedReloading(Player.LocalPlayer.Handle))
            {
                ReloadEnabled = false;
            }

            if (!ReloadEnabled && !IsPedReloading(Player.LocalPlayer.Handle))
            {
                ReloadEnabled = true;
            }

            if (CurrentWeapon != 0 && GetSelectedPedWeapon(Player.LocalPlayer.Handle) != CurrentWeapon)
            {
                ClearPedTasks(Player.LocalPlayer.Handle);
                SetCurrentPedWeapon(Player.LocalPlayer.Handle, 4208062921, true);
            }

            DisableControlAction(0, 140, true);
            EnableLaserSightRendering(true);

            if (ReloadEnabled && IsControlJustPressed(0, 45))
            {
                ReloadEnabled = false;
                Events.CallRemote("ClientReload");
            }
            RAGE.Game.Invoker.Invoke(0x3DDA37128DD1ACA8, true);
            if (IsControlJustPressed(0, 29))
            {
                isInHouseEditor = !isInHouseEditor;
                Render.Marker.Position = Vector3.Zero;
                Render.TextLabel.Position = Vector3.Zero;

                //RAGE.Game.Ui.SetRadarBigmapEnabled(true, true);
            }

            if (isInHouseEditor && IsControlJustPressed(0, 110))
            {
                if (Render.Marker.Position == Vector3.Zero) return;
                cVector3 pos = new cVector3(Render.Marker.Position.X, Render.Marker.Position.Y, Render.Marker.Position.Z);
                Events.CallRemote("AddHouseCoords", Newtonsoft.Json.JsonConvert.SerializeObject(pos));
            }
        }

        private void DropPedWeaponSync(object[] args)
        {
            object[] _args = Newtonsoft.Json.JsonConvert.DeserializeObject<object[]>(args[1].ToString());
            CWeaponObject obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CWeaponObject>(_args[0].ToString());

            int weapObject = CreateWeaponObject(obj.Hash, 1, obj.Pos_X, obj.Pos_Y, obj.Pos_Z, true, 0.0f, 0, 0, 0);

            for(int i = 0; i < obj.Components.Length; i++)
            {
                GiveWeaponComponentToWeaponObject(weapObject, GetHashKey(obj.Components[i]));
            }

            SetEntityRotation(weapObject, 90.0f, 0.0f, 0.0f, 0, true);
            SetEntityDynamic(weapObject, true);
            FreezeEntityPosition(weapObject, false);
            SetEntityHasGravity(weapObject, true);
            SetEntityRecordsCollisions(weapObject, true);

            obj.Streamed = true;
            obj.EntityID = weapObject;

            StreamedWeaponObjects.Add(obj);
        }

        private void DropPedWeapon(object[] args)
        {
            uint hash = GetSelectedPedWeapon(Player.LocalPlayer.Handle);

            List<uint> components = new List<uint>();

            foreach (uint weaponComponent in Enum.GetValues(typeof(CarbinRifleMk2Components)))
            {
                if (HasPedGotWeaponComponent(Player.LocalPlayer.Handle, GetHashKey("weapon_carbinerifle_mk2"), weaponComponent))
                {
                    components.Add(weaponComponent);
                }
            }

            int weapObject = CreateWeaponObject(hash, 1, Player.LocalPlayer.Position.X, Player.LocalPlayer.Position.Y, Player.LocalPlayer.Position.Z, true, 0.0f, 0, 0, 0);
            SetEntityVisible(weapObject, false, false);

            PlaceObjectOnGroundProperly(weapObject);
            SetEntityRotation(weapObject, 90.0f, 0.0f, 0.0f, 0, true);
            SetEntityDynamic(weapObject, true);
            FreezeEntityPosition(weapObject, false);
            SetEntityHasGravity(weapObject, true);
            SetEntityRecordsCollisions(weapObject, true);
            

            var pos = GetEntityCoords(weapObject, true);

            Events.CallRemote("UpdateWeaponObjectData", pos.X, pos.Y, pos.Z, Newtonsoft.Json.JsonConvert.SerializeObject(components));

            SetEntityCoords(weapObject, pos.X, pos.Y, pos.Z - 500.0f, false, false, false, false);
            FreezeEntityPosition(weapObject, true);
            SetEntityVisible(weapObject, false, false);

            /*MarkObjectForDeletion(weapObject);
            DeleteObject(ref weapObject);*/

            CurrentWeapon = 0;
        }

        private void SetPedCurrentWeapon(object[] args)
        {
            CurrentWeapon = GetHashKey(args[0].ToString());
            Events.CallRemote("UpdateMaxClipAmmo", GetMaxAmmoInClip(Player.LocalPlayer.Handle, CurrentWeapon, false));
        }

        private void SetWeaponComponent(object[] args)
        {
            GiveWeaponComponentToPed(Player.LocalPlayer.Handle, GetSelectedPedWeapon(Player.LocalPlayer.Handle), GetHashKey(args[0].ToString()));
            Events.CallRemote("UpdateMaxClipAmmo", GetMaxAmmoInClip(Player.LocalPlayer.Handle, CurrentWeapon, false));
        }

        private void ShowWeaponComponents(object[] args)
        {
            foreach(uint weaponComponent in Enum.GetValues(typeof(CarbinRifleMk2Components)))
            {
                if(HasPedGotWeaponComponent(Player.LocalPlayer.Handle, GetHashKey("weapon_carbinerifle_mk2"), weaponComponent))
                {
                    Chat.Output($"Got: {Enum.GetName(typeof(CarbinRifleMk2Components), weaponComponent)}");
                }

                if (IsPedWeaponComponentActive(Player.LocalPlayer.Handle, GetHashKey("weapon_carbinerifle_mk2"), weaponComponent))
                {
                    Chat.Output($"Active: {Enum.GetName(typeof(CarbinRifleMk2Components), weaponComponent)}");
                }
            }
        }

        private void SetPedProps(object[] args)
        {
            Dictionary<int, CPropData> props = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<int, CPropData>>(args[0].ToString());

            foreach (KeyValuePair<int, CPropData> entry in props)
            {
                Player.LocalPlayer.SetPropIndex(entry.Key, entry.Value.Index, entry.Value.TextureId, true);
            }
        }

        private void SetPedPropSync(object[] args)
        {
            object[] _args = Newtonsoft.Json.JsonConvert.DeserializeObject<object[]>(args[1].ToString());
            Entities.Players.GetAtRemote(Convert.ToUInt16(_args[3])).SetPropIndex(Convert.ToInt32(_args[0]), Convert.ToInt32(_args[1]), Convert.ToInt32(_args[2]), true);
        }

        private void SetPedProp(object[] args)
        {
            SetPedPropIndex(Player.LocalPlayer.Handle, Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt32(args[2]), true);
        }

        private void SetPedHairColor(object[] args)
        {
            RAGE.Game.Ped.SetPedHairColor(Player.LocalPlayer.Handle, Convert.ToInt32(args[0]), Convert.ToInt32(args[1]));
        }

        private void ClearPedProps(object[] args)
        {
            ClearAllPedProps(Player.LocalPlayer.Handle);
        }
    }
}
