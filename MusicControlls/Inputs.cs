using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace MusicControls
{
    public class Inputs : MonoBehaviour
    {
        private static bool rightControllerStickButton, leftControllerStickButton;
        private bool steam;

        private void Awake()
        {
            if (GorillaNetworking.PlayFabAuthenticator.instance != null)
            {
                var platform = (PlatformTagJoin)Traverse.Create(GorillaNetworking.PlayFabAuthenticator.instance)
                    .Field("platform").GetValue();
                steam = platform?.PlatformTag == "Steam";
            }
            else
            {
                Debug.LogWarning("PlayFabAuthenticator instance is null, how the hell");
            }
        }

        public static bool CurrentPress() =>
            Plugin.WhatHand?.Value switch
            {
                "right" => rightControllerStickButton,
                _ => leftControllerStickButton
            };

        public static Transform CurrentHand() =>
            Plugin.WhatHand?.Value switch
            {
                "right" => GorillaTagger.Instance.rightHandTransform,
                _ => GorillaTagger.Instance.leftHandTransform
            };

        private void Update()
        {
            if (steam)
            {
                leftControllerStickButton = SteamVR_Actions.gorillaTag_LeftJoystickClick.state;
                rightControllerStickButton = SteamVR_Actions.gorillaTag_RightJoystickClick.state;
            }
            else if (ControllerInputPoller.instance != null)
            {
                var left = ControllerInputPoller.instance.leftControllerDevice;
                var right = ControllerInputPoller.instance.rightControllerDevice;

                left.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out leftControllerStickButton);
                right.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out rightControllerStickButton);
            }
            else
            {
                Debug.LogWarning("ControllerInputPoller is null somehow");
            }
        }
    }
}
