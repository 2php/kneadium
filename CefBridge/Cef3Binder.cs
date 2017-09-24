﻿//MIT, 2015-2017, WinterDev

using System;
using System.Runtime.InteropServices;
#if NET20
namespace System.Runtime.CompilerServices
{
    public partial class ExtensionAttribute : Attribute { }
}
#endif  
namespace LayoutFarm.CefBridge
{

    public delegate void SimpleDel();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void MyCefCallback(int id, IntPtr args);
    //----------------------------------------------------------------------
    //cef msg constant
    //---------------------------------------------------------------------- 

    public enum MyCefMsg
    {
        MYCEF_MSG_UNKNOWN = 0,

        CEF_MSG_ClientHandler_NotifyBrowserClosing = 100,
        CEF_MSG_ClientHandler_NotifyBrowserClosed = 101,
        CEF_MSG_ClientHandler_NotifyBrowserCreated = 102,


        CEF_MSG_ClientHandler_ShowDevTools = 107,
        CEF_MSG_ClientHandler_CloseDevTools = 108,


        CEF_MSG_ClientHandler_SetResourceManager = 140,
        CEF_MSG_RequestUrlFilter2 = 142,
        CEF_MSG_BinaryResouceProvider_OnRequest = 145,

        //
        CEF_MSG_CefSettings_Init = 150,
        CEF_MSG_MainContext_GetConsoleLogPath = 151,
        CEF_MSG_OSR_Render = 155,
        //
        CEF_MSG_RenderDelegate_OnWebKitInitialized = 201,
        CEF_MSG_RenderDelegate_OnContextCreated = 202,
        CEF_MSG_RenderDelegate_OnContextReleased = 203,
        CEF_MSG_OnQuery = 205,
        //
        CEF_MSG_MyCefDomGetTextWalk_Visit = 302,
        CEF_MSG_MyV8ManagedHandler_Execute = 301,
        CEF_MSG_HereOnRenderer = 303,

        CEF_MSG_ClientHandler_NotifyTitle = 502,
        CEF_MSG_ClientHandler_NotifyAddress = 503,
    }

    public enum CefSettingsKey
    {
        CEF_SETTINGS_BrowserSubProcessPath = 9,
        CEF_SETTINGS_CachePath = 10,
        CEF_SETTINGS_ResourcesDirPath = 11,
        CEF_SETTINGS_UserDirPath = 12,
        CEF_SETTINGS_LocalDirPath = 14,
        CEF_SETTINGS_IgnoreCertError = 15,
        CEF_SETTINGS_RemoteDebuggingPort = 17,

        CEF_SETTINGS_LogFile = 18,
        CEF_SETTINGS_LogSeverity = 19,
    }

    public struct MyCefBw
    {
        //this is my custom class
        internal readonly IntPtr ptr;
        internal MyCefBw(IntPtr ptr)
        {
            this.ptr = ptr;
        }
        public Auto.CefBrowser GetBrowser()
        {
            JsValue ret;
            JsValue a1 = new JsValue();
            JsValue a2 = new JsValue();

            Cef3Binder.MyCefBwCall2(ptr,
                (int)CefBwCallMsg.CefBw_GetCefBrowser,
                out ret, ref a1, ref a2);
            return new Auto.CefBrowser(ret.Ptr);
        }
        public Auto.CefFrame GetMainFrame()
        {
            JsValue ret;
            JsValue a1 = new JsValue();
            JsValue a2 = new JsValue();

            Cef3Binder.MyCefBwCall2(ptr,
                (int)CefBwCallMsg.CefBw_GetMainFrame,
                out ret, ref a1, ref a2);
            return new Auto.CefFrame(ret.Ptr);
        }
    }


    public struct NativeMyCefStringHolder
    {
        internal IntPtr nativePtr;
        internal NativeMyCefStringHolder(IntPtr nativePtr)
        {
            this.nativePtr = nativePtr;
        }
        public void Dispose()
        {
            Cef3Binder.MyCefDeletePtr(this.nativePtr);
            nativePtr = IntPtr.Zero;
        }

