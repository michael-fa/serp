﻿using AMXWrapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace server.scripting
{
    public class Script
    {
        public string _amxFile = null;
        AMX amx;

        public Script(string _amxFile)
        {
            this._amxFile = _amxFile;
            try
            {
                 amx = new AMX("Scripts/" + _amxFile + ".amx");
            }
            catch(Exception e)
            {
                server.utils.Log.Print(e);
                server.utils.Log.Print("'" + _amxFile + "' script not loaded.", 2);
                return; 
            }

            amx.LoadLibrary(AMXDefaultLibrary.Core);
            this.RegisterNatives();

            amx.ExecuteMain();

            //AMXPublic p = amx.FindPublic("OnStart");
            //p.Execute();

            server.Program.Scripts.Add(this);
            return;
        }

        public bool RegisterNatives()
        {
            amx.Register("printc", (amx1, args1) => Natives.printc(amx1, args1, this));
            amx.Register("LoadScript", (amx1, args1) => Natives.loadscript(amx1, args1, this));


            return true;
        }
    }
}
