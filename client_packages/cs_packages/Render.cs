using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using System.Linq;
using static ProjectClient.Main;
using static RAGE.Game.Ui;
using static RAGE.Game.Graphics;
using static RAGE.Game.Cam;
using static RAGE.Game.Shapetest;
using static RAGE.Elements.Player;
using static RAGE.Game.Object;
using static RAGE.Game.Ped;
using static RAGE.Game.Pad;
using static RAGE.Game.Entity;
using static RAGE.Game.Network;

namespace ProjectClient
{
    class Render : Events.Script
    {
        public static RAGE.Elements.Marker Marker = new RAGE.Elements.Marker(1, Vector3.Zero, 1f, new Vector3(180f, 0f, 0f), Vector3.Zero, new RGBA(0, 255, 0, 100));
        public static RAGE.Elements.TextLabel TextLabel = new RAGE.Elements.TextLabel(Vector3.Zero, "Дом №131", new RGBA(255, 255, 255, 255), 300f, 0, true);
        public Render()
        {
            Events.Tick += RenderEvent;
            Events.Tick += HouseEditor;
            Events.Tick += NoClip;
        }

        private static bool closed = false;
        private void NoClip(List<Events.TickNametagData> nametags)
        {
            int door = GetClosestObjectOfType(-1455.769f, -534.4214f, 74.19384f, 5.0f, 34120519, false, false, false);
            Vector3 coords = GetEntityCoords(door, false);
            DrawLine(LocalPlayer.Position.X, LocalPlayer.Position.Y, LocalPlayer.Position.Z, coords.X, coords.Y, coords.Z, 255, 0, 0, 255);

            if(!closed)
            {
                NetworkRequestControlOfEntity(door);
                FreezeEntityPosition(door, false);
            }
            else
            {
                int _locked = -1;
                float heading = 0.0f;
                GetStateOfClosestDoorOfType(34120519, -1455.769f, -534.4214f, 74.19384f, ref _locked, ref heading);
                if(heading > -0.02 && heading < 0.02)
                {
                    NetworkRequestControlOfEntity(door);
                    FreezeEntityPosition(door, true);
                }
            }
            

            if (IsControlJustPressed(0, 182))
            {
                closed = !closed;
            }
            
            if (isInNoClip)
            {
                float degree = GetGameplayCamRot(0).Z;
                float pitch = GetGameplayCamRelativePitch();

                double yForward = Math.Cos(Convert.ToDouble(degree * Math.PI / 180));
                double xForward = (Math.Sin(Convert.ToDouble(degree * Math.PI / 180)) * -1);
                double zForward = (Math.Sin(Convert.ToDouble(pitch * Math.PI / 180)));

                Vector3 camForward = new Vector3((float)xForward, (float)yForward, (float)zForward);

                degree = GetGameplayCamRot(0).Z + 90.0f;
                pitch = GetGameplayCamRelativePitch();

                yForward = Math.Cos(Convert.ToDouble(degree * Math.PI / 180));
                xForward = (Math.Sin(Convert.ToDouble(degree * Math.PI / 180)) * -1);
                zForward = (Math.Sin(Convert.ToDouble(pitch * Math.PI / 180)));

                Vector3 camSideForward = new Vector3((float)xForward, (float)yForward, (float)zForward);

                Vector3 gamePlayCamRot = GetGameplayCamRot(0);
                Vector3 noClipCamCoords = GetCamCoord(NoClipCamera);

                SetCamRot(NoClipCamera, gamePlayCamRot.X, gamePlayCamRot.Y, gamePlayCamRot.Z, 0);
                SetEntityCoords(LocalPlayer.Handle, noClipCamCoords.X, noClipCamCoords.Y, noClipCamCoords.Z, false, false, false, false);
                SetEntityHeading(LocalPlayer.Handle, gamePlayCamRot.Z);

                DisableControlAction(0, 261, true);
                DisableControlAction(0, 262, true);
                DisableControlAction(0, 14, true);
                DisableControlAction(0, 15, true);
                DisableControlAction(0, 16, true);
                DisableControlAction(0, 17, true);

                if (IsDisabledControlJustPressed(0, 261)) // WheelUp
                {
                    if (NoClipCameraSpeed <= 5.0f)
                    {
                        NoClipCameraSpeed += NoClipCameraSpeed / 1.5f;
                    }
                }

                if (IsDisabledControlJustPressed(0, 262)) // WheelDown
                {
                    if (NoClipCameraSpeed >= 0.1f)
                    {
                        NoClipCameraSpeed -= NoClipCameraSpeed / 1.5f;
                    }
                }

                if (IsControlPressed(0, 232)) // W
                {
                    NoClipCameraPos = new Vector3(NoClipCameraPos.X + camForward.X * NoClipCameraSpeed, NoClipCameraPos.Y + camForward.Y * NoClipCameraSpeed, NoClipCameraPos.Z + camForward.Z * NoClipCameraSpeed);
                    SetCamCoord(NoClipCamera, NoClipCameraPos.X, NoClipCameraPos.Y, NoClipCameraPos.Z);
                }

                if (IsControlPressed(0, 234)) // A
                {
                    NoClipCameraPos = new Vector3(NoClipCameraPos.X + camSideForward.X * NoClipCameraSpeed, NoClipCameraPos.Y + camSideForward.Y * NoClipCameraSpeed, NoClipCameraPos.Z);
                    SetCamCoord(NoClipCamera, NoClipCameraPos.X, NoClipCameraPos.Y, NoClipCameraPos.Z);
                }

                if (IsControlPressed(0, 233)) // S
                {
                    NoClipCameraPos = new Vector3(NoClipCameraPos.X - camForward.X * NoClipCameraSpeed, NoClipCameraPos.Y - camForward.Y * NoClipCameraSpeed, NoClipCameraPos.Z - camForward.Z * NoClipCameraSpeed);
                    SetCamCoord(NoClipCamera, NoClipCameraPos.X, NoClipCameraPos.Y, NoClipCameraPos.Z);
                }

                if (IsControlPressed(0, 235)) // D
                {
                    NoClipCameraPos = new Vector3(NoClipCameraPos.X - camSideForward.X * NoClipCameraSpeed, NoClipCameraPos.Y - camSideForward.Y * NoClipCameraSpeed, NoClipCameraPos.Z);
                    SetCamCoord(NoClipCamera, NoClipCameraPos.X, NoClipCameraPos.Y, NoClipCameraPos.Z);
                }
            }
        }

