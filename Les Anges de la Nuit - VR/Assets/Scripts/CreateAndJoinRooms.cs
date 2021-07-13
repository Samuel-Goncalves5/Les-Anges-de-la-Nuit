using UnityEngine.UI;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    public static bool Load;

    public void CreateRoom()
    {
        string name = Load ? SaveSystem.LoadPlayer().roomName : createInput.text;
        PhotonNetwork.CreateRoom(name);
    }
    
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public void RecreateRoom()
    {
        Load = true;
        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Sélection"); // Charge une scène multijoueur
    }
}
