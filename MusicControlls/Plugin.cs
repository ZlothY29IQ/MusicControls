using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using BepInEx;
using Oculus.Platform;
using UnityEngine;
using static Photon.Pun.UtilityScripts.TabViewManager;
namespace MusicControls
{
	[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
	public class Plugin : BaseUnityPlugin
	{
        public static GameObject? med;
        public static AssetBundle? bundle;

        internal enum VirtualKeyCodes
        : uint
        {
            NEXT_TRACK = 0xB0,
            PREVIOUS_TRACK = 0xB1,
            PLAY_PAUSE = 0xB3,
        }
        //waa

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        internal static extern void keybd_event(uint bVk, uint bScan, uint dwFlags, uint dwExtraInfo);

        internal static void SendKey(VirtualKeyCodes virtualKeyCode) => keybd_event((uint)virtualKeyCode, 0, 0, 0);
        public static void NextTrack() => SendKey(VirtualKeyCodes.NEXT_TRACK);

        public static void PreviousTrack() => SendKey(VirtualKeyCodes.PREVIOUS_TRACK);

        public static void PlayPause() => SendKey(VirtualKeyCodes.PLAY_PAUSE);
        void Start() => 
            GorillaTagger.OnPlayerSpawned(delegate
            {
                gameObject.AddComponent<Inputs>();
                using (Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("MusicControls.media"))
                {
                    bundle = AssetBundle.LoadFromStream(str);
                    if (bundle != null)
                    {
                        med = bundle.LoadAsset<GameObject>("mediaControlls");
                        gameObject.AddComponent<MediaControlls>();
                    }
                }
            });
    }
}