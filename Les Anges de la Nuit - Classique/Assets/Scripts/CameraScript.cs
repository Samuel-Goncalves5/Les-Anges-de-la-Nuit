using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Range(1,2)] public int modele;
    public Transform controller1;
    public Transform controller2;
    [Range(-180,180)]public float controller1Value;
    [Range(-130,-50)]public float controller2Value;

    void Update()
    {
        if (modele == 1)
        {
            controller1.localRotation = Quaternion.Euler(0, controller1Value, 0);
        }
        else
        {
            controller1.localRotation = Quaternion.Euler(0, 180, controller1Value);
            controller2.localRotation = Quaternion.Euler(0, 90, controller2Value);
        }
    }
}
