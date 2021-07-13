using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string roomName;
    public string[] names;
    public float[] positionsSerializable;
    
    public PlayerData(List<PlayerController> players)
    {
        foreach (PlayerController player in players)
        {
            float[] position = new float[3];
            Vector3 p = player.transform.position;
            position[0] = p.x; position[1] = p.y; position[2] = p.z;

            if (Dictionnaries.positions.ContainsKey(player.playerName))
                Dictionnaries.positions.Remove(player.playerName);
            Dictionnaries.positions.Add(player.playerName, position);
        }

        roomName = PhotonNetwork.CurrentRoom.Name;
        
        names = new string[Dictionnaries.positions.Count];
        positionsSerializable = new float[Dictionnaries.positions.Count * 3];
        int i = 0;
        foreach (var keys in Dictionnaries.positions)
        {
            names[i] = keys.Key;
            positionsSerializable[3 * i] = keys.Value[0];
            positionsSerializable[3 * i + 1] = keys.Value[1];
            positionsSerializable[3 * i + 2] = keys.Value[2];

            i++;
        }
    }

    public void ReloadDico()
    {
        Dictionnaries.positions = new Dictionary<string, float[]>();
        
        for (int i = 0; i < names.Length; i++)
        {
            float[] floats = {positionsSerializable[3 * i], 
                              positionsSerializable[3 * i + 1], 
                              positionsSerializable[3 * i + 2]};

            Dictionnaries.positions.Add(names[i], floats);
        }
    }
}