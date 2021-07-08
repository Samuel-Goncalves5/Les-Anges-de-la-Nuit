using System;
using UnityEngine;
using Photon.Pun;
public class PlayerController : MonoBehaviour
{
    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
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
