using PineappleMod.Console;
using PineappleMod.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PineappleMod.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager instance;
        public GameObject menu;
        //protected void Start() => instance = this;

        protected void Start()
        {
            instance = this;
            Setup();
        }

        public void Setup()
        {
            menu = Plugin.Instance.console.transform.Find("Menu").gameObject;
            menu.transform.SetParent(GorillaTagger.Instance.leftHandTransform, false);


            var button = menu.transform.Find("KeyboardButton").AddComponent<Button>();
            var renderer = button.GetComponent<MeshRenderer>();
            renderer.SetMaterials(new List<Material>() {
                Plugin.Instance.pineappleBundle.LoadAsset<Material>("Materials/Material/m_Button") });
            renderer.material.shader = Shader.Find("GorillaTag/UberShader");


            var anotherRen = menu.GetComponent<MeshRenderer>();
            anotherRen.SetMaterials(new List<Material>() {
                Plugin.Instance.pineappleBundle.LoadAsset<Material>("Materials/Material/Black Material"), Plugin.Instance.pineappleBundle.LoadAsset<Material>("Materials/Material/m_Menu Outer") });
            anotherRen.materials[0].shader = Shader.Find("GorillaTag/UberShader");
            anotherRen.materials[1].shader = Shader.Find("GorillaTag/UberShader");


            button.onPress = (tr) => {
                ConsoleManager.Instance.console.SetActive(!ConsoleManager.Instance.console.activeSelf);
                ConsoleManager.Instance.keyboard.SetActive(!ConsoleManager.Instance.keyboard.activeSelf);
            };

            menu.transform.localScale = Plugin.Instance.getConsoleScale();
            menu.transform.rotation = Quaternion.Euler(0, 0, 0);

            GestureTracker.Instance.leftGrip.OnPressed += EnableMenu;
            GestureTracker.Instance.leftGrip.OnReleased += DisableMenu;
            Logging.Info("MenuManager started, menu initialized and button added.");
            menu.SetActive(false);
        }

        public void EnableMenu(InputTracker trakker)
        {
            ConsoleManager.Instance.console.SetActive(true);
            ConsoleManager.Instance.keyboard.SetActive(true);
        }

        public void DisableMenu(InputTracker takker)
        {
            ConsoleManager.Instance.console.SetActive(false);
            ConsoleManager.Instance.keyboard.SetActive(false);
        }
    }
}