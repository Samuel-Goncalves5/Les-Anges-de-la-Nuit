using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public static bool STOPCONTROL;
    
    [HideInInspector] public PhotonView view;
    public  GameObject eyes                 ;
    public  Rigidbody  Rigidbody            ;
    public GameObject  Pseudo               ;

    public Transform Reference;
    public Transform PiedDroit;
    public Transform PiedGauche;

    //Partie Animation---
    public Animator _animator;

    private int EnMarcheMoyenne;
    private int EnMarcheRapide ;
    private int EnMarche       ;
    private int EnRecul        ;
    private int EnLAir         ;
    private int EnAvantDroit   ;
    private int EnAtterrissage ;
    
    private void Awake()
    {
        view = GetComponent<PhotonView>();

        bool temp = view.IsMine;
        if (temp) 
        {
            GestionPseudo.CameraLocalPlayer = eyes.transform;
        }
        
        eyes.SetActive(temp);
        Pseudo.SetActive(!temp);
        
        //Partie Animation---
        EnMarche        = Animator.StringToHash("EnMarche"       );
        EnMarcheMoyenne = Animator.StringToHash("EnMarcheMoyenne");
        EnMarcheRapide  = Animator.StringToHash("EnMarcheRapide" );
        EnRecul         = Animator.StringToHash("EnRecul"        );
        EnLAir          = Animator.StringToHash("EnLAir"         );
        EnAvantDroit    = Animator.StringToHash("EnAvantDroit"   );
        EnAtterrissage  = Animator.StringToHash("EnAtterrissage" );
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
        if (!view.IsMine) return;

        KeyCode boutonMarche        = MenuInGame.Commands[0];
        KeyCode boutonRecul         = MenuInGame.Commands[1];
        KeyCode boutonTournerDroite = MenuInGame.Commands[2];
        KeyCode boutonTournerGauche = MenuInGame.Commands[3];
        KeyCode boutonMenu          = MenuInGame.Commands[4];
        KeyCode boutonSauter        = MenuInGame.Commands[5];
        KeyCode boutonGrimper       = MenuInGame.Commands[6];
        KeyCode boutonCourse        = MenuInGame.Commands[7];
        KeyCode boutonParler        = MenuInGame.Commands[8];
        
        bool marche        = !STOPCONTROL && _animator.GetBool(EnMarche);
        bool marcheMoyenne = !STOPCONTROL && _animator.GetBool(EnMarcheMoyenne);
        bool marcheRapide  = !STOPCONTROL && _animator.GetBool(EnMarcheRapide);
        bool air           = _animator.GetBool(EnLAir);
        
        DistanceUpdate();
        TranslateUpdate(marcheRapide, marcheMoyenne, marche, boutonRecul, boutonTournerDroite, boutonTournerGauche);
        JumpUpdate(air, boutonSauter, boutonRecul);
        AnimationUpdate(boutonRecul, marche, marcheMoyenne, marcheRapide, boutonMarche, boutonCourse);
        RunUpdate(marcheMoyenne, air, boutonMarche, boutonCourse);
    }

    private void OnCollisionEnter(Collision other)
    {
        //if (other.gameObject.CompareTag("Plateforme"))
        {
            StartCoroutine(CanJumpRoutine());
            _animator.SetBool(EnLAir, false);
        }
    }
    // TRANSLATE
    private void TranslateUpdate
        (bool marcheRapide,
         bool marcheMoyenne,
         bool marche,
         KeyCode boutonRecul,
         KeyCode boutonTournerDroite,
         KeyCode boutonTournerGauche)
    {
             if (marcheRapide ) transform.Translate(0, 0, 2f   * -5 * Time.deltaTime);
        else if (marcheMoyenne) transform.Translate(0, 0, 1.5f * -5 * Time.deltaTime);
        else if (marche       ) transform.Translate(0, 0, 1f   * -5 * Time.deltaTime);
            
        if (!STOPCONTROL && Input.GetKey(boutonRecul))         transform.Translate(0, 0, 2.5f * Time.deltaTime);
        if (!STOPCONTROL && Input.GetKey(boutonTournerDroite)) transform.Rotate   (0, 200 * Time.deltaTime, 0);
        if (!STOPCONTROL && Input.GetKey(boutonTournerGauche)) transform.Rotate   (0, -200 * Time.deltaTime, 0);
    }
    
    // JUMP
    private void JumpUpdate(bool air, KeyCode boutonSauter, KeyCode boutonRecul)
    {
        if (!air
            && !_animator.GetBool(EnAtterrissage)
            && !STOPCONTROL
            && Input.GetKeyDown(boutonSauter)
            && !Input.GetKey(boutonRecul))
        {
            Rigidbody.AddForce(new Vector3(0,5,0), ForceMode.Impulse);
            _animator.SetBool(EnLAir, true);
        }
    }

    IEnumerator CanJumpRoutine()
    {
        _animator.SetBool(EnAtterrissage, true);
        yield return new WaitForSeconds(0.75f);
        _animator.SetBool(EnAtterrissage, false);
    }
    
    // DISTANCE
    private float Distance(Transform t)
    {
        Vector3 temp = Reference.position - t.position;
        Vector3 Ref = transform.forward;
        return new Vector3(temp.x * Ref.x, temp.y * Ref.y, temp.z * Ref.z).sqrMagnitude;
    }

    private void DistanceUpdate()
    {
        float DistanceDroite = Distance(PiedDroit);
        float DistanceGauche = Distance(PiedGauche);
        
        _animator.SetBool(EnAvantDroit, DistanceDroite > DistanceGauche);
    }
    
    // ANIMATION
    private void AnimationUpdate(
        KeyCode boutonRecul,
        bool marche,
        bool marcheMoyenne,
        bool marcheRapide,
        KeyCode boutonMarche,
        KeyCode boutonCourse)
    {
        _animator.SetBool(EnRecul, Input.GetKey(boutonRecul) 
                                   && !STOPCONTROL
                                   && !marche 
                                   && !marcheMoyenne 
                                   && !marcheRapide);
        
        _animator.SetBool(EnMarche, Input.GetKey(boutonMarche) && !STOPCONTROL);
        
        _animator.SetBool(EnMarcheMoyenne, Input.GetKey(boutonMarche) 
                                           && Input.GetKey(boutonCourse) && !STOPCONTROL);
        
        _animator.SetBool(EnMarcheRapide, Input.GetKey(boutonMarche) 
                                          && Input.GetKey(boutonCourse) 
                                          && marcheRapide);
    }
    
    // RUN
    private void RunUpdate(bool marcheMoyenne, bool air, KeyCode boutonMarche, KeyCode boutonCourse)
    {
        if (!marcheMoyenne 
            && !air
            && Input.GetKey(boutonMarche) 
            && Input.GetKey(boutonCourse))
            StartCoroutine(RunRoutine(1));
    }
}