using System.Globalization;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    private void StartSpawn(string c)
    {
        Vector3 position;
        
        switch (c)
        {
            case "Elea"  : position = new Vector3(-15,1,2); break;
            case "Eva"   : position = new Vector3(-21,9,4); break;
            case "Elena" : position = new Vector3(22,9,22); break;
            case "Emma"  : position = new Vector3(13,1,12); break;
            default      : position = new Vector3(1,1,1); break;
        }
        
        Hashtable h = PhotonNetwork.LocalPlayer.CustomProperties;
        GameObject g = PhotonNetwork.Instantiate(c, position, Quaternion.identity);
        h.Add("Personnage", g);
    }

    private void LoadSpawn(string c, Vector3 position, Quaternion rotation)
    {
        Hashtable h = PhotonNetwork.LocalPlayer.CustomProperties;
        GameObject g = PhotonNetwork.Instantiate(c, position, rotation);
        h.Add("Personnage", g);
    }
    
    private void Start()
    {
        string c = (string) PhotonNetwork.LocalPlayer.CustomProperties["Character"];
        if (CreateAndJoinRooms.Load)
        {
            PlayerData data = SaveSystem.LoadPlayer();
            if (data is null || !SaveSystem.IsInitialized(data, c)) StartSpawn(c);
            else
            {
                string[] infos = SaveSystem.GetInfos(data, c);
                Vector3 position = 
                    new Vector3(
                        float.Parse(infos[0], CultureInfo.InvariantCulture), 
                        float.Parse(infos[1], CultureInfo.InvariantCulture), 
                        float.Parse(infos[2], CultureInfo.InvariantCulture));
                Quaternion rotation =
                    new Quaternion(
                        float.Parse(infos[3], CultureInfo.InvariantCulture), 
                        float.Parse(infos[4], CultureInfo.InvariantCulture), 
                        float.Parse(infos[5], CultureInfo.InvariantCulture), 
                        float.Parse(infos[6], CultureInfo.InvariantCulture));
                
                LoadSpawn(c, position, rotation);
            }
        }
        else StartSpawn(c);
    }
}
