using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    private void StartSpawn(Character c)
    {
        Vector3 Position;
        switch (c)
        {
            case Character.Elea : 
                Position = new Vector3(1,1,1);
                break;
            case Character.Eva :
                Position = new Vector3(1,1,1);
                break;
            case Character.Elena :
                Position = new Vector3(1,1,1);
                break;
            case Character.Emma :
                Position = new Vector3(1,1,1);
                break;
            default:
                Position = new Vector3(1,1,1);
                break;
        }
            
        PhotonNetwork.Instantiate(c.ToString(), Position, Quaternion.identity);
    }

    private void LoadSpawn(Character c, Dictionary<string, float[]> dico)
    {
        Vector3 Position = new Vector3(dico[c.ToString()][0], dico[c.ToString()][1], dico[c.ToString()][2]);
        PhotonNetwork.Instantiate(c.ToString(), Position, Quaternion.identity);
    }
    
    private void Start()
    {
        Character c = Selection.perso;
        if (!CreateAndJoinRooms.Load) StartSpawn(c);
        else
        {
            PlayerData data = SaveSystem.LoadPlayer();
            if (data is null) StartSpawn(c);
            else
            {
                Dictionary<string, float[]> dico = Dictionnaries.positions;
                if (dico.ContainsKey(c.ToString())) LoadSpawn(c, dico);
                else StartSpawn(c);
            }
        }
    }
}
