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

            Events.Add("CEF:ExecuteGlobalMutationString", ExecuteGlobalMutationString);
            Events.Add("CEF:ExecuteGlobalMutationInt", ExecuteGlobalMutationInt);
            Events.Add("CEF:ExecuteGlobalMutationBool", ExecuteGlobalMutationBool);
            Events.Add("CEF:ExecuteModuleMutationString", ExecuteModuleMutationString);
            Events.Add("CEF:ExecuteModuleMutationInt", ExecuteModuleMutationInt);
            Events.Add("CEF:ExecuteModuleMutationBool", ExecuteModuleMutationBool);
        }

        public static void ExecuteModuleMutationString(string module, string mutation, string payload)
        {
            Window.ExecuteJs($"store.commit('{module}/{mutation}', '{payload}')");
        }

        public static void ExecuteModuleMutationInt(string module, string mutation, int payload)
        {
            Window.ExecuteJs($"store.commit('{module}/{mutation}', {payload})");
        }

        public static void ExecuteModuleMutationBool(string module, string mutation, bool payload)
        {
            Window.ExecuteJs($"store.commit('{module}/{mutation}', {payload.ToString().ToLower()})");
        }

        public static void ExecuteGlobalMutationString(string mutation, string payload)
        {
            Window.ExecuteJs($"store.commit('{mutation}', '{payload}')");
        }

        public static void ExecuteGlobalMutationInt(string mutation, int payload)
        {
            Window.ExecuteJs($"store.commit('{mutation}', {payload})");
        }

        public static void ExecuteGlobalMutationBool(string mutation, bool payload)
        {
            Window.ExecuteJs($"store.commit('{mutation}', {payload.ToString().ToLower()})");
        }

        public void ToggleCursor(bool toggle)
        {
            Cursor.Visible = toggle;
        }
        private void ExecuteGlobalMutationString(object[] args)
        {
            Window.ExecuteJs($"store.commit('{args[0].ToString()}', '{args[1].ToString()}')");
        }

        private void ExecuteGlobalMutationInt(object[] args)
        {
            Window.ExecuteJs($"store.commit('{args[0].ToString()}', {Convert.ToInt32(args[1])})");
        }

        private void ExecuteGlobalMutationBool(object[] args)
        {
            Window.ExecuteJs($"store.commit('{args[0].ToString()}', {(args[1]).ToString().ToLower()})");
        }

        private void ExecuteModuleMutationString(object[] args)
        {
            Window.ExecuteJs($"store.commit('{args[0].ToString()}/{args[1].ToString()}', '{args[2].ToString()}')");
        }

        private void ExecuteModuleMutationInt(object[] args)
        {
            Window.ExecuteJs($"store.commit('{args[0].ToString()}/{args[1].ToString()}', '{Convert.ToInt32(args[2])}')");
        }

        private void ExecuteModuleMutationBool(object[] args)
        {
            Window.ExecuteJs($"store.commit('{args[0].ToString()}/{args[1].ToString()}', {(args[2]).ToString().ToLower()})");
        }
    }
}
