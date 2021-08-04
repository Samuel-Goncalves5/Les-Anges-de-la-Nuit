using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Rafale : MonoBehaviour
{
    private GameObject Elea;
    public Animator Animator;
    private int IsNear;
    public float speed;
    private bool Allongement;
    public Transform temp;

    private void Start()
    {
        IsNear = Animator.StringToHash("IsNear");
    }

    private void Update()
    {
        if (Elea is null) SearchElea();
        else FollowElea();
    }

    private void SearchElea()
    {
        Allongement = Animator.GetCurrentAnimatorStateInfo(0).IsName("Allongement");
        
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if ((string) player.CustomProperties["Character"] != "Elea") continue;
            Elea = (GameObject) player.CustomProperties["Personnage"];
            break;
        }
    }

    private void FollowElea()
    {
        bool near = Vector3.Distance(Elea.transform.position, transform.position) < 3;
        Animator.SetBool(IsNear, near);

        if (!near && !Allongement)
        {
            temp.LookAt(Elea.transform);
            Quaternion r = transform.rotation;
            transform.rotation = new Quaternion(r.x, temp.rotation.y, r.z, r.w);
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
    }
}