        public static NativeMyCefStringHolder CreateHolder(string str)
        {
            return new NativeMyCefStringHolder(Cef3Binder.MyCefCreateStringHolder(str));
        }
        public string ReadString(int len)
        {
            //--------
            if (len <= 0) return string.Empty;
            //-------- 
            if (len < 256)
            {
                unsafe
                {
                    char* h = stackalloc char[len];
                    int actualLen = 0;
                    Cef3Binder.MyCefStringHolder_Read(this.nativePtr, h, len, out actualLen);
                    return new string(h);
                }
            }
            else
            {
                char[] buffer = new char[len];
                unsafe
                {
                    fixed (char* h = &buffer[0])
                    {
                        int actualLen = 0;
                        Cef3Binder.MyCefStringHolder_Read(this.nativePtr, h, len, out actualLen);
                    }
                }
                return new string(buffer);
            }
        }
    }


    static partial class Cef3Binder
    {
        static Cef3InitEssential cefInitEssential;

        internal const string CEF_CLIENT_DLL = "cefclient.dll";
#if DEBUG
        public static bool s_dbugIsRendererProcess;
#endif

        static CefClientApp clientApp;


        public static IWindowForm CreateBlankForm(int width, int height)
        {
            return Cef3WinForms.s_currentImpl.CreateNewWindow(width, height);
        }
        public static IWindowForm CreateNewBrowserWindow(int width, int height)
        {
            return Cef3WinForms.s_currentImpl.CreateNewBrowserWindow(width, height);
        }
        public static void SafeUIInvoke(SimpleDel del)
        {
            Cef3WinForms.s_currentImpl.SaveUIInvoke(del);
        }

        public static bool LoadCef3(Cef3InitEssential cefInitEssential)
        {
            Cef3Binder.cefInitEssential = cefInitEssential;
            //follow these steps
            // 1. libcef
            if (!LoadNativeLibs(cefInitEssential))
            {
                return false;
            }

            //-----------------------------------------------------------
            //check start up process to see if what is this process
            //browser process
            //render process

            string[] startArgs = cefInitEssential.startArgs;
            if (startArgs.Length > 0)
            {
                CefStartArgs cefStartArg = CefStartArgs.Parse(startArgs);
                Cef3InitEssential.IsInRenderProcess = (cefStartArg.IsValidCefArgs && cefStartArg.ProcessType == "renderer");
                Cef3InitEssential.IsInMainProcess = (cefStartArg.IsValidCefArgs && cefStartArg.ProcessType == "");
                cefInitEssential.AfterProcessLoaded(cefStartArg);
            }
            else
            {
                Cef3InitEssential.IsInMainProcess = true;
            }
            //----------------------------------------------------------- 
            //check version ...
            //1.
            int myCefVersion = MyCefGetVersion();
            clientApp = cefInitEssential.CreateClientApp();
            return true;
        }

        static bool LoadNativeLibs(Cef3InitEssential initEssential)
        {

            //in version 3.2885.1548 (chrome 55+)
            PlatformNeutralMethods.LoadLibrary(cefInitEssential.GetLibChromeElfFileName());
            //1. lib cef
            string lib = initEssential.GetLibCefFileName();

            int tryLoadCount = 0;
            TRY_AGAIN:
            string lastErr = null;
            IntPtr libCefModuleHandler = PlatformNeutralMethods.LoadLibrary(lib);
            //Console.WriteLine(libCefModuleHandler);
            if (libCefModuleHandler == IntPtr.Zero)
            {
                lastErr = PlatformNeutralMethods.GetError();
                if (lastErr != null)
                {
                    if (lastErr == "2" & tryLoadCount < 3)
                    {
                        //if not finish
                        //System.Threading.Thread.Sleep(100);
                        tryLoadCount++;
                        goto TRY_AGAIN;
                    }
                    initEssential.AddLogMessage("load err code" + lastErr);
                    return false;
                }
            }

            //------------------------------------------------------------------
            //2. cef client
            lib = cefInitEssential.GetCefClientFileName();
            //if (!File.Exists(lib))
            //{
            //    initEssential.AddLogMessage("not found " + lib);
            //    return false;
            //}

            IntPtr nativeModule = PlatformNeutralMethods.LoadLibrary(lib);
            lastErr = PlatformNeutralMethods.GetError();
            //------------------------------------------------
            if (nativeModule == IntPtr.Zero)
            {
                return false;
            }
            return true;
        }

