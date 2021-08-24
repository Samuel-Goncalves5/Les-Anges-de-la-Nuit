using Photon.Pun;
using UnityEngine;

public class Grappin : MonoBehaviour
{
    
    public static GameObject PoidsGrappin;

    public CapsuleCollider NormalCollider;
    public CapsuleCollider GrappinCollider;

    public Transform PositionDepartPerso;
    
    public Camera Camera;
    public Camera GrappinCamera;
    private RaycastHit RaycastHit;
    public int maxDistance;
    public bool isMoving;
    public Vector3 PositionFin;
    public float vitesseGrappin;
    public Transform PositionDepart;
    private static readonly int EnGrappin1 = Animator.StringToHash("EnGrappin");

    private void Start()
    {
        bool a = gameObject.GetComponent<PlayerController>().infiltration;
        bool b = gameObject.GetComponent<PhotonView>().IsMine;
        enabled = !a && b;
    }

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
                try {if (!(PoidsGrappin is null) && PoidsGrappin.activeSelf) return;}
                catch (UnassignedReferenceException) {}
                
                Camera.gameObject.SetActive(true);
                GrappinCamera.gameObject.SetActive(false);
                GetComponent<PhotonView>().RPC("ActiveLanceGrappin", RpcTarget.All, 
                    transform.name, false);
                
            }
        }

        bool EnGrappin;
        try {EnGrappin = !(PoidsGrappin is null) && PoidsGrappin.activeSelf;}
        catch (UnassignedReferenceException) {EnGrappin = false;}

        PlayerController.Animator.SetBool(EnGrappin1, EnGrappin);
    }

    [PunRPC]
    public void ActiveLanceGrappin(string name, bool activation)
    {
        GameObject.Find(name).GetComponent<Grappin>().PositionDepart.parent.gameObject.SetActive(activation);
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
                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.velocity = new Vector3(0,0,0);
                rb.angularVelocity = new Vector3(0,0,0);
                if (!(PoidsGrappin is null)) PhotonNetwork.Destroy(PoidsGrappin);
                
                PoidsGrappin = PhotonNetwork.Instantiate("Grappin", transform.position, transform.rotation);
                gameObject.GetComponent<PhotonView>().RPC("RenameGrappin", RpcTarget.All, PoidsGrappin.name, gameObject.name);
                
                PlayerController.STOPCONTROL = true;
            }
        }
        else
        {
            Camera.gameObject.SetActive(false);
            GrappinCamera.gameObject.SetActive(true);
            GetComponent<PhotonView>().RPC("ActiveLanceGrappin", RpcTarget.All, 
                transform.name, true);
        }
    }

    [PunRPC]
    public void RenameGrappin(string g, string s)
    {
        GameObject.Find(g).name = "Grappin_" + s;
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
            PhotonNetwork.Destroy(PoidsGrappin);
            PoidsGrappin = null;
        }
    }
}