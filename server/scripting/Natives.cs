using AMXWrapper;

namespace server.scripting
{

    public static class Natives
    {
        public static int printc(AMX amx1, AMXArgumentList args1, Script caller_script)
        {
            utils.Log.Print(args1[0].AsString(), 4, caller_script._amxFile);
            return 1;
        }

        public static int loadscript(AMX amx1, AMXArgumentList args1, Script caller_script)
        {
            Program.Scripts.Add(new Script(args1[0].AsString()));
            return 1;
        }

        public static int unloadscript(AMX amx1, AMXArgumentList args1, Script caller_script)
        {
            foreach (Script scr in Program.Scripts)
            {
                if (scr.amx == amx1)
                {
                    if (scr.amx.FindPublic("OnUnload") != null)
                        scr.amx.FindPublic("OnUnload").Execute();

                    amx1.Dispose();
                    amx1 = null;
                    Program.Scripts.Remove(scr);
                }   

                
            }
            return 1;
        }
    }
}