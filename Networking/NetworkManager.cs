using PineappleMod.Console;
using System.Collections.Generic;
using UnityEngine;

namespace PineappleMod.Networking
{
    public class NetworkManager : MonoBehaviour
    {
        public static NetworkManager Instance { get; private set; }

        public Dictionary<VRRig, GameObject[]> playerTerminals = new Dictionary<VRRig, GameObject[]>();
        public void Awake() 
        {
            Instance = this;
            NetworkSystem.Instance.OnPlayerJoined += OnPlayerEnteredRoom;
            NetworkSystem.Instance.OnPlayerLeft += OnPlayerLeftRoom;
            NetworkSystem.Instance.OnReturnedToSinglePlayer += OnLeftRoom;
            NetworkSystem.Instance.OnMultiplayerStarted += OnJoinedRoom;

            InvokeRepeating(nameof(CheckAllTerminals), 1f, 1f);
        }

        public void CheckAllTerminals() 
        {
            if (!NetworkSystem.Instance.InRoom || playerTerminals.Count == 0) return;

            foreach (var kvp in playerTerminals)
            {
                kvp.Value[0].SetActive(kvp.Key.OwningNetPlayer.GetPlayerRef().CustomProperties["PineappleConsole"].ToString() == "true");
                kvp.Value[1].SetActive(kvp.Key.OwningNetPlayer.GetPlayerRef().CustomProperties["PineappleConsole"].ToString() == "true");
            }
        }


        public void OnPlayerEnteredRoom(NetPlayer plr)
        {
            if (plr.GetPlayerRef().CustomProperties.ContainsKey("PineappleTerminal"))
            {
                GorillaParent.instance.vrrigDict.TryGetValue(plr, out VRRig rig);
                if (rig.isLocal) return;
                var term = new GameObject[] {
                        Instantiate(ConsoleManager.Instance.console, rig.bodyTransform),
                        Instantiate(KeyboardNoKeys(), rig.bodyTransform)
                    };
                term[0].transform.localPosition = new Vector3(0, 0.25f, 0.7782f);
                term[0].transform.localRotation = Quaternion.Euler(270, 270, 0);
                term[0].transform.localScale = new Vector3(1.895491f, 24.84053f, 13.98671f);

                term[1].transform.localPosition = new Vector3(0, -0.05f, 0.5782f);
                term[1].transform.localRotation = Quaternion.Euler(300.132111f, 180f, 90f);
                term[1].transform.localScale = new Vector3(13.2667866f, 21.2343521f, 2.71945715f);
                playerTerminals.Add(rig, term);
            }
        }
        public void OnPlayerLeftRoom(NetPlayer plr)
        {
            if (plr.GetPlayerRef().CustomProperties.ContainsKey("PineappleTerminal"))
            {
                GorillaParent.instance.vrrigDict.TryGetValue(plr, out VRRig rig);
                if (rig.isLocal) return;
                playerTerminals.TryGetValue(rig, out GameObject[] term);
                playerTerminals.Remove(rig);
                if (term != null && term.Length > 0)
                {
                    Destroy(term[0]);
                    Destroy(term[1]);
                }
            }
        }
        public void OnLeftRoom()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig.isLocal) return;
                if (rig.OwningNetPlayer.GetPlayerRef().CustomProperties.ContainsKey("PineappleTerminal"))
                {
                    playerTerminals.TryGetValue(rig, out GameObject[] term);
                    playerTerminals.Remove(rig);
                    if (term != null && term.Length > 0)
                    {
                        Destroy(term[0]);
                        Destroy(term[1]);
                    }
                }
            }
        }
        public void OnJoinedRoom()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig.isLocal) return;
                if (rig.OwningNetPlayer.GetPlayerRef().CustomProperties.ContainsKey("PineappleTerminal"))
                {
                    var term = new GameObject[] {
                        Instantiate(ConsoleManager.Instance.console, rig.bodyTransform),
                        Instantiate(KeyboardNoKeys(), rig.bodyTransform)
                    };
                    term[0].transform.localPosition = new Vector3(0, 0.25f, 0.7782f);
                    term[0].transform.localRotation = Quaternion.Euler(270, 270, 0);
                    term[0].transform.localScale = new Vector3(1.895491f, 24.84053f, 13.98671f);

                    term[1].transform.localPosition = new Vector3(0, -0.05f, 0.5782f);
                    term[1].transform.localRotation = Quaternion.Euler(300.132111f, 180f, 90f);
                    term[1].transform.localScale = new Vector3(13.2667866f, 21.2343521f, 2.71945715f);
                    playerTerminals.Add(rig, term);
                }
            }
        }

        public GameObject KeyboardNoKeys() 
        {
            var keyboard = Instantiate(ConsoleManager.Instance.keyboard);

            foreach (Transform child in keyboard.transform)
            {
                if (child.TryGetComponent<Key>(out Key k))
                {
                    Destroy(k);
                }
            }

            return keyboard;
        }
    }
}
