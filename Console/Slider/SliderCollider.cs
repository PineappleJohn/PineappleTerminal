using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PineappleMod.Console.Slider
{
    public class SliderCollider : MonoBehaviour
    {
        public int myValue;

        public float debounceTime = 0.25f;

        public float touchTime;
        protected void OnTriggerEnter(Collider collider)
        {
            if (!base.enabled || !(touchTime + debounceTime < Time.time) || collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() == null)
            {
                return;
            }

            touchTime = Time.time;
            GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();

            SliderSetup.Instance.sldr.value = myValue;

            if (!(component == null))
            {
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(84, component.isLeftHand, 0.05f);
                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
            }
        }
    }
}
