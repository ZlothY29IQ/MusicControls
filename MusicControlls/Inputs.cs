using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using GorillaLocomotion;
using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace MusicControls
{
    public class Inputs : MonoBehaviour
    {
        static bool rightControllerStickButton, leftControllerStickButton;
        bool steam;

        public Inputs()
        {
            PlatformTagJoin platform = (PlatformTagJoin)Traverse.Create(GorillaNetworking.PlayFabAuthenticator.instance).Field("platform").GetValue();
            steam =  platform.PlatformTag == "Steam";
        }

        public static bool CurrentPress()
        {
            if (Plugin.WhatHand.Value == "left")
            {
                return leftControllerStickButton;
            }
            else if (Plugin.WhatHand.Value == "right")
            {
                return rightControllerStickButton;
            }
            else
            {
                return leftControllerStickButton;
            }
        }

        public static Transform CurrentHand()
        {
            if (Plugin.WhatHand.Value == "left")
            {
                return GorillaTagger.Instance.leftHandTransform;
            }
            else if (Plugin.WhatHand.Value == "right")
            {
                return GorillaTagger.Instance.rightHandTransform;
            }
            else
            {
                return GorillaTagger.Instance.leftHandTransform;
            }
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
