using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    private void Start()
    {
        Vector3 Position = new Vector3(Random.Range(-15,15), 2, Random.Range(-15,15));
        PhotonNetwork.Instantiate(playerPrefab.name, Position, Quaternion.identity);
    }
}
