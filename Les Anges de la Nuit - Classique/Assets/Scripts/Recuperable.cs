using Photon.Pun;
using UnityEngine;

public class Recuperable : MonoBehaviour
{
    public string nom;
    [HideInInspector] public GameObject representation;
    private GameObject reference;
    public float distance;

    private float Distance(GameObject g) => (transform.position - g.transform.position).sqrMagnitude;
    
    private void Awake()
    {
        representation = gameObject;
    }

    private void Update()
    {
        if (reference is null) {reference = (GameObject) PhotonNetwork.LocalPlayer.CustomProperties["Personnage"];
            return;}
        if (Distance(reference) > distance) return;
        
        if (!PlayerController.STOPCONTROL && Input.GetKeyDown(MenuInGame.Commands[9])) Activate();
    }

    public void Activate()
    {
        PlayerController.Objets.Add(this);
        gameObject.SetActive(false);
    }
}