        //---------------------------------------------------
        //Cef
        //---------------------------------------------------
        //part 1:  
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport(CEF_CLIENT_DLL)]
        public static extern int MyCefGetVersion();
        //
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int RegisterManagedCallBack(MyCefCallback funcPtr, int callbackKind);

        //client app init, build native-side process handle, dll-init-main,
        //when call to this func, native-side will ask back for cef-setting via registered
        //mananged delegate.
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MyCefCreateClientApp(IntPtr processHandle);

        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MyCefCreateMyWebBrowser(MyCefCallback mxcallback);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MyCefCreateMyWebBrowserOSR(MyCefCallback mxcallback);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void MyCefSetupBrowserHwnd(IntPtr myCefBw, IntPtr hWndParent, int x, int y, int width, int height, string initUrl, IntPtr requestContext);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void MyCefSetupBrowserHwndOSR(IntPtr myCefBw, IntPtr hWndParent, int x, int y, int width, int height, string initUrl, IntPtr requestContext);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport(CEF_CLIENT_DLL)]
        public static extern void MyCefDoMessageLoopWork();

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport(CEF_CLIENT_DLL)]
        public static extern void MyCefQuitMessageLoop();
        //
        [DllImport(CEF_CLIENT_DLL)]
        public static extern int MyCefShutDown();

        //TODO: review here, send setting as json?
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void MyCefSetInitSettings(IntPtr cefSetting, CefSettingsKey keyName, string value);
       
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MyCefShowDevTools(IntPtr myCefBw, IntPtr myCefDevTool, IntPtr parentWindow);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MyCefDeletePtr(IntPtr nativePtr);
        //
