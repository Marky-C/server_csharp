using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using GTANetworkAPI;

namespace ProjectServer
{
    public class WeaponObject
    {
        public static List<WeaponObject> WeaponObjectsList = new List<WeaponObject>();

        public string Key;
        public uint Hash;
        public int Ammo;
        public string[] Components;
        public float Pos_X;
        public float Pos_Y;
        public float Pos_Z;

        public static void LoadWeaponObjects()
        {
            var cmd = new MySqlCommand($"SELECT * FROM weapon_objects;", MySQL.connection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                WeaponObjectsList.Add(new WeaponObject()
                {
                    Key = reader.GetString("wkey"),
                    Hash = reader.GetUInt32("whash"),
                    Ammo = reader.GetInt32("ammo"),
                    Components = NAPI.Util.FromJson<string[]>(reader.GetString("components")),
                    Pos_X = reader.GetFloat("pos_x"),
                    Pos_Y = reader.GetFloat("pos_y"),
                    Pos_Z = reader.GetFloat("pos_z")
                });
            }
            reader.Close();
            Console.WriteLine($"Загружено {WeaponObjectsList.Count} Weapon Objects.");
        }
    }
}
