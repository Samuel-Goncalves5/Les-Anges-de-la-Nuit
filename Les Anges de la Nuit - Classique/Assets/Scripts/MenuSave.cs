using UnityEngine;
using UnityEngine.UI;

public class MenuSave : MonoBehaviour
{
    public Text buttonText;
    public GameObject button;
    public Image image1;
    public Image image2;
    private void Start()
    {
        PlayerData save = SaveSystem.LoadPlayer(true);
        if (save?.general?[0] is null) return;
        buttonText.text = save.general[0];
        button.SetActive(true);
        image1.sprite = image2.sprite;
    }
}