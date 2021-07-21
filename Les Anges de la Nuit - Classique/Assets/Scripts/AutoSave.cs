using System.Collections;
using Photon.Pun;
using UnityEngine;

public class AutoSave : MonoBehaviour
{
    public float secondsForAutoSave;
    
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient) StartCoroutine(SaveRoutine());
    }

    private IEnumerator SaveRoutine()
    {
        SaveSystem.SaveGame();
        yield return new WaitForSeconds(secondsForAutoSave);
        StartCoroutine(SaveRoutine());
    }
}
