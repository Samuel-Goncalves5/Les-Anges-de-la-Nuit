using System.Collections;
using UnityEngine;
using Photon.Pun;
public class PlayerController : MonoBehaviour
{
    private PhotonView view;
    public GameObject eyes;
    
    //Partie Animation---
    public Animator _animator;
    private int EnMarche;
    private int EnMarcheMoyenne;
    private int EnMarcheRapide;
    private int EnRecul;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        eyes.SetActive(view.IsMine);

        //Partie Animation---
        EnMarche = Animator.StringToHash("EnMarche");
        EnMarcheMoyenne = Animator.StringToHash("EnMarcheMoyenne");
        EnMarcheRapide = Animator.StringToHash("EnMarcheRapide");
        EnRecul = Animator.StringToHash("EnRecul");
    }
    
    IEnumerator RunRoutine(float s)
    {//Partie Animation---
        yield return new WaitForSeconds(0.1f);
        if (s < 0.1) {_animator.SetBool(EnMarcheRapide, true);}
        else
        {
            if (!_animator.GetBool(EnMarcheMoyenne)) yield break;
            StartCoroutine(RunRoutine(s - 0.1f));
        }
    }

    void Update()
    {
        bool boutonRecul = Input.GetKey(KeyCode.S);
        bool boutonMarche = Input.GetKey(KeyCode.Z);
        bool boutonCourse = Input.GetKey(KeyCode.LeftShift);
        
        if (!view.IsMine) return;
        
             if (_animator.GetBool(EnMarcheRapide )) transform.Translate(0, 0, 2    * -5 * Time.deltaTime);
        else if (_animator.GetBool(EnMarcheMoyenne)) transform.Translate(0, 0, 1.5f * -5 * Time.deltaTime);
        else if (_animator.GetBool(EnMarche       )) transform.Translate(0, 0, 1    * -5 * Time.deltaTime);
        
        if (boutonRecul) transform.Translate(0, 0, 2.5f * Time.deltaTime);
        if (Input.GetKey(KeyCode.D)) transform.Rotate(0, 200 * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.Q)) transform.Rotate(0, -200 * Time.deltaTime, 0);
        
        //Partie Animation---
        bool marche = _animator.GetBool(EnMarche);
        bool marcheMoyenne = _animator.GetBool(EnMarcheMoyenne);
        bool marcheRapide = _animator.GetBool(EnMarcheRapide);
        
        _animator.SetBool(EnRecul, boutonRecul && !marche && !marcheMoyenne && !marcheRapide);
        _animator.SetBool(EnMarche, boutonMarche);
        _animator.SetBool(EnMarcheMoyenne, boutonMarche && boutonCourse);
        _animator.SetBool(EnMarcheRapide, boutonMarche && boutonCourse && marcheRapide);

        if (!marcheMoyenne && boutonMarche && boutonCourse) StartCoroutine(RunRoutine(1));
    }
}