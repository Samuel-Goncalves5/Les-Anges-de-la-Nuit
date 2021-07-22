using Photon.Pun;
using UnityEngine;

public class PersoNonJoueur : MonoBehaviour
{
     public bool ACTIVATION;
     public string[] text;
     private GameObject Reference;
     private DialogueCanvas DialogueCanvas;
     public float distance;
     public bool stopPlayer;
     private int index;

     private void Start()
     {
          DialogueCanvas = GameObject.Find("DialogueCanvas").GetComponent<DialogueCanvas>();
          Reference = (GameObject) PhotonNetwork.LocalPlayer.CustomProperties["Personnage"];
     }

     private void Update()
     {
          if (ACTIVATION)
          {
               if (Input.GetKeyDown(MenuInGame.Commands[8])) UpdateActivation();
          }
          else
          {
               if (!Reference) 
               {Reference = (GameObject) PhotonNetwork.LocalPlayer.CustomProperties["Personnage"]; 
                    return;}
               if (Distance(Reference) > distance) 
                    return;
               if (DialogueCanvas is null || DialogueCanvas.Image.gameObject.activeSelf) 
                    return;
               
               if (Input.GetKeyDown(MenuInGame.Commands[8])) Activate();
          }
     }

     private void Activate()
     {
          index = 0;
          DialogueCanvas.Text.text = text[index];
          DialogueCanvas.Image.gameObject.SetActive(true);
          index++;
          ACTIVATION = true;
          if (stopPlayer) PlayerController.STOPCONTROL = true;
     }

     private void UpdateActivation()
     {
          if (index == text.Length)
          {
               DialogueCanvas.Image.gameObject.SetActive(false);
               index = 0;
               ACTIVATION = false;
               if (stopPlayer) PlayerController.STOPCONTROL = false;
          }
          else
          {
               DialogueCanvas.Text.text = text[index];
               index++;
          }
     }
     
     private float Distance(GameObject g) => (transform.position - g.transform.position).sqrMagnitude;
}
