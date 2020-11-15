using server.core;
using server.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace server
{ //Testcommit
    class Program
    {
        public static CoreSettings _CoreSettings = new CoreSettings();
        public static DateTime _StartTime;
        public static List<scripting.Script> Scripts;

        static void Main(string[] args)
        {
            //Note down the current time of the program start, needed for logs
            _StartTime = DateTime.Now;

            //Create list for scripts
            Scripts = new List<scripting.Script>();
            
            //Check if LOG Dir exists
            if (!Directory.Exists("Logs/"))
                Directory.CreateDirectory("Logs/");


            //Print out log file header (file only)
            File.AppendAllTextAsync("Logs/current.txt", "++++++++++++++++++++ | LOG " + DateTime.Now + " | ++++++++++++++++++++\n");
            
            //Read and set stuff with the arguments passed
            if (!HandleArgs(args))
            {
                Log.Print("Wrong or zero arguments, needs at least -CONFIGFILE <path_to_file_without_anglebrackets> (config file must also have valid values applied)\n", 2);
                StopServerSafely(); //needed otherwise we wouldn't handle the log stuff right since this also resembles the exit of the program
                return;
            }

            //Final beginning of the server flow
            Log.WriteLine("Simple Enterprise Resource Planning | © 2020 www.fanter.eu", Color.Yellow);

            scripting.Script scr = new scripting.Script("main");





            System.Console.ReadLine();

            StopServerSafely();
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
            //copy current log txt to one with the date in name and delete the old on
            File.Copy("Logs/current.txt", ("Logs/" + _StartTime.ToString().Replace(':', '-') + ".txt"));
            if (File.Exists("Logs/current.txt")) File.Delete("Logs/current.txt");
        }
    }
}
