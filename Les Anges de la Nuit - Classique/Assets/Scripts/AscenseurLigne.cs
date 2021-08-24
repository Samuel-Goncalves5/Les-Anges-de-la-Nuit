using Photon.Pun;
using UnityEngine;

public class AscenseurLigne : MonoBehaviour
{
    private static float AscenseurZ;
    private static float PlayerZ;

    public float ascenseurZ;
    public float playerZ;
    
    public Ascenseur[] ligne;
    public PlayerController pc;
    private CapsuleCollider cc;
    public bool _utilisation;
    public int index;
    public bool enter;
    public bool gotoindex;
    public bool exit;
    
    private void Start()
    {
        if (ascenseurZ != 0) AscenseurZ = ascenseurZ;
        if (playerZ    != 0) PlayerZ    = playerZ   ;
        
        pc = ((GameObject) PhotonNetwork.LocalPlayer.CustomProperties["Personnage"]).GetComponent<PlayerController>();
        cc = pc.GetComponent<Grappin>().NormalCollider;
        ligne = new Ascenseur[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            ligne[transform.childCount -i-1] = transform.GetChild(i).GetComponent<Ascenseur>();
    }

    private void Update()
    {
        if (enter) {Enter(); return;}
        if (gotoindex) {GoToIndex(); return;}
        if (exit) {Exit(); return;}

        if (_utilisation)
        {
            if (Input.GetKeyDown(MenuInGame.Commands[0]) && index < ligne.Length - 1)
            {index++; gotoindex = true;}
            else if (Input.GetKeyDown(MenuInGame.Commands[1]) && index > 0)
            {index--; gotoindex = true;}
            else if (Input.anyKeyDown)
                exit = true;
        }
        else
        {
            FindNear();
            if (abs(ligne[index].transform.position.x - pc.transform.position.x) > 0.5f) return;
            if (abs(ligne[index].transform.position.y - pc.transform.position.y) > 1f) return;
            if (!Input.GetKeyDown(MenuInGame.Commands[0])) return;
            
            enter = true;
        }
    }

    private float abs(float f)
    {
        if (f < 0) return -f; 
        return f;
    }

    private void Enter()
    {
        Quaternion r = pc.infiltrationCamera.transform.rotation;
        Vector3 p = pc.infiltrationCamera.transform.position;
        
        pc.Rigidbody.useGravity = false;
        cc.isTrigger = true;
        ligne[index].Porte.SetActive(false);
        
        PlayerController.STOPCONTROL = true;
        pc.transform.rotation = Quaternion.Euler(0,180,0);
        pc.ANIM_MARCHE = true;
        
        pc.transform.Translate(0,0,1f * -5 * Time.deltaTime);
        if (pc.transform.position.z > AscenseurZ)
        {
            pc.transform.position = new Vector3(pc.transform.position.x, pc.transform.position.y, AscenseurZ);
            pc.ANIM_MARCHE = false;
            enter = false;
            _utilisation = true;
            
            ligne[index].Porte.SetActive(true);
            pc.SkinnedMeshRenderer.enabled = false;
        }
        
        pc.infiltrationCamera.transform.rotation = r;
        pc.infiltrationCamera.transform.position = p;
    }

    private void Exit()
    {
        pc.SkinnedMeshRenderer.enabled = true;
        
        Quaternion r = pc.infiltrationCamera.transform.rotation;
        Vector3 p = pc.infiltrationCamera.transform.position;
        
        pc.transform.rotation = Quaternion.Euler(0,0,0);
        pc.ANIM_MARCHE = true;
        ligne[index].Porte.SetActive(false);
        
        pc.transform.Translate(0,0,1f * -5 * Time.deltaTime);
        if (pc.transform.position.z < PlayerZ)
        {
            cc.isTrigger = false;
            pc.Rigidbody.useGravity = true;
            
            pc.transform.position = new Vector3(pc.transform.position.x, pc.transform.position.y, PlayerZ);
            exit = false;
            pc.ANIM_MARCHE = false;
            _utilisation = false;
            PlayerController.STOPCONTROL = false;
            
            ligne[index].Porte.SetActive(true);
        }
        
        pc.infiltrationCamera.transform.rotation = r;
        pc.infiltrationCamera.transform.position = p;
    }

    private void FindNear()
    {
        int i = 0;
        float y = pc.transform.position.y;
        int actualIndex = 0;
        float distance = abs(ligne[0].transform.position.y - y); 
        
        foreach (Ascenseur ascenseur in ligne)
        {
            float y2 = ascenseur.transform.position.y;
            float distance2 = abs(y2 - y);
            if (distance2 < distance)
            {
                distance = distance2;
                i = actualIndex;
            }

            actualIndex++;
        }
        
        index = i;
    }

    void GoToIndex()
    {
        if (ligne[index].transform.position.y > pc.transform.position.y)
            pc.transform.Translate(0, 1f *  5 * Time.deltaTime, 0);
        else
            pc.transform.Translate(0, 1f * -5 * Time.deltaTime, 0);
        
        if (abs(ligne[index].transform.position.y - pc.transform.position.y) < 0.1f)
        {
            pc.transform.position = new Vector3(pc.transform.position.x, ligne[index].transform.position.y, pc.transform.position.z);
            gotoindex = false;
        }
    }
}