using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using PineappleMod.Console;
using PineappleMod.Tools;
using UnityEngine;

using Player = GorillaLocomotion.GTPlayer;

namespace PineappleMod.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager instance;
        Vector3 savedVelocity;
        bool menuon = false;

        protected void Start()
        {
            instance = this;
            Setup();
        }

        public void Setup()
        {
            Destroy(Plugin.Instance.console.transform.Find("Menu").gameObject);
            GestureTracker.Instance.rightSecondary.OnPressed += ToggleMenu;
        }

        public void ToggleMenu(InputTracker t)
        {
            if (ConsoleManager.Instance.console.activeSelf)
            {
                DisableMenu();
            }
            else
            {
                EnableMenu();
            }
        }

        public void Update()
        {
            if (menuon)
            {
                var rb = Player.Instance.bodyCollider.attachedRigidbody;
                rb.AddForce(-UnityEngine.Physics.gravity * rb.mass * Player.Instance.scale);
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 5);
            }
        }

        public void EnableMenu()
        {
            if (NetworkSystem.Instance.GameModeString.Contains("MODDED_"))
            {
                menuon = true;
                savedVelocity = Player.Instance.bodyCollider.attachedRigidbody.velocity;
                Player.Instance.bodyCollider.attachedRigidbody.velocity = Vector3.zero;
            }
            ConsoleManager.Instance.console.SetActive(true);
            ConsoleManager.Instance.keyboard.SetActive(true);

            PhotonNetwork.LocalPlayer.CustomProperties.AddOrUpdate("PineappleTerminal", true);
        }

        public void DisableMenu()
        {
            if (NetworkSystem.Instance.GameModeString.Contains("MODDED_"))
            {
                menuon = false;
                Player.Instance.bodyCollider.attachedRigidbody.AddForce(savedVelocity, ForceMode.VelocityChange);
            }
            ConsoleManager.Instance.console.SetActive(false);
            ConsoleManager.Instance.keyboard.SetActive(false);

            PhotonNetwork.LocalPlayer.CustomProperties.AddOrUpdate("PineappleTerminal", false);
        }
    }
}