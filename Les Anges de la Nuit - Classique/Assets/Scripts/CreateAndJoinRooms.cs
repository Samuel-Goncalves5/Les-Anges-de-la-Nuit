using ExitGames.Client.Photon;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    public Text nameField;
    public static bool Load;

    public void CreateRoom()
    { 
        PhotonNetwork.CreateRoom(createInput.text, new RoomOptions {MaxPlayers = 4});
    }
    
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public void RecreateRoom()
    {
        Load = true;
        
        PlayerData data = SaveSystem.LoadPlayer();
        string roomName = data.general[0];
        PhotonNetwork.CreateRoom(roomName, new RoomOptions {MaxPlayers = 4});
        
        string input = nameField.text;
        PhotonNetwork.LocalPlayer.NickName = input == "" ? data.general[1] : input;
    }

    public override void OnJoinedRoom()
    {
        if (!Load) PhotonNetwork.LocalPlayer.NickName = nameField.text;
        Hashtable h = PhotonNetwork.LocalPlayer.CustomProperties;
        h.Add("Character", "");
        PhotonNetwork.LocalPlayer.SetCustomProperties(h);
        
        PhotonNetwork.LoadLevel("Sélection"); // Charge une scène multijoueur
    }
}