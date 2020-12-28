using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using static ProjectServer.WeaponObject;

namespace ProjectServer
{

    public struct CPropData
    {
        public int Index;
        public int TextureId;
    }

    

    public class CPlayer : Player
    {
        public bool ActivePlayer = true;

        public uint WeaponInHands = 0;

        public int Bullets = 0;
        public int MaxClipAmmo = 0;

        public byte HairColor = 0;
        public byte HairHighlightColor = 0;
        public bool Gender = false;

        public bool AllAssetsLoaded = false;
        public bool AllWeaponObjectsLoaded = false;
        public bool Loaded = false;

        public WeaponObject CurrentWeaponObject = null;
       

        public Dictionary<int, HeadOverlay> HeadOverlays = new Dictionary<int, HeadOverlay>()
        {
            { 0, new HeadOverlay { Color = 1, SecondaryColor = 0, Index = 1, Opacity = 0.0f } },
            { 1, new HeadOverlay { Color = 1, SecondaryColor = 0, Index = 1, Opacity = 0.0f } },
            { 2, new HeadOverlay { Color = 1, SecondaryColor = 0, Index = 1, Opacity = 0.0f } },
            { 3, new HeadOverlay { Color = 1, SecondaryColor = 0, Index = 1, Opacity = 0.0f } },
            { 4, new HeadOverlay { Color = 1, SecondaryColor = 0, Index = 1, Opacity = 0.0f } },
            { 5, new HeadOverlay { Color = 1, SecondaryColor = 0, Index = 1, Opacity = 0.0f } },
            { 6, new HeadOverlay { Color = 1, SecondaryColor = 0, Index = 1, Opacity = 0.0f } },
            { 7, new HeadOverlay { Color = 1, SecondaryColor = 0, Index = 1, Opacity = 0.0f } },
            { 8, new HeadOverlay { Color = 1, SecondaryColor = 0, Index = 1, Opacity = 0.0f } },
            { 9, new HeadOverlay { Color = 1, SecondaryColor = 0, Index = 1, Opacity = 0.0f } },
            { 10, new HeadOverlay { Color = 1, SecondaryColor = 0, Index = 1, Opacity = 0.0f } },
            { 11, new HeadOverlay { Color = 1, SecondaryColor = 0, Index = 1, Opacity = 0.0f } },
            { 12, new HeadOverlay { Color = 1, SecondaryColor = 0, Index = 1, Opacity = 0.0f } }
        };

        public Dictionary<int, CPropData> Props = new Dictionary<int, CPropData>
        {
            { 0, new CPropData { Index = -1, TextureId = -1 } },
            { 1, new CPropData { Index = -1, TextureId = -1 } },
            { 2, new CPropData { Index = -1, TextureId = -1 } },
            { 6, new CPropData { Index = -1, TextureId = -1 } },
            { 7, new CPropData { Index = -1, TextureId = -1 } }
        };

        public Dictionary<int, ComponentVariation> Clothes = new Dictionary<int, ComponentVariation>
        {
            { 0, new ComponentVariation { Drawable = 65535, Texture = 0, Palette = 0 } },
            { 1, new ComponentVariation { Drawable = 65535, Texture = 0, Palette = 0 } },
            { 2, new ComponentVariation { Drawable = 65535, Texture = 0, Palette = 0 } },
            { 3, new ComponentVariation { Drawable = 65535, Texture = 0, Palette = 0 } },
            { 4, new ComponentVariation { Drawable = 65535, Texture = 0, Palette = 0 } },
            { 5, new ComponentVariation { Drawable = 65535, Texture = 0, Palette = 0 } },
            { 6, new ComponentVariation { Drawable = 65535, Texture = 0, Palette = 0 } },
            { 7, new ComponentVariation { Drawable = 65535, Texture = 0, Palette = 0 } },
            { 8, new ComponentVariation { Drawable = 65535, Texture = 0, Palette = 0 } },
            { 9, new ComponentVariation { Drawable = 65535, Texture = 0, Palette = 0 } },
            { 10, new ComponentVariation { Drawable = 65535, Texture = 0, Palette = 0 } },
            { 11, new ComponentVariation { Drawable = 65535, Texture = 0, Palette = 0 } }
        };

