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
        string roomName = createInput.text;
        PhotonNetwork.CreateRoom(roomName, new RoomOptions {MaxPlayers = 4});
        
        string input = nameField.text;
        PhotonNetwork.LocalPlayer.NickName = input;

        Hashtable h = PhotonNetwork.LocalPlayer.CustomProperties;
        h.Add("Character", "");
        PhotonNetwork.LocalPlayer.SetCustomProperties(h);
    }
    
    public void JoinRoom()
    {
        if (!PhotonNetwork.JoinRoom(joinInput.text))
        {
            //Pas de salle de ce nom
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = nameField.text;
            
            Hashtable h = PhotonNetwork.LocalPlayer.CustomProperties;
            h.Add("Character", "");
            PhotonNetwork.LocalPlayer.SetCustomProperties(h);
        }
    }

    public void RecreateRoom()
    {
        Load = true;
        
        PlayerData data = SaveSystem.LoadPlayer();
        string roomName = data.general[0];
        PhotonNetwork.CreateRoom(roomName, new RoomOptions {MaxPlayers = 4});
        
        string input = nameField.text;
        PhotonNetwork.LocalPlayer.NickName = input == "" ? data.general[1] : input;

        Hashtable h = PhotonNetwork.LocalPlayer.CustomProperties;
        h.Add("Character", "");
        PhotonNetwork.LocalPlayer.SetCustomProperties(h);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Sélection"); // Charge une scène multijoueur
    }
}