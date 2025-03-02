using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace MusicControls
{
    public class Inputs : MonoBehaviour
    {
        public bool rightControllerStickButton, leftControllerStickButton;
        public static Inputs Instance;
        bool steam;

        public Inputs()
        {
            Instance = this;
            PlatformTagJoin platform = (PlatformTagJoin)Traverse.Create(GorillaNetworking.PlayFabAuthenticator.instance).Field("platform").GetValue();
            steam =  platform.PlatformTag == "Steam";
        }
        public void Update()
        {
            if (steam)
            {
                leftControllerStickButton = SteamVR_Actions.gorillaTag_LeftJoystickClick.state;
                rightControllerStickButton = SteamVR_Actions.gorillaTag_RightJoystickClick.state;
            }
            else
            {
                var left = ControllerInputPoller.instance.leftControllerDevice;
                var right = ControllerInputPoller.instance.rightControllerDevice;
                left.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out leftControllerStickButton);
                right.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out rightControllerStickButton);
            }

        }
    }
}
