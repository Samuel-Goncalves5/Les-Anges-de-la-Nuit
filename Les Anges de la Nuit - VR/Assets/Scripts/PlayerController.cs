using UnityEngine;
using Photon.Pun;
public class PlayerController : MonoBehaviour
{
    public string playerName;
    private PhotonView view;
    public GameObject eyes;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        eyes.SetActive(view.IsMine);
/*
        if (!CreateAndJoinRooms.Load) return;
        
        SaveSystem.LoadPlayer();
        float[] position = Dictionnaries.positions[playerName];
        Vector3 positionVector = new Vector3(position[0], position[1], position[2]);
        transform.position = positionVector; */
    }

    void Update()
    {
        if (!view.IsMine) return;
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, 0, -5 * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, 0, 2.5f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, 200 * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -200 * Time.deltaTime, 0);
        }
    }
}
