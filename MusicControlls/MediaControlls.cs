using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MusicControls
{
    class MediaControlls : MonoBehaviour
    {
        static GameObject? MediaButtons;
        static MediaButton? SelectedButton;
        static AudioClip? openPlp, skip, back;
        static AudioSource? source;
        void Start()
        {
            if (MediaButtons = Instantiate(Plugin.med))
            {
                gameObject.AddComponent<Inputs>();
                foreach (Transform t in MediaButtons?.transform)
                {
                    t.AddComponent<MediaButton>();
                    t.gameObject.layer = LayerMask.NameToLayer("GorillaInteractable");
                }

                openPlp = Plugin.bundle?.LoadAsset<AudioClip>("open");
                openPlp?.LoadAudioData();

                skip = Plugin.bundle?.LoadAsset<AudioClip>("skip");
                skip?.LoadAudioData();

                back = Plugin.bundle?.LoadAsset<AudioClip>("bak");
                back?.LoadAudioData();

                source = MediaButtons.GetComponentInChildren<AudioSource>();
                source.transform.SetParent(GorillaTagger.Instance.leftHandTransform);
                source.transform.localPosition = Vector3.zero;
                source.name = "MediaCSoundFX";
                Destroy(source.GetComponent<MediaButton>());
                source.gameObject.layer = 0;

                MediaButtons.name = "MediaControlls";

                Plugin.bundle?.UnloadAsync(false);
            }
        }
        void FixedUpdate()
        {
            MediaButtons?.SetActive(Inputs.Instance.leftControllerStickButton);
        }

        static void ButtonRun()
        {
            switch (SelectedButton?.name)
            {
                case "Play/Pause":
                    Plugin.PlayPause();
                    source?.PlayOneShot(openPlp);
                    break;
                case "Previous":
                    Plugin.PreviousTrack();
                    source?.PlayOneShot(back);
                    break;
                case "Next":
                    Plugin.NextTrack();
                    source?.PlayOneShot(skip);
                    break;
            }
            SelectedButton = null;
        }

        class MediaButton : MonoBehaviour
        {
            void OnEnable()
            {
                if (MediaButtons != null)
                {
                    MediaButtons.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                    MediaButtons.transform.LookAt(Camera.main.transform);
                }
                source?.PlayOneShot(openPlp);
            }

            void OnDisable()
            {
                ButtonRun();
            }

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