        public CPlayer(NetHandle netHandle) : base(netHandle)
        {

        }

        public Dictionary<int, HeadOverlay> GetHeadOverlays()
        {
            return HeadOverlays;
        }

        public void SetPedHeadOverlay(int type, HeadOverlay data)
        {
            SetHeadOverlay(type, new HeadOverlay { Color = data.Color, SecondaryColor = data.SecondaryColor, Index = data.Index, Opacity = 1.0f });
            HeadOverlays[type] = new HeadOverlay { Color = data.Color, SecondaryColor = data.SecondaryColor, Index = data.Index, Opacity = 1.0f };
        }

        public void ClearPedHeadOverlays()
        {
            HeadOverlays = new Dictionary<int, HeadOverlay>()
            {
                { 0, new HeadOverlay { Color = 1, SecondaryColor = 1, Index = 0, Opacity = 0.0f } },
                { 1, new HeadOverlay { Color = 1, SecondaryColor = 1, Index = 0, Opacity = 0.0f } },
                { 2, new HeadOverlay { Color = 1, SecondaryColor = 1, Index = 0, Opacity = 0.0f } },
                { 3, new HeadOverlay { Color = 1, SecondaryColor = 1, Index = 0, Opacity = 0.0f } },
                { 4, new HeadOverlay { Color = 1, SecondaryColor = 1, Index = 0, Opacity = 0.0f } },
                { 5, new HeadOverlay { Color = 1, SecondaryColor = 1, Index = 0, Opacity = 0.0f } },
                { 6, new HeadOverlay { Color = 1, SecondaryColor = 1, Index = 0, Opacity = 0.0f } },
                { 7, new HeadOverlay { Color = 1, SecondaryColor = 1, Index = 0, Opacity = 0.0f } },
                { 8, new HeadOverlay { Color = 1, SecondaryColor = 1, Index = 0, Opacity = 0.0f } },
                { 9, new HeadOverlay { Color = 1, SecondaryColor = 1, Index = 0, Opacity = 0.0f } },
                { 10, new HeadOverlay { Color = 1, SecondaryColor = 1, Index = 0, Opacity = 0.0f } },
                { 11, new HeadOverlay { Color = 1, SecondaryColor = 1, Index = 0, Opacity = 0.0f } },
                { 12, new HeadOverlay { Color = 1, SecondaryColor = 1, Index = 0, Opacity = 0.0f } }
            };

            for (int i = 0; i < 13; i++)
            {
                SetHeadOverlay(i, new HeadOverlay { Color = 1, SecondaryColor = 1, Index = 0, Opacity = 0.0f });
            }
        }

        public async Task LoadAllCustomization()
        {
            var cmd = new MySqlCommand($"SELECT head_blend, gender, clothes, hair_color, hair_highlight_color, head_overlays, props FROM persons WHERE social_id = {SocialClubId}", MySQL.connection);
            var reader3 = cmd.ExecuteReader();
            if (reader3.Read())
            {
                Console.WriteLine("1");
                Clothes = NAPI.Util.FromJson<Dictionary<int, ComponentVariation>>(reader3.GetString("clothes"));
                HeadOverlays = NAPI.Util.FromJson<Dictionary<int, HeadOverlay>>(reader3.GetString("head_overlays"));
                Props = NAPI.Util.FromJson<Dictionary<int, CPropData>>(reader3.GetString("props"));
                HairColor = reader3.GetByte("hair_color");
                HairHighlightColor = reader3.GetByte("hair_highlight_color");
                HeadBlend headBlend = NAPI.Util.FromJson<HeadBlend>(reader3.GetString("head_blend"));
                Gender = reader3.GetInt32("gender") == 0 ? false : true;
                Console.WriteLine("2");

                reader3.Close();

                SetCustomization(Gender, headBlend, 0, HairColor, HairHighlightColor, new float[0], HeadOverlays, new Decoration[0]);
                SetPedProps(Props);
                SetPedClothes(Clothes);
                Console.WriteLine("3");
            }
        }

