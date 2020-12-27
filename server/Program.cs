using server.core;
using server.utils;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace servers
{ //Testcommit
    class Program
    {
        public static CoreSettings _CoreSettings = new CoreSettings();
        public static DateTime _StartTime;
        public static List<scripting.Script> Scripts;
        public static IniFile configFile;
        static void Main(string[] args)
        {   
            _StartTime = DateTime.Now;  //Note down the current time of the program start, needed for logs
            Scripts = new List<scripting.Script>();   //Create list for scripts

            //Check if LOG Dir exists
            if (!Directory.Exists("Logs/"))
                Directory.CreateDirectory("Logs/");
            
            File.AppendAllTextAsync("Logs/current.txt", "++++++++++++++++++++ | LOG " + DateTime.Now + " | ++++++++++++++++++++\n");//Print out log file header (file only)
            
            if (!HandleArgs(args)) //Read and set stuff with the arguments passed
            {
                Log.Print("Wrong or zero arguments, needs at least -CONFIGFILE <path_to_file_without_anglebrackets> (config file must also have valid values applied)\n", 2);
                StopServerSafely(); //needed otherwise we wouldn't handle the log stuff right since this also resembles the exit of the program
                return;
            }

            Log.WriteLine("Simple Enterprise Resource Planning | © 2020 www.fanter.eu", Color.Yellow);  //Final beginning of the server flow
            scripting.Script scr = new scripting.Script("main");

            cmdbegin:

            string cmdtext = System.Console.ReadLine();
            if (!String.IsNullOrEmpty(cmdtext) /*&& cmdtext[0] == '/' */)
            {
                Debug.Print("CmdInput:");
                //DEBUG Console.WriteLine(cmdtext.Substring(1, cmdtext.Length-1));
                string[] splitted = cmdtext/*.Substring(1, cmdtext.Length - 1)*/.Split(' ');
                if(splitted[0].Equals("exit") || splitted[0].Equals("c"))
                {
                    Console.WriteLine("Stopping the server..");
                    Thread.Sleep(1800);
                    StopServerSafely();
                    goto exit;
                }
                else if (splitted[0].Equals("loadscript"))
                {
                    Debug.Print(" 1");
                    if (splitted.Length > 1 && !String.IsNullOrEmpty(splitted[1]))
                    {

                        Debug.Print(" 2");
                        if (splitted[1].Contains('.'))
                        {
                            Debug.Print("3");
                            Console.WriteLine("Command Help: For unloadscript use <script> Parameter containing the file-name of the script\nAlso make sure no file-endings are used.");
                        }
                        else
                        {
                            Debug.Print("2B");
                            if (File.Exists("Scripts/" + splitted[1] + ".amx"))
                            {
                                Debug.Print("3B");
                                Program.Scripts.Add(new scripting.Script(splitted[1]));

                            }
                        }
                    }
                            
                }
                else if (splitted[0].Equals("unloadscript"))
                {
                    /*  TODO:
                     *   - Script-Object wird nicht aus Scripts-Liste entfernt bei UnloadScript befehl
                     * 
                     * 
                     */
                    Console.WriteLine("Scripts contains " + Scripts.Count + " scripts");
                    if (splitted.Length > 1 && !String.IsNullOrEmpty(splitted[1]))
                    {
                        if (splitted[1].Contains('.'))
                        {
                            Console.WriteLine("Command Help: For unloadscript use <script> Parameter containing the file-name of the script\nAlso make sure no file-endings are used.");
                        }
                        else
                        {
                            bool found = false;
                            scripting.Script dummyDelScript = null; 
                            foreach(scripting.Script script in Scripts)
                            {

                                if (script._amxFile.Equals(splitted[1]))
                                {
                                    if (script.amx == null) continue;
                                    if (script.amx.FindPublic("OnUnload") != null)
                                        script.amx.FindPublic("OnUnload").Execute();

                                    found = true;

                                    script.amx.Dispose();
                                    script.amx = null;
                                    dummyDelScript = script;
                                    Console.WriteLine("Script " + script._amxFile + " unloaded.");
                                    break;
                                }
                            }
                            if (!found) Console.WriteLine("UNLOAD: Script " + splitted[1] + " not found.");
                            else
                            {
                                Scripts.Remove(dummyDelScript);
                                dummyDelScript = null;
                            }
                                
                        } 
                    }

                }
                //else if(splitted[1].Equals(""))
            }

            goto cmdbegin;
            exit:
            return;
            
        }






        private static bool HandleArgs(string[] args)
        {
            if (args.Length == 0) return false;
            else
            {
                Log.Print("Argument List:\n", 3);
                for (int i = 0; i < args.Length; i++) 
                {
                    if (args[i] is null || args[i][0] != '-') continue;
                    Log.Write(args[i] + ": ", Color.Green);
                    switch (args[i])
                    {
                        case "-CONFIGFILE":
                            {
                                i++;
                                _CoreSettings._SettingFile = args[i];
                                if (!File.Exists(args[i]))
                                    File.Create(args[i]);
                                else configFile = new IniFile(args[i]);
                                Log.WriteLine(args[i], Color.Yellow);
                                break;
                            }
                        case "-LOGLEVEL":
                            {
                                i++;
                                try
                                {
                                    _CoreSettings.LogLevel = Int32.Parse(args[i]);
                                    Log.WriteLine(args[i], Color.Yellow);
                                }
                                catch (Exception e) { Log.Print(e); }
                                break;
                            }
                        case "-CORETEST":
                            {
                                i++;
                                Log.WriteLine(args[i], Color.Yellow);
                                break;
                            }
                    }
                }
                Log.Print("Arguments done\n", 3);
                return true;
            }
        }

        static private void StopServerSafely()
        {

            File.Copy("Logs/current.txt", ("Logs/" + _StartTime.ToString().Replace(':', '-') + ".txt")); //copy current log txt to one with the date in name and delete the old on
            if (File.Exists("Logs/current.txt")) File.Delete("Logs/current.txt");
        }
    }
}
