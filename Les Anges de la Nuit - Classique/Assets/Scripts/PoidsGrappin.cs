using Photon.Pun;
using UnityEngine;

public class PoidsGrappin : MonoBehaviour
{
    public Grappin Grappin;
    public float Vitesse;
    public Vector3 PositionFin;
    public LineRenderer lineRenderer;
    private static Rigidbody Rigidbody;
    private Transform PositionDepart;
    
    private void Start()
    {
        string s = gameObject.name;
        Grappin = GameObject.Find(s.Split('_')[1]).GetComponent<Grappin>();

        PositionDepart =
            Grappin == ((GameObject) PhotonNetwork.LocalPlayer.CustomProperties["Personnage"]).GetComponent<Grappin>()? 
                Grappin.PositionDepartPerso.transform : Grappin.PositionDepart;
        
        PositionFin = Grappin.PositionFin;
        transform.LookAt(Grappin.transform);
    }

    private void Update()
    {
        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.SetPosition(0, PositionDepart.position);
        lineRenderer.enabled = true;
        Bouger();
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
