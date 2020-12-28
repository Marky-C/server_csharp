using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using System.Linq;
using MySql.Data.MySqlClient;
using static ProjectServer.MySQL;

namespace ProjectServer
{
    class HouseManager : Script
    {
        public static List<House> HousesList = new List<House>();
        public HouseManager()
        {

        }

        [RemoteEvent("AddHouseCoords")]
        private void OnAddHouseCoords(Player player, string json)
        {
            var cmd = new MySqlCommand($"INSERT INTO houses(position) VALUES ('{json}');", connection);
            cmd.ExecuteNonQuery();

            cVector3 pos = NAPI.Util.FromJson<cVector3>(json);
            HousesList.Add(new House()
            {
                Position = pos,
                MapBlip = NAPI.Blip.CreateBlip(374, new Vector3(pos.X, pos.Y, pos.Z), 0.5f, 25, "Частный дом", 255, 0f, true),
                Marker = NAPI.Marker.CreateMarker(1, new Vector3(pos.X, pos.Y, pos.Z), new Vector3(0f, 0f, 0f), new Vector3(180f, 0f, 0f), 1f, new Color(0, 255, 0, 150)),
                TextLabel = NAPI.TextLabel.CreateTextLabel("Дом №231", new Vector3(pos.X, pos.Y, pos.Z + 1.0f), 5.0f, 1.0f, 1, new Color(255, 255, 255, 255))
            });
        }

        public static void LoadHouses()
        {
            var cmd = new MySqlCommand($"SELECT * FROM houses;", MySQL.connection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cVector3 pos = NAPI.Util.FromJson<cVector3>(reader.GetString("position"));
                HousesList.Add(new House()
                {
                    Position = pos,
                    MapBlip = NAPI.Blip.CreateBlip(374, new Vector3(pos.X, pos.Y, pos.Z), 0.5f, 25, "Частный дом", 255, 0f, true),
                    Marker = NAPI.Marker.CreateMarker(1, new Vector3(pos.X, pos.Y, pos.Z), new Vector3(0f, 0f, 0f), new Vector3(180f, 0f, 0f), 1f, new Color(0, 255, 0, 150)),
                    TextLabel = NAPI.TextLabel.CreateTextLabel($"Дом №{reader.GetInt32("id")}", new Vector3(pos.X, pos.Y, pos.Z + 1.0f), 5.0f, 1.0f, 1, new Color(255, 255, 255, 255))
                });
            }
            reader.Close();
            Console.WriteLine($"Загружено {HousesList.Count} Houses.");
        }
    }
}