#if DEBUG
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void MyCefDeletePtrArray(JsValue* nativePtr);
#endif
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern IntPtr MyCefCreatePdfPrintSetting(string pdfJsonConfig);
        //--------------------------------------------------- 
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void MyCefPrintToPdf(IntPtr myCefBw, IntPtr setting, string filename, MyCefCallback callback);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MyCefDomGetTextWalk(IntPtr myCefBw, MyCefCallback strCallBack);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MyCefDomGetSourceWalk(IntPtr myCefBw, MyCefCallback strCallBack);

        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static unsafe extern void MyCef_CefRegisterSchemeHandlerFactory(
           string schemeName,
           string startURL,
           IntPtr clientSchemeHandlerFactoryObject);
        //--------------------------------------------------- 
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr MyCefJsGetCurrentContext();
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void MyCefJsNotifyRenderer(MyCefCallback handler, IntPtr pars);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr MyCefJs_GetEnteredContext();
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr MyCefFrame_GetContext(IntPtr nativeFrame);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr MyCefJsGetGlobal(IntPtr jsContext);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr MyCefJs_EnterContext(IntPtr cefV8Context);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void MyCefJs_ExitContext(IntPtr cefV8Context);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr MyCefJs_New_V8Handler(MyCefCallback managedCallback);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        internal static extern void MyCefJs_CefV8Value_SetValue_ByString(IntPtr target, string key, IntPtr value, int setAttr);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        internal static extern bool MyCefJs_CefV8Value_IsFunc(IntPtr target);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void MyCefJs_CefV8Value_SetValue_ByIndex(IntPtr target, int index, IntPtr value);
        /// <summary>
        /// create cef string holder
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>

        internal static IntPtr MyCefCreateStringHolder(string str)
        {
            IntPtr nativePtr = MyCefCreateStringHolder(str, str.Length);
            return nativePtr;
        }
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        internal static extern IntPtr MyCefCreateStringHolder(string str, int len);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        internal static extern IntPtr MyCefCreateBufferHolder(int len);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        internal static extern unsafe IntPtr MyCefCreateBufferHolderWithInitData(int len, byte* initData);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        internal static extern IntPtr MyCefJs_CreateFunction(string name, IntPtr handler);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        internal static unsafe extern IntPtr MyCefJs_ExecJsFunctionWithContext(IntPtr cefJsFunc, IntPtr context, char* argAsJsonString);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        internal static extern bool MyCefJs_CefRegisterExtension(string extensionName, string extensionCode);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void MyCefString_Read(IntPtr cefStr, char* outputBuffer, int outputBufferLen, out int actualLength);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void MyCefStringHolder_Read(IntPtr mycefStrHolder, char* outputBuffer, int outputBufferLen, out int actualLength);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void MyCefStringGetRawPtr(IntPtr cefstring, out char* outputBuffer, out int actualLength);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern unsafe void MyCefJs_CefV8Value_ReadAsString(IntPtr cefV8Value, char* outputBuffer, int outputBufferLen, out int actualLength);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MyCefJs_MetReadArgAsInt32(IntPtr jsArgs, int index);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void MyCefJs_MetReadArgAsString(IntPtr jsArgs, int index, char* outputBuffer, int outputBufferLen, out int actualLength);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MyCefJs_MetReadArgAsCefV8Value(IntPtr jsArgs, int index);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MyCefJs_MetReadArgAsV8FuncHandle(IntPtr jsArgs, int index);
        //list func
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateStdList(int listType);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetListCount(int listType, IntPtr list, out int count);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetListElement(int elemType, IntPtr list, int index, ref JsValue jsvalue);
        //     
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern MyCefCallback MyCefJsValueGetManagedCallback(ref JsValue v);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MyCefJsValueSetManagedCallback(ref JsValue v, MyCefCallback cb);
        //
        [DllImport(CEF_CLIENT_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MyCefBwCall2(IntPtr myCefBw, int methodName, out JsValue ret, ref JsValue arg1, ref JsValue arg2);

        public static void MyCefBwCall(IntPtr myCefBw, CefBwCallMsg methodName, int value)
        {
            JsValue ret;
            //
            JsValue arg1 = new JsValue();
            arg1.Type = JsValueType.Integer;
            arg1.I32 = value;
            //
            JsValue arg2 = new JsValue();

            MyCefBwCall2(myCefBw, (int)methodName, out ret, ref arg1, ref arg2);
        }


        //public static void MyCefCreateNativeStringHolder(ref JsValue ret, string value)
        //{
        //    unsafe
        //    {
        //        ret.Type = JsValueType.NativeCefString;
        //        ret.Ptr = Cef3Binder.MyCefCreateStringHolder(value);
        //        ret.I32 = value.Length;
        //    }
        //}
        public static string CopyStringAndDestroyNativeSide(ref JsValue value)
        {
            NativeMyCefStringHolder ret_str = new NativeMyCefStringHolder(value.Ptr);
            string str = ret_str.ReadString(value.I32);
            ret_str.Dispose();
            value.Ptr = IntPtr.Zero;
            return str;
        }
        public static void CopyStdInt64ListAndDestroyNativeSide(IntPtr stdInt64List, System.Collections.Generic.List<long> outputList)
        {
            int listCount;
            Cef3Binder.GetListCount(1, stdInt64List, out listCount);
            for (int i = 0; i < listCount; ++i)
            {
                JsValue value = new JsValue();
                Cef3Binder.GetListElement(1, stdInt64List, i, ref value);
                outputList.Add(value.I64);
            }
            Cef3Binder.MyCefDeletePtr(stdInt64List);
        }
        public static void CopyStdStringListAndDestroyNativeSide(IntPtr stdStringList, System.Collections.Generic.List<string> outputList)
        {
            int listCount;
            Cef3Binder.GetListCount(2, stdStringList, out listCount);

            for (int i = 0; i < listCount; ++i)
            {
                JsValue value = new JsValue();
                Cef3Binder.GetListElement(2, stdStringList, i, ref value);
                outputList.Add(CopyStringAndDestroyNativeSide(ref value));
            }
            Cef3Binder.MyCefDeletePtr(stdStringList);
        }
    }


    public static class CefBinder2
    {
        public static bool RegisterCefExtension(string extensionName, string extensionCode)
        {
            return Cef3Binder.MyCefJs_CefRegisterExtension(extensionName, extensionCode);
        }
        public static void NotifyRendererAsync(MyCefCallback callback)
        {
            Cef3Binder.MyCefJsNotifyRenderer(callback, IntPtr.Zero);
        }
    }


    enum OsPlatform
    {
        Unknown,
        Windows,
        Mac,
        Linux
    }
    static class PlatformNeutralMethods
    {
        static NativeModuleLoader nativeModuleLoader;
        static PlatformNeutralMethods()
        {
            //evaluate current platform
            switch (GetOsName())
            {
                //TODO: review here for linux
                default:
                    throw new NotSupportedException();
                case OsPlatform.Windows:
                    nativeModuleLoader = new Win32NativeModuleLoader();
                    break;
                case OsPlatform.Mac:
                    nativeModuleLoader = new MacOsNativeModuleLoader();
                    break;
            }
        }
        public static OsPlatform GetOsName()
        {
            //check platform
#if NETCOREAPP1_1
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
                System.Runtime.InteropServices.OSPlatform.OSX))
            {
                return OsPlatform.Mac;
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
                System.Runtime.InteropServices.OSPlatform.Linux)
                )
            {
                return OsPlatform.Linux;
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
              System.Runtime.InteropServices.OSPlatform.Windows))
            {
                return OsPlatform.Windows;
            }
            else
            {
                return OsPlatform.Unknown;
            }            
