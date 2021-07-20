using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInGame : MonoBehaviour
{
    public GameObject MenuControles  ;
    public GameObject GeneralMenu    ;
    public static KeyCode[] Commands ;
    public string[] names            ;
    public KeyCode[] OriginalCommands;
    public GameObject text           ;
    public GameObject button         ;
    public GameObject reset          ;
    
    private List<Text> buttonTexts = new List<Text>();

    private void Start()
    {
        Commands = new KeyCode[OriginalCommands.Length];
        for (int k = 0; k < Commands.Length; k++) {ResetControl(k);}
        
        int i = 0; int j = 0;
        
        for (; i < Commands.Length; i++)
        {
            GameObject r = Instantiate(reset, MenuControles.transform);
            r.GetComponent<ChangeCommand>().index = i;
            
            GameObject b = Instantiate(button, MenuControles.transform);
            var temp = b.transform.GetChild(0).GetComponent<Text>();
            temp.text = names[i] + " :";
            buttonTexts.Add(temp);
            b.transform.GetChild(0).GetComponent<Text>().text = Commands[i].ToString();
            b.GetComponent<ChangeCommand>().index = i;
            
            GameObject t = Instantiate(text, MenuControles.transform);
            t.GetComponent<Text>().text = names[i] + " :";
            
            RectTransform rect = t.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.05f + 0.5f * (i%2), 0.9f - 0.1f * (i/2));
            rect.anchorMax = new Vector2(0.3f  + 0.5f * (i%2), 1    - 0.1f * (i/2));
            
            rect = b.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.3f  + 0.5f * (i%2), 0.9f - 0.1f * (i/2));
            rect.anchorMax = new Vector2(0.4f  + 0.5f * (i%2), 1    - 0.1f * (i/2));
            
            rect = r.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.4f  + 0.5f * (i%2), 0.9f - 0.1f * (i/2));
            rect.anchorMax = new Vector2(0.5f  + 0.5f * (i%2), 1    - 0.1f * (i/2));
        }
    }

    void Update()
    {
        KeyCode boutonMenu = Commands[4];
        
        if (!GeneralMenu.activeSelf) MenuControles.SetActive(false);
        if (Input.GetKeyDown(boutonMenu)) GeneralMenu.SetActive(!GeneralMenu.activeSelf);

        for (int i = 0; i < buttonTexts.Count; i++) buttonTexts[i].text = Commands[i].ToString();
    }

    public void MenuClose() {MenuControles.SetActive(false); GeneralMenu.SetActive(false);}
    public void MenuOpen() => MenuControles.SetActive(!MenuControles.activeSelf);
    public void ChangeControl(int index, KeyCode newKeyCode) => Commands[index] = newKeyCode;
    public void ResetControl(int index) => Commands[index] = OriginalCommands[index];
}