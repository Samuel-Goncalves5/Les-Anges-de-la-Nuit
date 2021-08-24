using UnityEngine;

public class CameraDetection : MonoBehaviour
{
    private PlayerController pc;

    private void Awake() => pc = PlayerController.local;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>() == pc)
            Debug.Log(gameObject.name + " : Détection du joueur");
    }
}
