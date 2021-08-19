using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static bool STOPCONTROL;

    public static List<Recuperable> Objets = new List<Recuperable>();
    
    [HideInInspector] public PhotonView view;
    
    public Rigidbody  Rigidbody         ;
    public GameObject eyes              ;
    public GameObject infiltrationCamera;
    public GameObject Pseudo            ;
    public GameObject Cinemachine       ;
    public GameObject Corps             ;
    public Transform  Reference         ;
    public Transform  PiedDroit         ;
    public Transform  PiedGauche        ;
    
    //Partie Animation---
    public        Animator _animator;
    public static Animator  Animator;

    [HideInInspector] public bool infiltration;
    private string sceneName ;

    private int EnMarcheMoyenne;
    private int EnMarcheRapide ;
    private int EnMarche       ;
    private int EnRecul        ;
    private int EnLAir         ;
    private int EnAvantDroit   ;
    private int EnAtterrissage ;
    private int AnimGrappin    ;
    
    private void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
        infiltration = sceneName == "Infiltration";

        view = GetComponent<PhotonView>();

        bool temp = view.IsMine;
        if (temp)
        {
            var h = PhotonNetwork.LocalPlayer.CustomProperties;
            var levelName = SceneManager.GetActiveScene().name;
            
            if (!h.ContainsKey("Personnage")) h.Add("Personnage", gameObject);
            h["Personnage"] = gameObject;
            
            if (!h.ContainsKey("Level")) h.Add("Level", levelName);
            h["Level"] = levelName;
            
            Animator = _animator;
            GestionPseudo.CameraLocalPlayer = eyes.transform;
            if (!(Corps is null)) Corps.layer = 8;
            GetComponent<Grappin>().PositionDepart.parent.GetChild(0).gameObject.layer = 8;
        }
        
        infiltrationCamera.SetActive(temp && infiltration);
        eyes.SetActive(temp && !infiltration);
        Pseudo.SetActive(!temp);
        Cinemachine.SetActive(temp && !infiltration);

        //Partie Animation---
        EnMarche        = Animator.StringToHash("EnMarche"       );
        EnMarcheMoyenne = Animator.StringToHash("EnMarcheMoyenne");
        EnMarcheRapide  = Animator.StringToHash("EnMarcheRapide" );
        EnRecul         = Animator.StringToHash("EnRecul"        );
        EnLAir          = Animator.StringToHash("EnLAir"         );
        EnAvantDroit    = Animator.StringToHash("EnAvantDroit"   );
        EnAtterrissage  = Animator.StringToHash("EnAtterrissage" );
        AnimGrappin     = Animator.StringToHash("AnimGrappin"    );
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
        //KeyCode boutonMenu          = MenuInGame.Commands[4];
        KeyCode boutonSauter        = MenuInGame.Commands[5];
        //KeyCode boutonGrimper       = MenuInGame.Commands[6];
        KeyCode boutonCourse        = MenuInGame.Commands[7];
        //KeyCode boutonParler        = MenuInGame.Commands[8];
        //KeyCode boutonRecuperer     = MenuInGame.Commands[9];
        //KeyCode boutonObjets        = MenuInGame.Commands[10];
        
        bool marche        = !STOPCONTROL && _animator.GetBool(EnMarche);
        bool marcheMoyenne = !STOPCONTROL && _animator.GetBool(EnMarcheMoyenne);
        bool marcheRapide  = !STOPCONTROL && _animator.GetBool(EnMarcheRapide);
        bool air           = _animator.GetBool(EnLAir);
        //bool grappin       = _animator.GetBool(EnGrappin);

        bool a = _animator.GetCurrentAnimatorStateInfo(0).IsName("Grappin");
        bool b = _animator.GetCurrentAnimatorStateInfo(0).IsName("Grappin 2");
        _animator.SetBool(AnimGrappin, a || b);
        
        DistanceUpdate();
        TranslateUpdate(marcheRapide, marcheMoyenne, marche, boutonRecul, boutonTournerDroite, boutonTournerGauche);
        JumpUpdate(air, boutonMarche, boutonSauter, boutonRecul);
        AnimationUpdate(boutonRecul, marche, marcheMoyenne, marcheRapide, boutonMarche, boutonCourse, boutonTournerDroite, boutonTournerGauche);
        RunUpdate(marcheMoyenne, air, boutonMarche, boutonCourse, boutonTournerDroite, boutonTournerGauche);
    }

    private void OnCollisionEnter(Collision other)
    {
        ContactPoint cp = other.GetContact(0);
        if (cp.normal.y > 0.5f)
        {
            StartCoroutine(CanJumpRoutine());
            _animator.SetBool(EnLAir, false);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        ContactPoint cp = other.GetContact(0);
        if (cp.normal.y < 0.1f && Input.GetKeyDown(MenuInGame.Commands[5]))
        {
            if (_animator.GetBool(EnLAir))
            {
                bool a = _animator.GetCurrentAnimatorStateInfo(0).IsName("SautDroit");
                bool b = _animator.GetCurrentAnimatorStateInfo(0).IsName("MidSautDroit");
                bool c = _animator.GetCurrentAnimatorStateInfo(0).IsName("PostSautDroit");
                if (a || b || c) _animator.Play("SautGauche");
                else _animator.Play("SautDroit");

                Debug.Log(cp.normal.x + " " + cp.normal.z);
                try
                {
                    if (infiltration)
                    {
                        transform.localRotation = Quaternion.Euler(
                            0, 
                            -transform.rotation.y, 
                            0
                        );
                    }
                    else
                    {
                        transform.localRotation = Quaternion.Euler(
                            0, 
                            (float) (System.Math.Atan(cp.normal.x / cp.normal.z) * 180/ System.Math.PI) + 180, 
                            0
                        );
                    }
                }
                catch (System.Exception)
                {
                    if (infiltration)
                    {
                        transform.localRotation = Quaternion.Euler(
                            0, 
                            -transform.rotation.y, 
                            0
                        );
                    }
                    else
                    {
                        transform.localRotation = Quaternion.Euler(
                            0,
                            180 + transform.localRotation.y,
                            0
                        );
                    }
                }
                Rigidbody.AddForce(new Vector3(0,2,0), ForceMode.Impulse);
            }
        }
    }

    // TRANSLATE
    private void TranslateUpdate (bool marcheRapide, bool marcheMoyenne, bool marche,
         KeyCode boutonRecul, KeyCode boutonTournerDroite, KeyCode boutonTournerGauche)
    {
        if (infiltration && !STOPCONTROL)
        {
            if (Input.GetKey(boutonTournerGauche))
            {
                Quaternion r = infiltrationCamera.transform.rotation;
                Vector3 p = infiltrationCamera.transform.position;
                
                transform.rotation = Quaternion.Euler(0,90,0);

                infiltrationCamera.transform.rotation = r;
                infiltrationCamera.transform.position = p;
                
                if (marcheRapide) transform.Translate(0, 0, 2f * -5 * Time.deltaTime);
                else if (marcheMoyenne) transform.Translate(0, 0, 1.5f * -5 * Time.deltaTime);
                else if (marche) transform.Translate(0, 0, 1f * -5 * Time.deltaTime);
            }

            else if (Input.GetKey(boutonTournerDroite))
            {
                Quaternion r = infiltrationCamera.transform.rotation;
                Vector3 p = infiltrationCamera.transform.position;
                
                transform.rotation = Quaternion.Euler(0,-90,0);
                
                infiltrationCamera.transform.rotation = r;
                infiltrationCamera.transform.position = p;
                
                if (marcheRapide) transform.Translate(0, 0, 2f * -5 * Time.deltaTime);
                else if (marcheMoyenne) transform.Translate(0, 0, 1.5f * -5 * Time.deltaTime);
                else if (marche) transform.Translate(0, 0, 1f * -5 * Time.deltaTime);
            }
        }
        else
        {
            float rotation = Input.GetAxis("Mouse X") * Time.deltaTime;
            transform.Rotate(0, rotation * 300, 0);
            if (!STOPCONTROL && Input.GetKey(boutonTournerDroite)) transform.Rotate(0, 200 * Time.deltaTime, 0);
            if (!STOPCONTROL && Input.GetKey(boutonTournerGauche)) transform.Rotate(0, -200 * Time.deltaTime, 0);


            if (marcheRapide) transform.Translate(0, 0, 2f * -5 * Time.deltaTime);
            else if (marcheMoyenne) transform.Translate(0, 0, 1.5f * -5 * Time.deltaTime);
            else if (marche) transform.Translate(0, 0, 1f * -5 * Time.deltaTime);

            if (!STOPCONTROL && Input.GetKey(boutonRecul)) transform.Translate(0, 0, 2.5f * Time.deltaTime);
        }
    }
    
    // JUMP
    private void JumpUpdate(bool air, KeyCode marche, KeyCode boutonSauter, KeyCode boutonRecul)
    {
        if (!air
            && !_animator.GetBool(EnAtterrissage)
            && !STOPCONTROL
            && (Input.GetKeyDown(boutonSauter) || Input.GetKeyDown(marche) && infiltration)
            && !Input.GetKey(boutonRecul))
        {
            Rigidbody.AddForce(new Vector3(0,5,0), ForceMode.Impulse);
            _animator.SetBool(EnLAir, true);
        }
    }

    IEnumerator CanJumpRoutine()
    {
        _animator.SetBool(EnAtterrissage, true);
        yield return new WaitForSeconds(0.05f);
        _animator.SetBool(EnAtterrissage, false);
    }
    
    // DISTANCE
    private float Distance(Transform t)
    {
        Vector3 temp = Reference.position - t.position;
        Vector3 refVector3 = transform.forward;
        return new Vector3(temp.x * refVector3.x, temp.y * refVector3.y, temp.z * refVector3.z).sqrMagnitude;
    }

    private void DistanceUpdate()
    {
        float distanceDroite = Distance(PiedDroit);
        float distanceGauche = Distance(PiedGauche);
        
        _animator.SetBool(EnAvantDroit, distanceDroite > distanceGauche);
    }
    
    // ANIMATION
    private void AnimationUpdate(
        KeyCode boutonRecul,
        bool marche,
        bool marcheMoyenne,
        bool marcheRapide,
        KeyCode boutonMarche,
        KeyCode boutonCourse,
        KeyCode boutonTournerDroite,
        KeyCode boutonTournerGauche)
    {
        if (infiltration)
        {
            _animator.SetBool(EnMarche, (Input.GetKey(boutonTournerDroite) || Input.GetKey(boutonTournerGauche)) 
                                        && !STOPCONTROL);
        
            _animator.SetBool(EnMarcheMoyenne, (Input.GetKey(boutonTournerDroite) || Input.GetKey(boutonTournerGauche))
                                               && Input.GetKey(boutonCourse) 
                                               && !STOPCONTROL);
        
            _animator.SetBool(EnMarcheRapide, (Input.GetKey(boutonTournerDroite) || Input.GetKey(boutonTournerGauche))
                                              && Input.GetKey(boutonCourse) 
                                              && marcheRapide);
        }
        else
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
    }
    
    // RUN
    private void RunUpdate(
        bool marcheMoyenne, 
        bool air, 
        KeyCode boutonMarche, 
        KeyCode boutonCourse,
        KeyCode boutonTournerDroite,
        KeyCode boutonTournerGauche)
    {
        if (infiltration)
        {
            if (!marcheMoyenne 
                && !air
                && (Input.GetKey(boutonTournerDroite) || Input.GetKey(boutonTournerGauche))
                && Input.GetKey(boutonCourse))
                StartCoroutine(RunRoutine(1));
        }
        else
        {
            if (!marcheMoyenne 
                && !air
                && Input.GetKey(boutonMarche) 
                && Input.GetKey(boutonCourse))
                StartCoroutine(RunRoutine(1));
        }
    }
}