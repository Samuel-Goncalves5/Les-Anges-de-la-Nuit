using Photon.Pun;
using UnityEngine;

public class GestionPseudo : MonoBehaviour
{
    public Canvas Canvas;
    private Transform LocalPlayer;

    private void Start()
    {
        LocalPlayer = ((GameObject) PhotonNetwork.LocalPlayer.CustomProperties["Personnage"]).transform;
    }

    private void Update()
    {
        Canvas.transform.LookAt(LocalPlayer);
    }
}
