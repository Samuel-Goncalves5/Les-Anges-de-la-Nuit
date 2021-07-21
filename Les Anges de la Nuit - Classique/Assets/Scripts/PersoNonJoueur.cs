using Photon.Pun;
using UnityEngine;

public class PersoNonJoueur : MonoBehaviour
{
     public bool ACTIVATION;
     public string[] text;
     public GameObject Reference;
     private DialogueCanvas DialogueCanvas;

     private void Start()
     {
          DialogueCanvas = GameObject.Find("DialogueCanvas").GetComponent<DialogueCanvas>();
     }

     private void Update()
     {
          if (ACTIVATION)
          {
               UpdateActivation();
          }
          else
          {
               if (!Reference) 
               {Reference = (GameObject) PhotonNetwork.LocalPlayer.CustomProperties["Personnage"]; 
                    return;}
               if (Distance(Reference) > 3) 
                    return;
               if (DialogueCanvas.gameObject.activeSelf) 
                    return;

               Activate();
          }
     }

     private void Activate()
     {
          DialogueCanvas.gameObject.SetActive(true);
          
     }

     private void UpdateActivation()
     {
          DialogueCanvas.gameObject.SetActive(false);
     }
     
     private float Distance(GameObject g) => (transform.position - g.transform.position).sqrMagnitude;
}
