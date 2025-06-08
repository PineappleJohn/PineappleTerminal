using GorillaExtensions;
using GorillaLocomotion.Climbing;
using GorillaTag;
using Photon.Pun;
using PineappleMod.Tools;
using System;
using System.Collections;
using UnityEngine;

namespace PineappleMod.Console
{
    public class Key : MonoBehaviour
    {
        public Action<Key, string> OnPressed;
        public Renderer ButtonRenderer;

        public ButtonColorSettings ButtonColorSettings = new ButtonColorSettings();

        public float debounceTime = 0.25f;

        public float touchTime;

        private MaterialPropertyBlock propBlock;
        public string value;

        public virtual void Awake()
        {
            if (gameObject.name != "Space")
                value = gameObject.name;
            else
                value = " ";

            ButtonColorSettings.PressedColor = Color.red;
            ButtonColorSettings.UnpressedColor = Plugin.Instance.pineappleBundle.LoadAsset<Material>("m_Button").color;
            ButtonColorSettings.PressedTime = 0.2f;

            if (ButtonRenderer == null)
            {
                ButtonRenderer = GetComponent<Renderer>();
            }

            propBlock = new MaterialPropertyBlock();
        }

        protected void OnTriggerEnter(Collider collider)
        {
            if (!base.enabled || !(touchTime + debounceTime < Time.time) || collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() == null)
            {
                return;
            }

            touchTime = Time.time;
            GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();
            ButtonActivationWithHand(component.isLeftHand);
            if (!(component == null))
            {
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, component.isLeftHand, 0.05f);
                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
            }
        }

        public void ButtonActivationWithHand(bool isLeftHand)
        {
            PressButtonColourUpdate();
            OnPressed?.Invoke(this, value);
        }
        public virtual void TestClick()
        {
            GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.1f);
            OnPressed?.Invoke(this, value);
        }

        float pressTime = 0f;
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
                    propBlock.SetColor("_BaseColor", ButtonColorSettings.UnpressedColor);
                    propBlock.SetColor("_Color", ButtonColorSettings.UnpressedColor);
                    ButtonRenderer.SetPropertyBlock(propBlock);
                    pressTime = 0f;
                }
            }
        }
    }
}
//[Info   :PineappleMod] (Key.OnTriggerEnter()) GorillaHandClimber (UnityEngine.SphereCollider) GorillaHandClimber