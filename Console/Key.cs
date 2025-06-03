using GorillaTag;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace PineappleMod.Console
{
    public class Key : MonoBehaviour
    {
        public string characterString;

        public Action<string> OnButtonPressedEvent;

        public float pressTime;

        public bool functionKey;

        public bool testClick;

        public bool repeatTestClick;

        public float repeatCooldown = 2f;

        public Renderer ButtonRenderer;

        public ButtonColorSettings ButtonColorSettings;

        private float lastTestClick;

        private MaterialPropertyBlock propBlock;

        private void Start()
        {
            if (ButtonRenderer == null)
            {
                ButtonRenderer = GetComponent<Renderer>();
            }

            propBlock = new MaterialPropertyBlock();
            pressTime = 0f;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (!(collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null))
            {
                return;
            }

            GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();
            
            PressButtonColourUpdate();
            if (component != null)
            {
                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, component.isLeftHand, 0.1f);
                OnButtonPressedEvent?.Invoke(characterString);
            }
        }

        public void PressButtonColourUpdate()
        {
            propBlock.SetColor("_BaseColor", ButtonColorSettings.PressedColor);
            propBlock.SetColor("_Color", ButtonColorSettings.PressedColor);
            ButtonRenderer.SetPropertyBlock(propBlock);
            pressTime = Time.time;
            StartCoroutine(ButtonColorUpdate_Local());
            IEnumerator ButtonColorUpdate_Local()
            {
                yield return new WaitForSeconds(ButtonColorSettings.PressedTime);
                if (pressTime != 0f && Time.time > ButtonColorSettings.PressedTime + pressTime)
                {
                    propBlock.SetColor("_BaseColor", ButtonColorSettings.UnpressedColor);
                    propBlock.SetColor("_Color", ButtonColorSettings.UnpressedColor);
                    ButtonRenderer.SetPropertyBlock(propBlock);
                    pressTime = 0f;
                }
            }
        }
    }
}
