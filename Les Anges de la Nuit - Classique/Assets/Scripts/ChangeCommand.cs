using Enum = System.Enum;
using System.Collections;
using UnityEngine;

public class ChangeCommand : MonoBehaviour
{
    public int index;
    private MenuInGame _menuInGame;
    
    private void Start()
    {
        _menuInGame = transform.parent.parent.GetComponent<MenuInGame>();
    }

    public void buttonChange() => StartCoroutine(ChangeCommandRoutine());
    public void buttonReset() => _menuInGame.ResetControl(index);

    IEnumerator ChangeCommandRoutine()
    {
        yield return new WaitUntil(() => Input.anyKeyDown);

        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode))) 
            if(Input.GetKey(key)) 
                _menuInGame.ChangeControl(index, key);
    }
}
