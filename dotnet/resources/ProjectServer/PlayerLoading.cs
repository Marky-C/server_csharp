using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using static ProjectServer.Main;
using System.Diagnostics;

namespace ProjectServer
{
    class PlayerLoading : Script
    {
        public PlayerLoading()
        {

        }

        [RemoteEvent("AllAssetsLoaded")]
        public void OnAllAssetsLoaded(Player player)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].AllAssetsLoaded = true;
        }

        [RemoteEvent("AllWeaponObjectsLoaded")]
        public void OnAllWeaponObjectsLoaded(Player player)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;

            PlayerList[pId].AllWeaponObjectsLoaded = true;
        }

        [RemoteEvent("ClientLoaded")]
        public void OnClientLoaded(Player player)
        {
            if (PlayerList.Any(x => x.Value == null))
            {
                PlayerList[PlayerList.First(x => x.Value == null).Key] = new CPlayer(player.Handle);
            }
            else
            {
                PlayerList.Add(PlayerList.Count, new CPlayer(player.Handle));
            }

            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;

            PlayerList[pId].ExecuteGlobalMutation("setVersion", buildVersion);
            PlayerList[pId].ExecuteModuleMutation("loading", "toggleLoading", true);
        }

        [ServerEvent(Event.PlayerSpawn)]
        public async void OnPlayerSpawn(Player player)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;

            if(!PlayerList[pId].Loaded) 
            {
                try
                {
                    var personId = -1;
                    var cmd = new MySqlCommand($"SELECT social_id FROM accounts WHERE social_id = {player.SocialClubId}", MySQL.connection);
                    var reader1 = cmd.ExecuteReader();
                    if (!reader1.Read())
                    {
                        reader1.Close();

                        cmd = new MySqlCommand($"INSERT INTO persons(name, head_blend, gender, social_id, clothes, head_overlays, props) VALUES ('{player.Name}', '{NAPI.Util.ToJson(player.HeadBlend)}', true, {player.SocialClubId}, '{NAPI.Util.ToJson(PlayerList[pId].GetClothes())}', '{NAPI.Util.ToJson(PlayerList[pId].GetHeadOverlays())}', '{NAPI.Util.ToJson(PlayerList[pId].GetPedProps())}');", MySQL.connection);
                        cmd.ExecuteNonQuery();

                        cmd = new MySqlCommand($"SELECT id FROM persons WHERE social_id = {player.SocialClubId}", MySQL.connection);
                        var reader2 = cmd.ExecuteReader();
                        if (reader2.Read())
                        {
                            personId = reader2.GetInt32("id");
                            PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Value.SetData("CurrentPerson", personId);
                            reader2.Close();
                        }

                        cmd = new MySqlCommand($"INSERT INTO accounts(social_id, person_1) VALUES ({player.SocialClubId}, {personId});", MySQL.connection);
                        cmd.ExecuteNonQuery();
                        player.SendChatMessage("You now was registered");
                    }
                    else
                    {
                        reader1.Close();

                        cmd = new MySqlCommand($"SELECT id FROM persons WHERE social_id = {player.SocialClubId}", MySQL.connection);
                        var reader3 = cmd.ExecuteReader();
                        if (reader3.Read())
                        {
                            personId = reader3.GetInt32("id");
                            PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Value.SetData("CurrentPerson", personId);
                            reader3.Close();
                        }
                        Stopwatch sw = Stopwatch.StartNew();
                        long totalSw = 0;

                        PlayerList[pId].ExecuteModuleMutation("loading", "setLoadingStep", "Загрузка моделей оружия..");
                        player.TriggerEvent("StartPlayerLoading");
                        while(!PlayerList[pId].AllAssetsLoaded)
                        {
                            await NAPI.Task.WaitForMainThread(100);
                        }
                        sw.Stop();
                        totalSw += sw.ElapsedMilliseconds;
                        PlayerList[pId].ExecuteModuleMutation("loading", "addLoadingHistory", $"Модели оружия загружены за {(sw.ElapsedMilliseconds / 1000.0f).ToString("N3")}с.");

                        PlayerList[pId].ExecuteModuleMutation("loading", "setLoadingStep", "Загрузка кастомизаци..");
                        sw = Stopwatch.StartNew();
                        await PlayerList[pId].LoadAllCustomization();
                        sw.Stop();
                        totalSw += sw.ElapsedMilliseconds;
                        PlayerList[pId].ExecuteModuleMutation("loading", "addLoadingHistory", $"Кастомизация загружена за {(sw.ElapsedMilliseconds / 1000.0f).ToString("N3")}с.");

                        PlayerList[pId].ExecuteModuleMutation("loading", "setLoadingStep", "Загрузка оружия..");
                        sw = Stopwatch.StartNew();
                        await PlayerList[pId].LoadWeapons();
                        sw.Stop();
                        totalSw += sw.ElapsedMilliseconds;
                        PlayerList[pId].ExecuteModuleMutation("loading", "addLoadingHistory", $"Оружие загружено за {(sw.ElapsedMilliseconds / 1000.0f).ToString("N3")}с.");
                        PlayerList[pId].ExecuteModuleMutation("loading", "addLoadingHistory", $"Суммарное время загрузки - {(totalSw / 1000.0f).ToString("N3")}с.");

                        PlayerList[pId].ExecuteModuleMutation("loading", "setLoadingStep", "Завершаем загрузку..");

                        PlayerList[pId].Loaded = true;

                        PlayerList[pId].ExecuteModuleMutation("loading", "toggleLoading", false);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}\n{e.StackTrace}");
                }
            }
        }
    }
}
