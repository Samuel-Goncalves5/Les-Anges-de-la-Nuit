using System;
using UnityEngine;

public class PoidsGrappin : MonoBehaviour
{
    public bool launched;
    
    public Grappin Grappin;
    public float Vitesse;
    public Transform PositionDepart;
    public Vector3 PositionFin;
    public LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer.enabled = true;
    }

    private void Update()
    {
        lineRenderer.SetPosition(1, transform.position);
        if (launched) Bouger();
    }
    
    public void Bouger()
    {
        transform.position = Vector3.Lerp(transform.position, PositionFin, Vitesse * Time.deltaTime / Vector3.Distance(transform.position, PositionFin));
        lineRenderer.SetPosition(1, transform.position);
        
        if (Vector3.Distance(transform.position, PositionFin) < 1f)
        {
            Grappin.isMoving = true;
            Grappin.gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
    }
}
