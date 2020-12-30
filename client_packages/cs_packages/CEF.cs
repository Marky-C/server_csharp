using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Ui;
using static RAGE.Elements.Player;
using static RAGE.Game.Pad;

namespace ProjectClient
{
    public class CEF : Events.Script
    {
        public static HtmlWindow Window = new HtmlWindow("package://cef/index.html");

        public CEF()
        {
            Window.Active = true;

            Events.Add("CEF:ExecuteGlobalMutation", ExecuteGlobalMutation);
            Events.Add("CEF:ExecuteModuleMutation", ExecuteModuleMutation);
        }

        public static void ExecuteModuleMutation(string module, string mutation, object payload)
        {
            string type = payload.GetType().Name;

            switch (type)
            {
                case "Int32":
                    {
                        Window.ExecuteJs($"store.commit('{module}/{mutation}', {Convert.ToInt32(payload)})");
                        break;
                    }
                case "String":
                    {
                        Window.ExecuteJs($"store.commit('{module}/{mutation}', '{payload.ToString()}')");
                        break;
                    }
                case "Boolean":
                    {
                        Window.ExecuteJs($"store.commit('{module}/{mutation}', {payload.ToString().ToLower()})");
                        break;
                    }
                default:
                    {
                        RAGE.Ui.Console.Log(ConsoleVerbosity.Error, "CEF-ERROR: Invalid type.");
                        break;
                    }
            }
        }

        public static void ExecuteGlobalMutation(string mutation, object payload)
        {
            string type = payload.GetType().Name;

            switch (type)
            {
                case "Int32":
                    {
                        Window.ExecuteJs($"store.commit('{mutation}', '{Convert.ToInt32(payload)}')");
                        break;
                    }
                case "String":
                    {
                        Window.ExecuteJs($"store.commit('{mutation}', '{payload.ToString()}')");
                        break;
                    }
                case "Boolean":
                    {
                        Window.ExecuteJs($"store.commit('{mutation}', {payload.ToString().ToLower()})");
                        break;
                    }
                default:
                    {
                        RAGE.Ui.Console.Log(ConsoleVerbosity.Error, "CEF-ERROR: Invalid type.");
                        break;
                    }
            }
            
        }

        public void ToggleCursor(bool toggle)
        {
            Cursor.Visible = toggle;
        }
        private void ExecuteGlobalMutation(object[] args)
        {
            string type = args[1].GetType().Name;

            switch (type)
            {
                case "Int32":
                    {
                        Window.ExecuteJs($"store.commit('{args[0].ToString()}', '{Convert.ToInt32(args[1])}')");
                        break;
                    }
                case "String":
                    {
                        Window.ExecuteJs($"store.commit('{args[0].ToString()}', '{args[1].ToString()}')");
                        break;
                    }
                case "Boolean":
                    {
                        Window.ExecuteJs($"store.commit('{args[0].ToString()}', {args[1].ToString().ToLower()})");
                        break;
                    }
                default:
                    {
                        RAGE.Ui.Console.Log(ConsoleVerbosity.Error, "CEF-ERROR: Invalid type.");
                        break;
                    }
            }
        }

        private void ExecuteModuleMutation(object[] args)
        {
            string type = args[2].GetType().Name;

            switch (type)
            {
                case "Int32":
                    {
                        Window.ExecuteJs($"store.commit('{args[0].ToString()}/{args[1].ToString()}', '{Convert.ToInt32(args[2])}')");
                        break;
                    }
                case "String":
                    {
                        Window.ExecuteJs($"store.commit('{args[0].ToString()}/{args[1].ToString()}', '{args[2].ToString()}')");
                        break;
                    }
                case "Boolean":
                    {
                        Window.ExecuteJs($"store.commit('{args[0].ToString()}/{args[1].ToString()}', {args[2].ToString().ToLower()})");
                        break;
                    }
                default:
                    {
                        RAGE.Ui.Console.Log(ConsoleVerbosity.Error, "CEF-ERROR: Invalid type.");
                        break;
                    }
            }
        }
    }
}
