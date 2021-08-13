using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScene : MonoBehaviour
{
    public string levelName;
    public float distance;
    private GameObject g;
    private PlayerController p;

    private void Update()
    {
        var h = PhotonNetwork.LocalPlayer.CustomProperties;
        if (g is null) {g = (GameObject) h["Personnage"]; return;}
        if (p is null) {p = g.GetComponent<PlayerController>(); return;}
        if (Vector3.Distance(p.transform.position, transform.position) > distance) return;
        if (!Input.GetKeyDown(MenuInGame.Commands[8])) return;
        
        if (!h.ContainsKey("Level")) h.Add("Level", levelName);
        h["Level"] = levelName;
        SceneManager.LoadScene(levelName);
    }
}
