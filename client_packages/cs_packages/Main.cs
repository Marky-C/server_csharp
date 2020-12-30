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
using static RAGE.Game.Cam;
using static RAGE.Game.Streaming;
using static RAGE.Game.Ui;
using static RAGE.Game.Misc;
using static RAGE.Game.Object;
using static ProjectClient.Enums;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;

namespace ProjectClient
{
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
            Chat.Output(GetHashKey("hei_v_ilev_bk_gate_pris").ToString());

            Events.CallRemote("ClientLoaded");

            RAGE.Game.Streaming.RequestModel(GetHashKey("w_at_ar_supp"));

            Events.OnEntityCreated += EntityCreated;
            Events.OnEntityStreamIn += OnEntityStreamIn;

            Nametags.Enabled = false;

            Events.Tick += OnTick;
            Events.Tick += UpdateTime;
            Events.Tick += OnSecond;
            

            Events.Add("PlayerQuit", OnPlayerQuit);

            Events.Add("ClearPedProps", ClearPedProps);
            Events.Add("SetPedProp", SetPedProp);
            Events.Add("SetPedPropSync", SetPedPropSync);
            Events.Add("SetPedProps", SetPedProps);
            Events.Add("SetPedPropsSync", SetPedPropsSync);

            Events.Add("ShowWeaponComponents", ShowWeaponComponents);
            Events.Add("SetWeaponComponent", SetWeaponComponent);
            Events.Add("SetWeaponComponentSync", SetWeaponComponentSync);

            Events.Add("SetPedScreenFadeOut", SetPedScreenFadeOut);
            Events.Add("SetPedScreenFadeIn", SetPedScreenFadeIn);

            Events.Add("SetWeaponAmmo", SetWeaponAmmo);

            Events.Add("SetPedCurrentWeapon", SetPedCurrentWeapon);

            Events.Add("DropPedWeapon", DropPedWeapon);
            Events.Add("DropPedWeaponSync", DropPedWeaponSync);
            Events.Add("AddWeaponObjectToStreamList", AddWeaponObjectToStreamList);
            Events.Add("AddWeaponObjectsToStreamList", AddWeaponObjectsToStreamList);
            Events.Add("ClearWeaponObjectList", ClearWeaponObjectList);

            Events.Add("SetPedHairColor", SetPedHairColor);
            
        }

        private void SetPedScreenFadeIn(object[] args)
        {
            DoScreenFadeIn(200);
            FreezeEntityPosition(Player.LocalPlayer.Handle, false);
        }

        private void SetPedScreenFadeOut(object[] args)
        {
            DoScreenFadeOut(200);
            FreezeEntityPosition(Player.LocalPlayer.Handle, true);
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
        }

        private void EntityCreated(Entity entity)
        {
            /*Chat.Output($"Created entity {entity.Type} with network id {entity.RemoteId}");*/
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

        private long timeStamp = -1;
        private long _timeStamp = -1;
        private void UpdateTime(List<Events.TickNametagData> nametags)
        {
            Milliseconds = DateTime.Now.Millisecond;
            Seconds = DateTime.Now.Second;
            timeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();



            if (_timeStamp + 250 < timeStamp)
            {
                _timeStamp = timeStamp;
                Entities.Blips.All.ForEach((blip) =>
                {
                    if (blip.GetSprite() == 475)
                    {
                        Vector3 coords_blip = blip.GetCoords();
                        if (RAGE.Game.Utils.Vdist(Player.LocalPlayer.Position.X, Player.LocalPlayer.Position.Y, Player.LocalPlayer.Position.Z, coords_blip.X, coords_blip.Y, coords_blip.Z) > 10.0f)
                        {
                            blip.SetDisplay(0);
                        }
                        else
                        {
                            blip.SetDisplay(5);
                        }
                    }

                    else if (blip.GetSprite() == 476)
                    {
                        Vector3 coords_blip = blip.GetCoords();
                        if (RAGE.Game.Utils.Vdist(Player.LocalPlayer.Position.X, Player.LocalPlayer.Position.Y, Player.LocalPlayer.Position.Z, coords_blip.X, coords_blip.Y, coords_blip.Z) < 40.0f)
                        {
                            blip.SetDisplay(3);
                        }
                        else
                        {
                            blip.SetDisplay(2);
                        }
                    }
                });
            }
        }

        public static bool isInHouseEditor = false;
        public static bool isInNoClip = false;
        public static int NoClipCamera = -1;
        public static float NoClipCameraSpeed = 0.1f;
        public static Vector3 NoClipCameraPos = new Vector3();
        private void OnTick(List<Events.TickNametagData> nametags)
        {
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

            DisableControlAction(0, 140, true);

            if (ReloadEnabled && IsControlJustPressed(0, 45))
            {
                ReloadEnabled = false;
                Events.CallRemote("ClientReload");
            }
            
            if (isInNoClip && IsControlJustPressed(0, 29))
            {
                isInHouseEditor = !isInHouseEditor;
                Render.Marker.Position = Vector3.Zero;
                Render.TextLabel.Position = Vector3.Zero;
            }

            
            if (IsControlJustPressed(0, 249))
            {
                isInNoClip = !isInNoClip;

                if(isInNoClip)
                {
                    SetEntityVisible(Player.LocalPlayer.Handle, false, false);
                    NoClipCamera = CreateCam("DEFAULT_SCRIPTED_CAMERA", true);
                    NoClipCameraPos = Player.LocalPlayer.Position;
                    SetCamCoord(NoClipCamera, NoClipCameraPos.X, NoClipCameraPos.Y, NoClipCameraPos.Z);
                    SetCamActive(NoClipCamera, true);
                    RenderScriptCams(true, false, 0, true, true, 0);
                }
                else
                {
                    SetEntityVisible(Player.LocalPlayer.Handle, true, false);
                    SetCamActive(NoClipCamera, false);
                    DestroyCam(NoClipCamera, false);
                    RenderScriptCams(false, false, 0, true, true, 0);
                }
            }

            if (isInHouseEditor && IsControlJustPressed(0, 24))
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
