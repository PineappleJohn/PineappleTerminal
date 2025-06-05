using GorillaExtensions;
using GorillaTag;
using System;
using System.Collections;
using UnityEngine;

namespace PineappleMod.Console
{
    public class Key : MonoBehaviour
    {
        public string characterString;

        public Action<string> OnButtonPressedEvent;

        public float pressTime;

        public float repeatCooldown = 2f;

        public Renderer ButtonRenderer;

        public ButtonColorSettings ButtonColorSettings = new ButtonColorSettings();

        private MaterialPropertyBlock propBlock;

        /*
         * Thx monkemod and HanSolo1000Falcon for the help!
         * */

        public virtual void Awake()
        {
            ButtonColorSettings.PressedColor = Color.red;
            ButtonColorSettings.UnpressedColor = Color.white;
            ButtonColorSettings.PressedTime = 0.2f;
             
            if (ButtonRenderer == null)
            {
                ButtonRenderer = GetComponent<Renderer>();
            }

            propBlock = new MaterialPropertyBlock();
            pressTime = 0f;

            gameObject.layer = GorillaTagger.Instance.rightHandTriggerCollider.layer; // Layer 18 doesnt work (GorillaInteractable)
            gameObject.GetOrAddComponent<BoxCollider>().isTrigger = true;
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject == (GorillaTagger.Instance.leftHandTriggerCollider || GorillaTagger.Instance.rightHandTriggerCollider))
            {
                var component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();
                PressButtonColourUpdate();
                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, component.isLeftHand, 0.1f);
                OnButtonPressedEvent?.Invoke(characterString);
            }
        }

        public virtual void TestClick() {
            PressButtonColourUpdate();
            GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.1f);
            OnButtonPressedEvent?.Invoke(characterString);
        }

        public virtual void PressButtonColourUpdate()
        {
            propBlock.SetColor("_BaseColor", Color.red);
            propBlock.SetColor("_Color", Color.red);
            ButtonRenderer.SetPropertyBlock(propBlock);
            pressTime = Time.time;
            StartCoroutine(ButtonColorUpdate_Local());
            IEnumerator ButtonColorUpdate_Local()
            {
                yield return new WaitForSeconds(ButtonColorSettings.PressedTime);
                if (pressTime != 0f && Time.time > ButtonColorSettings.PressedTime + pressTime)
                {
                    propBlock.SetColor("_BaseColor", Color.white);
                    propBlock.SetColor("_Color", Color.white);
                    ButtonRenderer.SetPropertyBlock(propBlock);
                    pressTime = 0f;
                }
            }
        }
    }
}
