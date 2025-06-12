using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace PineappleMod.Console.Slider
{
    public class SliderSetup : MonoBehaviour
    {
        public static SliderSetup Instance;
        public GameObject sliderObject;
        public UnityEngine.UI.Slider sldr;

        public void Awake()
        {
            Instance = this;
            sliderObject = ConsoleManager.Instance.keyboard.transform.Find("Canvas").Find("Panel").Find("Slider").gameObject;
            sldr = sliderObject.GetComponent<UnityEngine.UI.Slider>();

            for (int i = 0; i < sldr.maxValue; i++)
            {
                sldr.value = i;
                var tempCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tempCollider.transform.SetParent(sliderObject.transform);
                tempCollider.transform.position = sldr.handleRect.position;
                tempCollider.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                tempCollider.name = $"SliderCollider_{i}";
                tempCollider.AddComponent<SliderCollider>().myValue = i;
            }
            sldr.value = 0;
        }
    }
}