        private void HouseEditor(List<Events.TickNametagData> nametags)
        {
            if(isInHouseEditor)
            {
                float degree = GetGameplayCamRot(0).Z;
                float pitch = GetGameplayCamRelativePitch();
                float distance = 20f;
                double yForward = Math.Cos(Convert.ToDouble(degree * Math.PI / 180));
                double xForward = (Math.Sin(Convert.ToDouble(degree * Math.PI / 180)) * -1);
                double zForward = (Math.Sin(Convert.ToDouble(pitch * Math.PI / 180)));

                Vector3 camForward = new Vector3((float)xForward, (float)yForward, (float)zForward);

                var camPos = GetGameplayCamCoords();
                float zFix = 1f;
                if (pitch <= -20)
                {
                    zFix = pitch * -1 / 25;
                }
                int raycast = StartShapeTestRay(camPos.X, camPos.Y, camPos.Z, camPos.X + camForward.X * distance, camPos.Y + camForward.Y * distance, camPos.Z + camForward.Z * distance * zFix, -1, LocalPlayer.Handle, 0);
                int hit = -1;
                int entityHit = -1;
                Vector3 endCoords = new Vector3();
                Vector3 surfaceNormal = new Vector3();
                GetShapeTestResult(raycast, ref hit, endCoords, surfaceNormal, ref entityHit);
                endCoords.Z += 0.1f;
                if (hit != -1)
                {
                    Marker.Position = endCoords;
                    endCoords.Z += 1f;
                    TextLabel.Position = endCoords;
                }
                else
                {
                    TextLabel.Position = Vector3.Zero;
                    Marker.Position = Vector3.Zero;
                }
            }
        }
        
        private void RenderEvent(List<Events.TickNametagData> nametags)
        {
            StreamedWeaponObjects.Where(x => x.Streamed).ToList().ForEach((obj) =>
            {
                float screenX = 0.0f;
                float screenY = 0.0f;

                if(RAGE.Game.Graphics.GetScreenCoordFromWorldCoord(obj.Pos_X, obj.Pos_Y, obj.Pos_Z, ref screenX, ref screenY))
                {
                    SetDrawOrigin(obj.Pos_X, obj.Pos_Y, obj.Pos_Z + 0.5f, 0);

                    var camPos = GetGameplayCamCoords();
                    int raycast = StartShapeTestRay(camPos.X, camPos.Y, camPos.Z, obj.Pos_X, obj.Pos_Y, obj.Pos_Z, -1, RAGE.Elements.Player.LocalPlayer.Handle, 0);
                    int hit = -1;
                    int entityHit = -1;
                    Vector3 endCoords = new Vector3();
                    Vector3 surfaceNormal = new Vector3();
                    GetShapeTestResult(raycast, ref hit, endCoords, surfaceNormal, ref entityHit);

                    if (hit == 1) return;

                    var dist = Vector3.Distance(new Vector3(obj.Pos_X, obj.Pos_Y, obj.Pos_Z), camPos);
                    float scale = 1 / dist * 2f;
                    float fov = 1 / GetGameplayCamFov() * 100f;
                    scale *= fov;
                    if (scale < 0.4)
                        scale = 0.4f;

                    SetTextScale(0.1f * scale, 0.55f * scale);
                    SetTextFont(0);
                    SetTextProportional(true);
                    SetTextColour(255, 255, 255, 255);
                    SetTextDropshadow(0, 0, 0, 0, 255);
                    SetTextOutline();
                    SetTextEdge(2, 0, 0, 0, 150);
                    SetTextDropShadow();
                    BeginTextCommandDisplayText("STRING");
                    SetTextCentre(true);
                    AddTextComponentSubstringPlayerName($"Key: {obj.Key}\nHash: {obj.Hash}\nComponents: {obj.Components.Length}");
                    EndTextCommandDisplayText(0f, 0f, 0);
                    ClearDrawOrigin();
                }
                else
                {
                    
                }
            });
        }
    }
}
