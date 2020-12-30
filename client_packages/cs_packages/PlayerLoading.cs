using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using static RAGE.Game.Ped;
using static RAGE.Game.Entity;
using static RAGE.Game.Player;
using static RAGE.Game.Ai;
using static RAGE.Game.Pad;
using static RAGE.Game.Weapon;
using static RAGE.Game.Streaming;
using static RAGE.Game.Ui;
using static RAGE.Game.Misc;
using static RAGE.Game.Object;
using static RAGE.Elements.Player;
using static ProjectClient.Enums;

namespace ProjectClient
{
    class PlayerLoading : Events.Script
    {
        public PlayerLoading()
        {
            Events.Tick += RequestModels;

            Events.Add("StartPlayerLoading", (object[] args) => { AllAssetsLoaded = false; });

            SetPedCanSwitchWeapon(LocalPlayer.Handle, false);
        }
        
        private List<uint> LoadedWeaponAssets = new List<uint>();
        private List<uint> LoadedWeaponComponentsAssets = new List<uint>();

        private List<uint> RequestedWeaponAssets = new List<uint>();
        private List<uint> RequestedWeaponComponentsAssets = new List<uint>();

        private bool AllAssetsLoaded = true;

        private void RequestModels(List<Events.TickNametagData> nametags)
        {
            if (DateTime.Now.Millisecond % 10 == 0)
            {
                if (AllAssetsLoaded) return;

                if (LoadedWeaponAssets.Count != Enum.GetValues(typeof(WeaponHash)).Length)
                {
                    int idx = -1;
                    foreach (WeaponHash weapon in Enum.GetValues(typeof(WeaponHash)))
                    {
                        idx++;
                        if (!HasWeaponAssetLoaded((uint)weapon))
                        {
                            if (RequestedWeaponAssets.Contains((uint)weapon)) return;

                            RequestWeaponAsset((uint)weapon, 31, 0);
                            RequestedWeaponAssets.Add((uint)weapon);
                            CEF.ExecuteModuleMutation("loading", "setLoadingStep", $"Загрузка моделей оружия.. [{idx}]");
                        }
                        else if (!LoadedWeaponAssets.Contains((uint)weapon))
                        {
                            LoadedWeaponAssets.Add((uint)weapon);
                        }
                    }
                }

                if (LoadedWeaponAssets.Count != Enum.GetValues(typeof(WeaponHash)).Length) return;

                if (LoadedWeaponComponentsAssets.Count != ObjectsWeaponComponents.Length)
                {
                    for (int i = 0; i < ObjectsWeaponComponents.Length; i++)
                    {
                        if (!HasModelLoaded(ObjectsWeaponComponents[i]))
                        {
                            if (RequestedWeaponComponentsAssets.Contains(ObjectsWeaponComponents[i])) return;

                            RequestModel(ObjectsWeaponComponents[i]);
                            CEF.ExecuteModuleMutation("loading", "setLoadingStep", $"Загрузка моделей компонентов оружия.. [{LoadedWeaponComponentsAssets.Count}/{ObjectsWeaponComponents.Length}] ({i})");
                            RequestedWeaponComponentsAssets.Add(ObjectsWeaponComponents[i]);
                        }
                        else if (!LoadedWeaponComponentsAssets.Contains(ObjectsWeaponComponents[i]))
                        {
                            LoadedWeaponComponentsAssets.Add(ObjectsWeaponComponents[i]);
                        }
                    }
                }

                if (LoadedWeaponComponentsAssets.Count == ObjectsWeaponComponents.Length)
                {
                    AllAssetsLoaded = true;
                    RequestedWeaponAssets = null;
                    RequestedWeaponComponentsAssets = null;
                    Events.CallRemote("AllAssetsLoaded");
                }
            }
        }
    }
}
