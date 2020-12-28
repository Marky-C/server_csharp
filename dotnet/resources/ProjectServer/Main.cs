using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace ProjectServer
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

    public class Main : Script
    {
        public static string buildVersion;
        public Main()
        {
            SecondTimer();

            buildVersion = $"0.0.{DateTime.Now.Day.ToString("D2")}{DateTime.Now.Hour.ToString("D2")}{DateTime.Now.Minute.ToString("D2")}";

            Console.WriteLine($"build {buildVersion}");
        }

        public static Dictionary<int, CPlayer> PlayerList = new Dictionary<int, CPlayer>();

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnect(Player player, DisconnectionType type, string reason)
        {
            try
            {
                if (PlayerList.Any(x => x.Value.Handle.Value == player.Handle.Value))
                {
                    PlayerList[PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key] = null;
                }
            }
            catch { }
        }

        public async void SecondTimer()
        {

        }
        

        [ServerEvent(Event.ResourceStart)]
        public async void OnResourceStart()
        {
            if(MySQL.Initialize())
            {
                WeaponObject.LoadWeaponObjects();
                HouseManager.LoadHouses();
            }
        }
        

        [RemoteEvent("GetPlayerPropsForSync")]
        public void GetPlayerPropsForSync(Player player, int netid)
        {
            var rId = PlayerList.First(x => x.Value.Handle.Value == netid).Key;
            Console.WriteLine($"{player.Name} requested props from {PlayerList[rId].Name}");
            Console.WriteLine($"Sending to {player.Name} - {NAPI.Util.ToJson(PlayerList[rId].Props)}");
            player.TriggerEvent("SetPedPropsSync", netid, NAPI.Util.ToJson(PlayerList[rId].Props));
        }

        [RemoteEvent("UpdateMaxClipAmmo")]
        public void UpdateMaxClipAmmo(Player player, int max)
        {
            Console.WriteLine("UpdateMaxClipAmmo " + max);
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].MaxClipAmmo = max;
        }

        [RemoteEvent("UpdateWeaponObjectData")]
        public void UpdateWeaponObjectData(Player player, float x, float y, float z, string components)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            string key = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds().ToString();
            PlayerList[pId].CurrentWeaponObject = new WeaponObject() { Key = key, Hash = (uint)PlayerList[pId].CurrentWeapon, Ammo = 0, Pos_X = x, Pos_Y = y, Pos_Z = z, Components = NAPI.Util.FromJson<string[]>(components) };
        }

        [RemoteEvent("ClientReload")]
        public void OnClientReload(Player player)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].TryReloadCurrentWeapon();
        }

        [Command("getobjects")]
        public void GetObjectsCommand(Player player)
        {
            player.TriggerEvent("GetObjects");
        }

        [Command("setweaponc")]
        public void SetWeaponComponentCommand(Player player, string name)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].SetPedWeaponComponent(name);
        }

        [Command("weaponcomponents")]
        public void WeaponComponentsCommand(Player player)
        {
            player.TriggerEvent("ShowWeaponComponents");
        }
        [Command("objreload")]
        public void ObjectReloadCommand(Player player)
        {
            player.TriggerEvent("ClearWeaponObjectList");
        }

        [Command("veh")]
        public void VehCommand(Player player, string model, int color)
        {
            var veh = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(model), player.Position, player.Heading, color, color);

            player.SetIntoVehicle(veh.Handle, 0);
        }
        [Command("saveprops")]
        public void SavePropsCommand(Player player)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].SavePedProps();
        }

        [Command("props")]
        public void PropsCommand(Player player, int type, int id, int texture_id)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].SetPedProp(type, new CPropData { Index = id, TextureId = texture_id });
        }
        
        [Command("cprops")]
        public void ClearPropsCommand(Player player)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].ClearPedProps();
        }

        [Command("weaponammo")]
        public void WeaponCommand(Player player, int ammo)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].GivePedWeaponAmmo(ammo);
        }

        [Command("dropweapon")]
        public void DropWeaponCommand(Player player)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].DropPedWeapon();
        }

        [Command("weapon")]
        public void WeaponCommand(Player player)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].GivePedWeapon();
        }

        [Command("saveclothes")]
        public void SaveClothesCommand(Player player)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].SaveClothes();
        }

        [Command("clothes")]
        public void ClothesCommand(Player player, int type, int id, int texture_id)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].SetPedCloth(type, new ComponentVariation { Drawable = id, Texture = texture_id, Palette = 0 });
        }

        [Command("overlays")]
        public void OverlaysCommand(Player player, int id, byte color, byte secondaryColor, byte index)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].SetPedHeadOverlay(id, new HeadOverlay { Color = color, SecondaryColor = secondaryColor, Index = index, Opacity = 1.0f });
        }

        [Command("clearoverlays")]
        public void ClearOverlaysCommand(Player player)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].ClearPedHeadOverlays();
        }

        [Command("saveoverlays")]
        public void SaveOverlaysCommand(Player player)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].SavePedOverlays();
        }

        [Command("savehaircolor")]
        public void SaveHairColorCommand(Player player)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].SavePedHairColor();
        }

        [Command("haircolor")]
        public void HairColorCommand(Player player, byte color1, byte color2)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].SetPedHairColor(color1, color2);
        }

        [Command("saveface")]
        public void SaveFaceCommand(Player player)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].SavePedFace();
        }

        [RemoteEvent("PlayerReadyToQuit")]
        public void OnPlayerReadyToQuit(Player player)
        {
            /*var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList.Remove(pId);*/
            
            player.KickSilent();
        }

        [Command("q")]
        public void QuitCommand(Player player)
        {
            player.TriggerEvent("PlayerQuit");
        }

        [Command("face")]
        public void FaceCommand(Player player, byte mother, byte father, float shape)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].SetPedFace(mother, father, shape);
        }
    }
}

    