        public void ExecuteModuleMutation(string module, string mutation, int payload)
        {
            TriggerEvent("CEF:ExecuteModuleMutationInt", module, mutation, payload);
        }

        public void ExecuteModuleMutation(string module, string mutation, bool payload)
        {
            TriggerEvent("CEF:ExecuteModuleMutationBool", module, mutation, payload);
        }

        public void ExecuteModuleMutation(string module, string mutation, string payload)
        {
            TriggerEvent("CEF:ExecuteModuleMutationString", module, mutation, payload);
        }

        public void ExecuteGlobalMutation(string mutation, int payload)
        {
            TriggerEvent("CEF:ExecuteGlobalMutationInt", mutation, payload);
        }

        public void ExecuteGlobalMutation(string mutation, bool payload)
        {
            TriggerEvent("CEF:ExecuteGlobalMutationBool", mutation, payload);
        }

        public void ExecuteGlobalMutation(string mutation, string payload)
        {
            TriggerEvent("CEF:ExecuteGlobalMutationString", mutation, payload);
        }

        public Dictionary<int, ComponentVariation> GetPedClothes()
        {
            return Clothes;
        }

        public Dictionary<int, CPropData> GetPedProps()
        {
            return Props;
        }

        public void SetPedProp(int type, CPropData data)
        {
            Props[type] = data;
            TriggerEvent("SetPedProp", type, data.Index, data.TextureId);
            TriggerEventToStreamed(false, "SetPedPropSync", null, type, data.Index, data.TextureId, Handle.Value);
        }

        public void ClearPedProps()
        {
            TriggerEvent("ClearPedProps");
            Props = new Dictionary<int, CPropData>
            {
                { 0, new CPropData { Index = -1, TextureId = -1 } },
                { 1, new CPropData { Index = -1, TextureId = -1 } },
                { 2, new CPropData { Index = -1, TextureId = -1 } },
                { 6, new CPropData { Index = -1, TextureId = -1 } },
                { 7, new CPropData { Index = -1, TextureId = -1 } }
            };
        }

        public void SaveClothes()
        {
            var cmd = new MySqlCommand($"UPDATE persons SET clothes = '{NAPI.Util.ToJson(GetClothes())}' WHERE id = {GetData<int>("CurrentPerson")}", MySQL.connection);
            cmd.ExecuteNonQuery();
        }

        public void SetPedProps(Dictionary<int, CPropData> data)
        {
            TriggerEvent("SetPedProps", NAPI.Util.ToJson(data));

            Props = new Dictionary<int, CPropData>
            {
                { 0, new CPropData { Index = data[0].Index, TextureId = data[0].TextureId } },
                { 1, new CPropData { Index = data[1].Index, TextureId = data[1].TextureId } },
                { 2, new CPropData { Index = data[2].Index, TextureId = data[2].TextureId } },
                { 6, new CPropData { Index = data[6].Index, TextureId = data[6].TextureId } },
                { 7, new CPropData { Index = data[7].Index, TextureId = data[7].TextureId } }
            };
        }

        public void SavePedHairColor()
        {
            var cmd = new MySqlCommand($"UPDATE persons SET hair_color = {HairColor}, hair_highlight_color = {HairHighlightColor} WHERE id = {GetData<int>("CurrentPerson")}", MySQL.connection);
            cmd.ExecuteNonQuery();
        }

        public void SavePedOverlays()
        {
            var cmd = new MySqlCommand($"UPDATE persons SET head_overlays = '{NAPI.Util.ToJson(GetHeadOverlays())}' WHERE id = {GetData<int>("CurrentPerson")}", MySQL.connection);
            cmd.ExecuteNonQuery();
        }

        public void SavePedProps()
        {
            var cmd = new MySqlCommand($"UPDATE persons SET props = '{NAPI.Util.ToJson(Props)}' WHERE id = {GetData<int>("CurrentPerson")}", MySQL.connection);
            cmd.ExecuteNonQuery();
        }

