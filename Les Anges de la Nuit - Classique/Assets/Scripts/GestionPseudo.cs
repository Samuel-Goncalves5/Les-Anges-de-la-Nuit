using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GestionPseudo : MonoBehaviour
{
    public static Transform CameraLocalPlayer;
    public GameObject currentChar;
    public Player currentPlayer;
    public Text Text;

    private void Start()
    {
        currentPlayer = currentChar.GetComponent<PlayerController>().view.Controller;
    }

    private void Update()
    {
        if (!(currentPlayer is null)) Text.text = currentPlayer.NickName;
        Vector3 cameraPosition = CameraLocalPlayer.position;
        Vector3 position = transform.position;
        
        Vector3 temp = new Vector3(cameraPosition.x, position.y, cameraPosition.z);

        if (CameraLocalPlayer) transform.LookAt(temp);
    }
}
