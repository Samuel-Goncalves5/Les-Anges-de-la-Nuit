using UnityEngine.SceneManagement;
using Photon.Pun;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Connection au server
    }

    public override void OnConnectedToMaster()
    { // Quand la connection est réussie
        PhotonNetwork.JoinLobby(); // Permet de rejoindre / créer un lobby
    }

    public override void OnJoinedLobby()
    { // Quand un lobby est rejoint
        SceneManager.LoadScene("Menu");
    }
}
