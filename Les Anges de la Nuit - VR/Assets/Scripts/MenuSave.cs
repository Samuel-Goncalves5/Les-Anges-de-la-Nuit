using UnityEngine;
using UnityEngine.UI;

public class MenuSave : MonoBehaviour
{
    public Text buttonText;
    public GameObject button;
    public GameObject text;

    private PlayerData save;
    
    private void Start()
    {
        save = SaveSystem.LoadPlayer();
        if (save is null) return;
        buttonText.text = save.roomName;
        button.SetActive(true);
        text.SetActive(true);
    }
}
