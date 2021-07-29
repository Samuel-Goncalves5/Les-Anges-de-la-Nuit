using System;
using Photon.Pun;
using UnityEngine;

public class Grappin : MonoBehaviour
{
    public static bool MODEGRAPPIN;
    public static GameObject PoidsGrappin;
    private PoidsGrappin pg;
    
    public Camera Camera;
    public Camera GrappinCamera;
    public RaycastHit RaycastHit;
    public int maxDistance;
    public bool isMoving;
    public Vector3 PositionFin;
    public float vitesseGrappin;
    public Transform PositionDepart;
    
    private void Update()
    {
        try
        {
            pg.lineRenderer.SetPosition(0, PositionDepart.position);
        }
        catch (Exception)
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
                if (!(PoidsGrappin is null) && PoidsGrappin.activeSelf) return;
                
                Camera.gameObject.SetActive(true);
                GrappinCamera.gameObject.SetActive(false);
                MODEGRAPPIN = false;
            }
        }
    }

    public void Tirer()
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
                pg.PositionDepart = PoidsGrappin.transform;
                pg.PositionFin = PositionFin;
                pg.launched = true;
                
                PlayerController.STOPCONTROL = true;
            }
        }
        else
        {
            Camera.gameObject.SetActive(false);
            GrappinCamera.gameObject.SetActive(true);
            MODEGRAPPIN = true;
        }
    }

    public void Bouger()
    {
        transform.position = Vector3.Lerp(transform.position, PositionFin, vitesseGrappin * Time.deltaTime / Vector3.Distance(transform.position, PositionFin));

        if (Vector3.Distance(transform.position, PositionFin) < 1f)
        {
            isMoving = false;
            PlayerController.STOPCONTROL = false;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            Destroy(PoidsGrappin);
        }
    }
}