#else
            OperatingSystem os = Environment.OSVersion;
            switch (os.Platform)
            {
                default:
                    throw new NotSupportedException();
                case PlatformID.MacOSX:
                    return OsPlatform.Mac;
                case PlatformID.Win32NT:
                    return OsPlatform.Windows;
            }
#endif
        }


        public static IntPtr LoadLibrary(string libname)
        {
            return nativeModuleLoader.LoadLib(libname);
        }
        public static string GetError()
        {
            return nativeModuleLoader.GetError();
        }
    }


    abstract class NativeModuleLoader
    {

        public abstract IntPtr LoadLib(string libname);
        public abstract bool FreeLib(IntPtr hModule);
        public abstract IntPtr GetFunc(IntPtr hModule, string funcName);
        public abstract string GetError();

    }
    class Win32NativeModuleLoader : NativeModuleLoader
    {
        //TODO: review here, check for other platforms
        //-----------------------------------------------
        //this is Windows Specific class ***
        [DllImport("Kernel32.dll")]
        static extern IntPtr LoadLibrary(string libraryName);
        [DllImport("Kernel32.dll")]
        static extern bool FreeLibrary(IntPtr hModule);
        [DllImport("Kernel32.dll")]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
        [DllImport("Kernel32.dll")]
        static extern uint SetErrorMode(int uMode);
        [DllImport("Kernel32.dll")]
        static extern uint GetLastError();

        public override IntPtr LoadLib(string libname)
        {
            return LoadLibrary(libname);
        }

        public override bool FreeLib(IntPtr hModule)
        {
            return FreeLibrary(hModule);
        }

        public override IntPtr GetFunc(IntPtr hModule, string funcName)
        {
            return GetProcAddress(hModule, funcName);
        }
        public override string GetError()
        {
            uint lastError = GetLastError();
            if (lastError == 0)
            {
                return null;
            }
            else
            {
                return lastError.ToString();
            }
        }
        //---------------------- 

    }
    class MacOsNativeModuleLoader : NativeModuleLoader
    {
        /*
#define RTLD_LAZY   1
#define RTLD_NOW    2
#define RTLD_GLOBAL 4
*/
        [DllImport("libdl.so")]
        static extern IntPtr dlopen(string filename, int flags);
        [DllImport("libdl.so")]
        static extern int dlclose(IntPtr handle);
        [DllImport("libdl.so")]
        static extern IntPtr dlsym(IntPtr handle, string symbol);
        [DllImport("libdl.so")]
        static extern string dlerror();

        public override bool FreeLib(IntPtr hModule)
        {
            return dlclose(hModule) != 0;
        }
        public override IntPtr GetFunc(IntPtr hModule, string funcName)
        {
            return dlsym(hModule, funcName);
        }
        public override IntPtr LoadLib(string libname)
        {
            return dlopen(libname, 2);
        }
        public override string GetError()
        {
            return dlerror();
        }
    }



    static class MyMetArgs
    {
        //TODO: inline? 

        internal static IntPtr GetNativeObjPtr(IntPtr nativePtr, out int argCountAndFlags)
        {
            unsafe
            {
                //return address of vargs
                argCountAndFlags = *((int*)nativePtr); //MyMetArgsN
                                                       //check flags

                if (((argCountAndFlags >> 18) & 1) == 1)
                {
                    //this native
                    return nativePtr;
                }
                else
                {     //struct MyMetArgsN
                      //{
                      //    int32_t argCount;
                      //    jsvalue* vargs;
                      //}; 
                    IntPtr h1 = (IntPtr)(((byte*)nativePtr) + sizeof(int));
                    return (IntPtr)(*((JsValue**)h1));
                }

            }
        }
        internal static string GetAsString(IntPtr varr, int index)
        {
            unsafe
            {
                return MyCefJsReadString((JsValue*)varr + index);
            }
        }
        internal static string GetAsString(IntPtr cefStringPtr)
        {
            unsafe
            {
                char* rawCefString_char16_t;
                int actualLen;
                Cef3Binder.MyCefStringGetRawPtr(cefStringPtr, out rawCefString_char16_t, out actualLen);
                return new string(rawCefString_char16_t, 0, actualLen);
            }
        }
        internal static int GetAsInt32(IntPtr varr, int index)
        {
            unsafe
            {
                return ((JsValue*)varr + index)->I32;
            }
        }
        internal static void SetAsInt32(IntPtr varr, int index, int value)
        {
            unsafe
            {
                ((JsValue*)varr + index)->I32 = value;
            }
        }
        internal static uint GetAsUInt32(IntPtr varr, int index)
        {
            unsafe
            {

                return (uint)((JsValue*)varr + index)->I32;
            }
        }
        internal static long GetAsInt64(IntPtr varr, int index)
        {
            unsafe
            {
                return ((JsValue*)varr + index)->I64;
            }
        }
        internal static ulong GetAsUInt64(IntPtr varr, int index)
        {
            unsafe
            {
                return (ulong)((JsValue*)varr + index)->I64;
            }
        }
        internal static bool GetAsBool(IntPtr varr, int index)
        {
            unsafe
            {
                return ((JsValue*)varr + index)->I32 != 0;
            }
        }
        internal static double GetAsDouble(IntPtr varr, int index)
        {
            unsafe
            {
                return ((JsValue*)varr + index)->Num;
            }
        }
        internal static float GetAsFloat(IntPtr varr, int index)
        {
            unsafe
            {
                return (float)((JsValue*)varr + index)->Num;
            }
        }
        internal static IntPtr GetAsIntPtr(IntPtr varr, int index)
        {
            unsafe
            {
                return ((JsValue*)varr + index)->Ptr;
            }
        }
        internal static void SetAsIntPtr(IntPtr varr, int index, IntPtr value)
        {
            unsafe
            {
                ((JsValue*)varr + index)->Ptr = value;
            }
        }
        internal static void SetBoolToAddress(IntPtr varr, int index, bool value)
        {
            unsafe
            {
                JsValue* jsvalue = ((JsValue*)varr + index);
                *((bool*)jsvalue->Ptr) = value;
            }
        }
        internal static void SetUInt32ToAddress(IntPtr varr, int index, uint value)
        {
            unsafe
            {

                JsValue* jsvalue = ((JsValue*)varr + index);
                *((uint*)jsvalue->Ptr) = value;
            }
        }
        internal static void SetInt32ToAddress(IntPtr varr, int index, int value)
        {
            unsafe
            {

                JsValue* jsvalue = ((JsValue*)varr + index);
                *((int*)jsvalue->Ptr) = value;
            }
        }

        unsafe static string MyCefJsReadString(JsValue* ret)
        {
            int actualLen;
            int buffLen = ret->I32 + 1; //string len
            //check if string is on method-call's frame stack or heap
            if (ret->Type == JsValueType.NativeCefString)
            {
                char* rawCefString_char16_t;
                Cef3Binder.MyCefStringGetRawPtr(ret->Ptr, out rawCefString_char16_t, out actualLen);
                return new string(rawCefString_char16_t, 0, actualLen);
            }
            if (buffLen < 1024)
            {
                char* buffHead = stackalloc char[buffLen];
                Cef3Binder.MyCefStringHolder_Read(ret->Ptr, buffHead, buffLen, out actualLen);
                if (actualLen > buffLen)
                {
                    //read more
                }
                return new string(buffHead, 0, actualLen);
            }
            else
            {
                char[] buffHead = new char[buffLen];
                fixed (char* h = &buffHead[0])
                {
                    Cef3Binder.MyCefStringHolder_Read(ret->Ptr, h, buffLen, out actualLen);
                    if (actualLen > buffLen)
                    {
                        //read more
                    }
                }
                return new string(buffHead, 0, actualLen);
            }

        }
    }
}
