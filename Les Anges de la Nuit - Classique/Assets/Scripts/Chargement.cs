using UnityEngine;
public class Chargement : MonoBehaviour
{
    static GameObject _menuChargement;
    void Start()
    {
        _menuChargement = gameObject;
        _menuChargement.SetActive(false);
    }
    public static void On() => _menuChargement.SetActive(true);
}