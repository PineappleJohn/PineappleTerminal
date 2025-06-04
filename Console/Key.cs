using GorillaExtensions;
using GorillaTag;
using PineappleMod.Tools;
using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace PineappleMod.Console
{
    public class Key : MonoBehaviour // Decompiled from GorillaTag's code, please dont come after me.
    {
        public string characterString;

        public Action<string> OnButtonPressedEvent;

        public float pressTime;

        public bool functionKey;

        public bool testClick;

        public bool repeatTestClick;

        public float repeatCooldown = 2f;

        public Renderer ButtonRenderer;

        public ButtonColorSettings ButtonColorSettings = new ButtonColorSettings();

        private float lastTestClick;

        private MaterialPropertyBlock propBlock;

        public void Awake()
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

            gameObject.layer = LayerMask.NameToLayer("GorillaInteractible");
            gameObject.GetOrAddComponent<BoxCollider>().isTrigger = true;
        }

        protected void OnTriggerEnter(Collider collider)
        {
            GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();
            
            if (component != null)
            {
                PressButtonColourUpdate();
                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, component.isLeftHand, 0.1f);
                OnButtonPressedEvent?.Invoke(characterString);
            }
        }

        public void TestClick() {
            PressButtonColourUpdate();
            GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.1f);
            OnButtonPressedEvent?.Invoke(characterString);
        }

        public void PressButtonColourUpdate()
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
