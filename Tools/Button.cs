using System;
using UnityEngine;

namespace PineappleMod.Tools
{
    public class Button : MonoBehaviour
    {
        public bool onlyGorillaHands = true;
        public Action<Transform> onPress;

        public void OnTriggerEnter(Collider c) {
            if (onlyGorillaHands && c.gameObject.layer == LayerMask.NameToLayer("Gorilla Hand")) {
                onPress?.Invoke(c.transform);
                return;
            } else if (!onlyGorillaHands)
            {
                onPress?.Invoke(c.transform);
            }
        }
    }
}
