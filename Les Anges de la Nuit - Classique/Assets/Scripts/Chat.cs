using UnityEngine;

public class Chat : MonoBehaviour
{
    public Animator Animator;
    Vector3 PreviousFramePosition;
    
    private int HashVitesse;

    private void Start()
    {
        HashVitesse = Animator.StringToHash("Vitesse");
        PreviousFramePosition = transform.position;
    }

    private void Update()
    {
        Vector3 pos = transform.position;
        float Move = Vector3.Distance(PreviousFramePosition, pos);
        float Speed = Move / Time.deltaTime;
        PreviousFramePosition = pos;
        
        Animator.SetFloat(HashVitesse, Speed);
    }
}
