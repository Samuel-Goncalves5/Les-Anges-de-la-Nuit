using System.Collections;
using UnityEngine;

public class PoidsGrappin : MonoBehaviour
{
    public bool launched;
    
    public Grappin Grappin;
    public float Vitesse;
    public Vector3 PositionFin;
    public LineRenderer lineRenderer;
    private static Rigidbody Rigidbody;

    private bool preparation = true;

    private void Update()
    {
        lineRenderer.SetPosition(1, transform.position);
        if (!launched) return;
        if (preparation) StartCoroutine(Preparation());
        lineRenderer.enabled = true;
        Bouger();
    }

    private IEnumerator Preparation()
    {
        preparation = false;
        yield return new WaitForSeconds(0.001f);
        transform.LookAt(Grappin.transform);
    }
    
    private void Bouger()
    {
        transform.position = Vector3.Lerp(transform.position, PositionFin, Vitesse * Time.deltaTime / Vector3.Distance(transform.position, PositionFin));
        lineRenderer.SetPosition(1, transform.position);

        if (Vector3.Distance(transform.position, PositionFin) < 1f)
        {
            Grappin.isMoving = true;
            if (Rigidbody is null) Rigidbody = Grappin.gameObject.GetComponent<Rigidbody>();
            Rigidbody.useGravity = false;
        }
    }
}
