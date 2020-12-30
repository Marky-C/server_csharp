using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using static ProjectServer.Main;

namespace ProjectServer.Apartments
{
    public class DelPerroHeights : Script
    {
        public static List<ColShape> ElevatorColshapes = new List<ColShape>();

        

        public static List<float> ElevatorTeleportRotations = new List<float>
        {
            -144.55682f, // гл. вход
             34.440914f, // 6 этаж
             32.401173f, // 9 этаж
             31.965734f, // 10 этаж
            -147.25546f, // парковка
             121.75966f, // чёрный вход
            -146.63094f  // крыша
        };

        public static List<Vector3> ElevatorTeleportPositions = new List<Vector3>
        {
            new Vector3(-1446.61f, -539.6775f, 34.740475f), // гл. вход
            new Vector3(-1451.06f, -524.1936f, 56.928974f), // 6 этаж
            new Vector3(-1451.12f, -524.2350f, 69.556570f), // 9 этаж
            new Vector3(-1452.86f, -539.5382f, 74.044266f), // 10 этаж
            new Vector3(-1453.75f, -565.6451f, 85.072914f), // парковка
            new Vector3(-1454.88f, -515.2513f, 31.581804f), // чёрный вход
            new Vector3(-1453.75f, -565.6451f, 85.072914f)  // крыша
        };

        public static List<Vector3> ElevatorColshapesCoords = new List<Vector3>
        {
            new Vector3(-1447.68726f, -537.384f, 33.84016f), // гл. вход
            new Vector3(-1450.07690f, -525.7575, 56.02901f), // 6 этаж
            new Vector3(-1450.07690f, -525.7575, 68.65664f), // 9 этаж
            new Vector3(-1452.18250f, -540.717f, 73.14436f), // 10 этаж
            new Vector3(-1456.54858f, -514.177f, 30.68182f), // парковка
            new Vector3(-1477.84082f, -519.534f, 33.84170f), // чёрный вход
            new Vector3(-1454.12085f, -564.039f, 84.17300f)  // крыша
        };

        public static List<string> ElevatorLevelsNames = new List<string>
        {
            "Холл",
            "6 этаж",
            "9 этаж",
            "10 этаж",
            "Парковка",
            "Чёрный выход",
            "Крыша"
        };

        public DelPerroHeights()
        {
            NAPI.Blip.CreateBlip(476, new Vector3(-1460.6864f, -535.408447f, 62.67618f), 1.0f, 4, "Жилой комплекс", 255, 50.0f, true);

            for (int i = 0; i < ElevatorColshapesCoords.Count; i++)
            {
                ElevatorColshapes.Add(NAPI.ColShape.CreateSphereColShape(ElevatorColshapesCoords[i], 1.5f));
                NAPI.Blip.CreateBlip(475, ElevatorColshapesCoords[i], 0.5f, 4, "Точка входа/выхода", 255, 50.0f, true);
            }
        }

        [ServerEvent(Event.PlayerExitColshape)]
        private void OnPlayerExitedColshape(ColShape colShape, Player player)
        {
            int idx = ElevatorColshapes.IndexOf(colShape);

            if (idx == -1) return;

            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].ElevatorType = "None";
            PlayerList[pId].ElevatorLevel = -1;

            PlayerList[pId].ExecuteModuleMutation("elevatorMenu", "toggleElevatorMenu", false);

            player.SendChatMessage($"Your level: None");
        }

        [ServerEvent(Event.PlayerEnterColshape)]
        private void OnPlayerEnteredColshape(ColShape colShape, Player player)
        {
            int idx = ElevatorColshapes.IndexOf(colShape);

            if (idx == -1) return;

            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;
            PlayerList[pId].ElevatorType = "dph_elevator_menu";
            PlayerList[pId].ElevatorLevel = idx;

            PlayerList[pId].ExecuteModuleMutation("elevatorMenu", "setElevatorCurrentLevel", ElevatorLevelsNames[idx]);
            PlayerList[pId].ExecuteModuleMutation("elevatorMenu", "setElevatorMenuElements", NAPI.Util.ToJson(ElevatorLevelsNames));
            PlayerList[pId].ExecuteModuleMutation("elevatorMenu", "toggleElevatorMenu", true);

            player.SendChatMessage($"Your level: {ElevatorLevelsNames[idx]}");
        }

        [RemoteEvent("SetElevatorChoosedLevel")]
        public void ElevatorCommand(Player player, int choosedLevel)
        {
            var pId = PlayerList.First(x => x.Value.Handle.Value == player.Handle.Value).Key;

            if(PlayerList[pId].ElevatorLevel == -1) return;
            if(PlayerList[pId].ElevatorType != "dph_elevator_menu") return;
            if (PlayerList[pId].ElevatorLevel == choosedLevel) return;

            PlayerList[pId].SetPedScreenFadeOut();

            NAPI.Task.Run(() =>
            {
                PlayerList[pId].Position = ElevatorTeleportPositions[choosedLevel];
                PlayerList[pId].Heading = ElevatorTeleportRotations[choosedLevel];
                PlayerList[pId].SetPedScreenFadeIn();
            }, 1000);
        }
    }
}