        public void SavePedFace()
        {
            var cmd = new MySqlCommand($"UPDATE persons SET head_blend = '{NAPI.Util.ToJson(HeadBlend)}' WHERE id = {GetData<int>("CurrentPerson")}", MySQL.connection);
            cmd.ExecuteNonQuery();
        }

        public void SetPedFace(byte mother, byte father, float shape)
        {
            SetCustomization(Gender, new HeadBlend { ShapeFirst = mother, ShapeSecond = father, ShapeThird = 0, SkinFirst = mother, SkinSecond = father, ShapeMix = shape, SkinMix = shape, SkinThird = 0, ThirdMix = 0.0f }, 0, HairColor, HairHighlightColor, new float[0], GetHeadOverlays(), new Decoration[0]);
        }

        public void TryReloadCurrentWeapon()
        {
            if (Bullets >= MaxClipAmmo)
            {
                Bullets -= MaxClipAmmo;

                TriggerEvent("SetWeaponAmmo", MaxClipAmmo);
            }
        }

        public void GivePedWeapon()
        {
            GiveWeapon(WeaponHash.Carbinerifle_mk2, 0);

            WeaponInHands = NAPI.Util.GetHashKey("weapon_carbinerifle_mk2");

            TriggerEvent("SetPedCurrentWeapon", "weapon_carbinerifle_mk2");
        }

        public async void DropPedWeapon()
        {
            TriggerEvent("DropPedWeapon");

            int waiter = 0;
            while(CurrentWeaponObject == null)
            {
                await NAPI.Task.WaitForMainThread(100);
                waiter += 100;
                if(waiter == 5000)
                {
                    Console.WriteLine("too long awaiting");
                    return;
                }
            }

            RemoveWeapon(CurrentWeapon);

            var cmd = new MySqlCommand($"INSERT INTO weapon_objects(wkey, whash, ammo, components, pos_x, pos_y, pos_z) VALUES ('{CurrentWeaponObject.Key}', {CurrentWeaponObject.Hash}, {CurrentWeaponObject.Ammo}, '{NAPI.Util.ToJson(CurrentWeaponObject.Components)}', {CurrentWeaponObject.Pos_X.ToString().Replace(',', '.')}, {CurrentWeaponObject.Pos_Y.ToString().Replace(',', '.')}, {CurrentWeaponObject.Pos_Z.ToString().Replace(',', '.')});", MySQL.connection);
            cmd.ExecuteNonQuery();

            WeaponObjectsList.Add(CurrentWeaponObject);
            TriggerEventToStreamed(true, "DropPedWeaponSync", null, NAPI.Util.ToJson(CurrentWeaponObject));
            NAPI.ClientEvent.TriggerClientEventForAll("AddWeaponObjectToStreamList", NAPI.Util.ToJson(CurrentWeaponObject));

            CurrentWeaponObject = null;
            WeaponInHands = 0;
        }

        public async Task LoadWeapons()
        {
            if (WeaponObjectsList.Count > 0)
            {
                TriggerEvent("AddWeaponObjectsToStreamList", NAPI.Util.ToJson(WeaponObjectsList));

                while (!AllWeaponObjectsLoaded)
                {
                    await NAPI.Task.WaitForMainThread(100);
                }
            }
        }

        public void GivePedWeaponAmmo(int ammo)
        {
            Bullets += ammo;
        }

        public void SetPedWeaponComponent(string name)
        {
            TriggerEvent("SetWeaponComponent", name);
            TriggerEventToStreamed(false, "SetWeaponComponentSync", null, WeaponInHands, name, Handle.Value);
        }

        public void SetPedHairColor(byte color1, byte color2)
        {
            HairColor = color1;
            HairHighlightColor = color2;

            TriggerEvent("SetPedHairColor", color1, color2);
        }

        public void SetPedClothes(Dictionary<int, ComponentVariation> data)
        {
            SetClothes(data);
            Clothes = data;
        }

        public void SetPedCloth(int type, ComponentVariation data)
        {
            SetClothes(type, data.Drawable, data.Texture);
            Clothes[type] = data;
        }

        public Dictionary<int, ComponentVariation> GetClothes()
        {
            return Clothes;
        }
    }
}
