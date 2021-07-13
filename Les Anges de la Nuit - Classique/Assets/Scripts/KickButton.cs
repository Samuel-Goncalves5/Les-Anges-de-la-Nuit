using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class KickButton : MonoBehaviour
{
    public Text text;
    public Player Player = null;
    public bool end;

    private void Update()
    {
        if (end || Player is null) return;
        text.text = Player.NickName;
        end = true;
    }
    
    public void Ban() => PhotonNetwork.CloseConnection(Player);
}
