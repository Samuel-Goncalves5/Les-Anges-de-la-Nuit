using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class KickOption : MonoBehaviour
{
    public GameObject kickButtonPrefab;
    
    private void Start() {Reload();}

    public void Reload()
    {
        for (int i = 0; i < transform.childCount; i++) {Destroy(transform.GetChild(i).gameObject);}
        
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.IsLocal) continue;
            GameObject kickButton = Instantiate(kickButtonPrefab, transform, true);
            kickButton.GetComponent<KickButton>().Player = p;
        }
    }
}