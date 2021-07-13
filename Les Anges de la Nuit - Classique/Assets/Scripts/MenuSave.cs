using UnityEngine;
using UnityEngine.UI;

public class MenuSave : MonoBehaviour
{
    public Text buttonText;
    public GameObject button;
    public GameObject text;
    private void Start()
    {
        PlayerData save = SaveSystem.LoadPlayer();
        if (save?.general?[0] is null) return;
        buttonText.text = save.general[0];
        button.SetActive(true);
        text.SetActive(true);
    }
}