using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Selection : MonoBehaviourPunCallbacks
{
    Dictionary<string, Player> sélection = new Dictionary<string, Player>();

    public GameObject LaunchButton;
    public GameObject Kick;
    public GameObject KickMenu;
    public Text List;

    private void Start()
    {
        StartCoroutine(ReloadRoutine());

        if (!PhotonNetwork.IsMasterClient && PhotonNetwork.MasterClient.CustomProperties.ContainsKey("Save"))
        {
            SaveSystem.Sauvegarde = (PlayerData) PhotonNetwork.MasterClient.CustomProperties["Save"];
            CreateAndJoinRooms.Load = true;
        }
        else if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LocalPlayer.CustomProperties.Add("Save", SaveSystem.LoadPlayer());
    }

    IEnumerator ReloadRoutine()
    {
        try
        {
            sélection.Clear();

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                string s = (string) p.CustomProperties["Character"];
                if (s != "") sélection.Add(s, p);
            }

            Kick.SetActive(PhotonNetwork.IsMasterClient);
        }
        catch (System.NullReferenceException) {yield break;}

        yield return new WaitForSeconds(5);
        StartCoroutine(ReloadRoutine());
    }

    private void Update()
    {
        LaunchButton.SetActive(PhotonNetwork.PlayerList.Length == sélection.Count);

        if (!PhotonNetwork.InRoom) return;
        
        string s = PhotonNetwork.CurrentRoom.Name + " :\n" + PhotonNetwork.CurrentRoom.PlayerCount;
        s += " joueur";
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2) s += "s";
        s += " :\n";

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            s += p.NickName;
            string property = (string) p.CustomProperties["Character"];
            if (property != "") s += " : " + property;
            s += "\n";
        }
        
        List.text = s;
    }

    [PunRPC]
    public void GlobalLaunch()
    {
        Chargement.On();
        PhotonNetwork.CurrentRoom.MaxPlayers = (byte) PhotonNetwork.PlayerList.Length;
        PhotonNetwork.LoadLevel("Jeu");
    }

    public void Launch()
    {
        gameObject.GetComponent<PhotonView>().RPC("GlobalLaunch", RpcTarget.All);
    }

    [PunRPC]
    void Choose(Player p, string c)
    {
        foreach (var k in sélection)
            if (Equals(k.Value, p))
            {
                sélection.Remove(k.Key);
                break;
            }
        sélection.Add(c, p);
    }

    [PunRPC]
    void DeChoose(string c)
    {
        sélection.Remove(c);
    }

    void SelectOne(string c)
    {
        if (sélection.ContainsKey(c))
        {
            if (sélection[c].IsLocal)
            {
                DeSelectOne();
                return;
            }
                
            else
            {
                Debug.Log("Ce perso a déjà été choisi par un autre joueur");
                return;
            }
        }
        
        gameObject.GetComponent<PhotonView>().RPC(
            "Choose", RpcTarget.All, PhotonNetwork.LocalPlayer, c);
        

        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable {{"Character", c}});
    }

    public void DeSelectOne()
    {
        string c = (string) PhotonNetwork.LocalPlayer.CustomProperties["Character"];

        gameObject.GetComponent<PhotonView>().RPC("DeChoose", RpcTarget.All, c);

        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable {{"Character", ""}});
    }

    public void Emma() => SelectOne("Emma");
    public void Elea() => SelectOne("Elea");
    public void Eva() => SelectOne("Eva");
    public void Elena() => SelectOne("Elena");
    public void Quitter()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        SceneManager.LoadScene("Menu");
    }

    public void KickButton() => KickMenu.SetActive(!KickMenu.activeSelf);
}