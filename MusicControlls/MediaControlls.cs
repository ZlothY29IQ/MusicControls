using UnityEngine;
using TMPro;
using Windows.Media.Control;
using Windows.Media;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Foundation.Collections;
using System.Threading.Tasks;
using System;
namespace MusicControls
{
    class MediaControls : MonoBehaviour
    {
        private static GameObject? MediaButtons;
        private static MediaButton? SelectedButton;
        private static AudioClip? openPlp, skip, back;
        private static AudioSource? source;
        private static TextMeshProUGUI? playingText;

        void Start()
        {
            if (Plugin.med != null && (MediaButtons = Instantiate(Plugin.med)) is GameObject mediaButtons)
            {
                gameObject.AddComponent<Inputs>();

                source = mediaButtons.GetComponentInChildren<AudioSource>();
                playingText = mediaButtons.GetComponentInChildren<TextMeshProUGUI>();
                if (source != null)
                {
                    Transform hand = Inputs.CurrentHand();
                    source.transform.SetParent(hand);
                    source.transform.localPosition = Vector3.zero;
                    if (Plugin.SilentUI?.Value == "low")
                    {
                        float wawa = source.volume;
                        source.volume = wawa /2;
                    }
                    source.name = "MediaCSoundFX";
                }

                Transform mediaTransform = mediaButtons.transform;
                foreach (Transform t in mediaTransform)
                {
                    t.gameObject.AddComponent<MediaButton>();
                    t.gameObject.layer = LayerMask.NameToLayer("GorillaInteractable");
                }

                if (Plugin.bundle != null)
                {
                    openPlp = Plugin.bundle.LoadAsset<AudioClip>("open");
                    openPlp?.LoadAudioData();

                    skip = Plugin.bundle.LoadAsset<AudioClip>("skip");
                    skip?.LoadAudioData();

                    back = Plugin.bundle.LoadAsset<AudioClip>("bak");
                    back?.LoadAudioData();

                    Plugin.bundle.UnloadAsync(false);
                }

                mediaButtons.name = "MediaControls";
            }
        }


        void FixedUpdate()
        {
            MediaButtons?.SetActive(Inputs.CurrentPress());
            //playingText!.text = $"NOW PLAYING:\n{GetCurrtlyPlaying().ToUpper()}";
        }

        private static void ButtonRun()
        {
            PlaySound();
            switch (SelectedButton?.name)
            {
                case "Play/Pause":
                    Plugin.PlayPause();
                    break;
                case "Previous":
                    Plugin.PreviousTrack();
                    break;
                case "Next":
                    Plugin.NextTrack();
                    break;
            }
            SelectedButton = null;
        }

        static void PlaySound()
        {
            if (Plugin.SilentUI?.Value != "true")
            {
                switch (SelectedButton?.name)
                {
                    case "Play/Pause":
                        source?.PlayOneShot(openPlp);
                        break;
                    case "Previous":
                        source?.PlayOneShot(back);
                        break;
                    case "Next":
                        source?.PlayOneShot(skip);
                        break;
                }
            }
        }

        private class MediaButton : MonoBehaviour
        {
            void OnEnable()
            {
                if (MediaButtons != null)
                {
                    Transform hand = Inputs.CurrentHand();
                    Transform mediaTransform = MediaButtons.transform;
                    mediaTransform.position = hand.position;
                    mediaTransform.LookAt(Camera.main.transform);
                }
                if (Plugin.SilentUI?.Value != "true")
                {
                    source?.PlayOneShot(openPlp);
                }
            }

            void OnDisable() => ButtonRun();

            void OnTriggerEnter(Collider collider)
            {
                if (collider.GetComponent<GorillaTriggerColliderHandIndicator>() != null)
                {
                    SelectedButton = this;
                }
            }

            void OnTriggerExit(Collider other)
            {
                if (SelectedButton == this)
                {
                    SelectedButton = null;
                }
            }
        }
    }
}
