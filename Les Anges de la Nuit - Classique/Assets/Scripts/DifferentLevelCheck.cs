using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class DifferentLevelCheck : MonoBehaviour
{
    private void Update()
    {
        string s = (string) PhotonNetwork.LocalPlayer.CustomProperties["Level"];

        foreach (Player p in PhotonNetwork.PlayerListOthers)
        {
            GameObject g = (GameObject) p.CustomProperties["Personnage"];
            bool b = (string) p.CustomProperties["Level"] == s;
            g.SetActive(b);
        }
    }
}
