using Photon.Pun;
using UnityEngine;

public class Grappin : MonoBehaviour
{
    public static bool MODEGRAPPIN;
    public GameObject PoidsGrappin;
    private PoidsGrappin pg;

    public CapsuleCollider NormalCollider;
    public CapsuleCollider GrappinCollider;
    
    public GameObject Cinemachine1;
    public GameObject Cinemachine2;
    
    public Camera Camera;
    public Camera GrappinCamera;
    private RaycastHit RaycastHit;
    public int maxDistance;
    public bool isMoving;
    public Vector3 PositionFin;
    public float vitesseGrappin;
    public Transform PositionDepart;
    
    private void Update()
    {
        if (isMoving && GrappinCollider.isTrigger)
        {
            NormalCollider.isTrigger = true;
            GrappinCollider.isTrigger = false;
        }
        
        if (!isMoving && NormalCollider.isTrigger)
        {
            NormalCollider.isTrigger = false;
            GrappinCollider.isTrigger = true;
        }
        
        try
        {
            pg.lineRenderer.SetPosition(0, PositionDepart.position);
        }
        catch (System.Exception)
        {
            isMoving = false;
        }
        
        
        if (Input.GetKeyDown(MenuInGame.Commands[6])) Tirer();

        if (isMoving) Bouger();

        if (Input.GetKeyDown(MenuInGame.Commands[11]))
        {
            if (isMoving)
            {
                isMoving = false;
                PlayerController.STOPCONTROL = false;
                gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
            else
            {
                try
                {
                    if (!(PoidsGrappin is null) && PoidsGrappin.activeSelf) return;
                }
                catch (UnassignedReferenceException) {}

                Camera.gameObject.SetActive(true);
                Cinemachine1.SetActive(true);
                GrappinCamera.gameObject.SetActive(false);
                Cinemachine2.SetActive(false);
                MODEGRAPPIN = false;
                PositionDepart.parent.gameObject.SetActive(false);
            }
        }
    }

    private void Tirer()
    {
        if (GrappinCamera.gameObject.activeSelf)
        {
            if (Physics.Raycast(GrappinCamera.transform.position, GrappinCamera.transform.forward, out RaycastHit, maxDistance))
            {
                PositionFin = RaycastHit.point;
                
                isMoving = false;
                PlayerController.STOPCONTROL = false;
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                if (!(PoidsGrappin is null))Destroy(PoidsGrappin);
                PoidsGrappin = PhotonNetwork.Instantiate("Grappin", transform.position, transform.rotation);
                pg = PoidsGrappin.GetComponent<PoidsGrappin>();
                pg.lineRenderer.SetPosition(0, PositionDepart.position);
                pg.Grappin = this;
                pg.PositionFin = PositionFin;
                pg.launched = true;
                
                PlayerController.STOPCONTROL = true;
            }
        }
        else
        {
            Camera.gameObject.SetActive(false);
            Cinemachine1.SetActive(false);
            GrappinCamera.gameObject.SetActive(true);
            Cinemachine2.SetActive(true);
            MODEGRAPPIN = true;
            PositionDepart.parent.gameObject.SetActive(true);
        }
    }

    private void Bouger()
    {
        transform.position = Vector3.Lerp(transform.position, PositionFin, vitesseGrappin * Time.deltaTime / Vector3.Distance(transform.position, PositionFin));

        if (Vector3.Distance(transform.position, PositionFin) < 1f)
        {
            isMoving = false;
            PlayerController.STOPCONTROL = false;
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            Destroy(PoidsGrappin);
            PoidsGrappin = null;
        }
    }
}