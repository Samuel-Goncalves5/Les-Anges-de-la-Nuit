using UnityEngine;
using UnityEngine.UI;

public class MenuObjets : MonoBehaviour
{
    public GameObject GameObject;
    public Text Text;
    
    private void Update()
    {
        if (Input.GetKeyDown(MenuInGame.Commands[10]))
        {
            string Objets = "Objets :\n\n";
            foreach (Recuperable recuperable in PlayerController.Objets) {Objets += recuperable.nom + "\n";}
            Text.text = Objets;
            PlayerController.STOPCONTROL = !PlayerController.STOPCONTROL;
            GameObject.SetActive(!GameObject.activeSelf);
        }
    }
}
