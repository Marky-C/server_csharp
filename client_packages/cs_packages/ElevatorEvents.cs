using System;
using System.Collections.Generic;
using System.Text;
using RAGE;

namespace ProjectClient
{
    class ElevatorEvents : Events.Script
    {
        public ElevatorEvents()
        {
            Events.Add("setElevatorChoosedLevel", SetElevatorChoosedLevel);
        }

        private void SetElevatorChoosedLevel(object[] args)
        {
            Events.CallRemote("SetElevatorChoosedLevel", Convert.ToInt32(args[0]));
        }
    }
}
