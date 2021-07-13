using System.Collections;
using UnityEngine;

public class AutoSave : MonoBehaviour
{
    public float secondsForAutoSave;
    
    private void Start()
    {
        StartCoroutine(SaveRoutine());
    }

    private IEnumerator SaveRoutine()
    {
        SaveSystem.SavePlayer();
        yield return new WaitForSeconds(secondsForAutoSave);
        StartCoroutine(SaveRoutine());
    }
}
