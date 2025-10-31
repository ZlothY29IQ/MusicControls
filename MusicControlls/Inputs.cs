using GorillaLocomotion;
using GorillaNetworking;
using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace MusicControls;

public class Inputs : MonoBehaviour
{
    private static bool rightControllerStickButton, leftControllerStickButton;
    private        bool steam;

    private void Awake()
    {
        if (PlayFabAuthenticator.instance != null)
        {
            PlatformTagJoin? platform = (PlatformTagJoin)Traverse.Create(PlayFabAuthenticator.instance)
                                                                 .Field("platform").GetValue();

            steam = platform?.PlatformTag == "Steam";
        }
        else
        {
            Debug.LogWarning("PlayFabAuthenticator instance is null, how the hell");
        }
    }

    private void Update()
    {
        if (steam)
        {
            leftControllerStickButton  = SteamVR_Actions.gorillaTag_LeftJoystickClick.state;
            rightControllerStickButton = SteamVR_Actions.gorillaTag_RightJoystickClick.state;
        }
        else if (ControllerInputPoller.instance != null)
        {
            InputDevice left  = ControllerInputPoller.instance.leftControllerDevice;
            InputDevice right = ControllerInputPoller.instance.rightControllerDevice;

            left.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out leftControllerStickButton);
            right.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out rightControllerStickButton);
        }
        else
        {
            Debug.LogWarning("ControllerInputPoller is null somehow");
        }
    }

    public static bool CurrentPress() =>
            Plugin.WhatHand?.Value switch
            {
                    "right" => rightControllerStickButton,
                    var _   => leftControllerStickButton,
            };

    public static Transform CurrentHand() =>
            Plugin.WhatHand?.Value switch
            {
                    "right" => GTPlayer.Instance.LeftHand.controllerTransform,
                    var _   => GTPlayer.Instance.RightHand.controllerTransform,
            };
}