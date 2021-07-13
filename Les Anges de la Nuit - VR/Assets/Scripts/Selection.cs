using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public enum Character { Emma, Elea, Elena, Eva }

public class Selection : MonoBehaviour
{
    public static List<string> sélection = new List<string>();
    public static Character perso;

    public GameObject ButtonEmma;
    public GameObject ButtonElea;
    public GameObject ButtonElena;
    public GameObject ButtonEva;
    public GameObject WaitZone;

    private void Update()
    {
        if (PhotonNetwork.PlayerList.Length == sélection.Count)
            PhotonNetwork.LoadLevel("Jeu");
    }

    [PunRPC]
    void Choose(Character c)
    {
        sélection.Add(c.ToString());

        switch (c)
        {
            case Character.Elea :
                ButtonElea.SetActive(false);
                break;
            case Character.Eva :
                ButtonEva.SetActive(false);
                break;
            case Character.Elena :
                ButtonElena.SetActive(false);
                break;
            case Character.Emma :
                ButtonEmma.SetActive(false);
                break;
        }
        
    }

    void SelectOne(Character c)
    {
        gameObject.GetComponent<PhotonView>().RPC("Choose", RpcTarget.All, c);
        
        perso = c;
        
        ButtonElea.SetActive(false);
        ButtonEva.SetActive(false);
        ButtonEmma.SetActive(false);
        ButtonElena.SetActive(false);
        WaitZone.SetActive(true);
    }
    
    public void Emma()
    {
        SelectOne(Character.Emma);
    }

    public void Elea()
    {
        SelectOne(Character.Elea);
    }

    public void Eva()
    {
        SelectOne(Character.Eva);
    }

    public void Elena()
    {   
        SelectOne(Character.Elena);
    }
}
