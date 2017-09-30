﻿//MIT, 2015-2017, WinterDev 

using System;

namespace LayoutFarm.CefBridge
{
    static class Program
    {    
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            LibFolderManager.CheckNativeLibs();
            //---------------------
            //this is designed for cef subprocess(eg gpu process, render process)
            //so we not include System.Drawing and System.Windows.Form
            //---------------------
            ReferencePaths.SUB_PROCESS_PATH = null; //use this same process..

            //1. load cef before OLE init (eg init winform) ***
            //see more detail ...  MyCef3InitEssential
            if (!MyCef3RenderProcessInitEssential.LoadAndInitCef3(args))
            {
                return;
            }
            //------------------------------------------
            //1. if this is main UI process
            //the code go here, and we just start
            //winform app as usual
            //2. if this is other process
            //mean this process is finish and will terminate soon.
            //so we do noting, just exit!
            //(***please note that 
            //*** we call ShutDownCef3 only in main thread ***)

            if (!MyCef3RenderProcessInitEssential.IsInMainProcess)
            {
                MyCef3RenderProcessInitEssential.ClearRemainingCefMsg();
                return;
            }
            /////////////////////////////////////////////
            MyCef3RenderProcessInitEssential.ClearRemainingCefMsg();
            MyCef3RenderProcessInitEssential.ShutDownCef3();
            //(***please note that 
            //*** we call ShutDownCef3 only in main thread ***)
        }
    }
}
