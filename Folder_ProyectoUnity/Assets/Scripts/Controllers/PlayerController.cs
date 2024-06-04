using Cinemachine;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Runtime.CompilerServices;

public class PlayerController : MonoBehaviour
{
    //GENERAL
    [SerializeField] private EventManagerData eventManagerData;
    [SerializeField] private PlayerSoundData soundData;

    //COMPONENTS
    private Rigidbody _myRB;
    private AudioSource _myAS;

    //ESCENTIALS
    [SerializeField] private float MaxLife;
    [SerializeField] private float CurrentLife;

    //MOVEMENT
    [SerializeField] private Transform RelativeMovement;
    private float CurrentSpeed;
    [SerializeField] private float Speed;
    private Vector3 Direction;

    //SPRINT
    private bool IsSprinting;
    [SerializeField] private float SprintSpeed; //<-- Obviously greater than previous Speed;
    [SerializeField] private float CurrentStamina;
    [SerializeField] private float MaxStamina;

    //ROTATION
    [SerializeField] private float RotationSpeed;

    //JUMPING
    private bool CanJump;
    private int CurrentJumps;
    [SerializeField] private int MaxJumps;
    [SerializeField] private float JumpForce;
    [SerializeField] private float JumpRayLenght;
    [SerializeField] private LayerMask JumpLayerMask;
    RaycastHit JumpHit;

    //LEVITATION
    [SerializeField] private float Top;
    [SerializeField] private float DepthFromPoint;
    [SerializeField] private float DisplacementAmount;

    //SHOOTING
    private bool IsShooting;
    [SerializeField] private float ShotCadency;
    [SerializeField] private Transform BulletSpawner;
    [SerializeField] private Transform ProjectileSpawner;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private GameObject Explosive;
    private IEnumerator SpawnBulletsRef;

    //AIM
    private bool IsAiming;
    [SerializeField] private float CameraReincorporationTime;
    [SerializeField] private float AimZoom;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask AimLayerMask;
    RaycastHit AimHit;
    private Vector3 MousePositionOnScreen;
    private Vector3 MousePositionOnWorld;
    [SerializeField] private Transform AimObject;
    [SerializeField] private Transform CharacterLookAt;
    [SerializeField] private CinemachineFreeLook ThirdPersonCamera;
    private Vector3 Velocity = Vector3.zero;

    //CONTRATION
    private bool IsConcentrated;

    //EXTERNAL PARTS
    [SerializeField] private float LeftArmSpeed;
    [SerializeField] private GameObject LeftArm;
    [SerializeField] private Transform LeftArmTargetPosition;

    [SerializeField] private Color[] LifeColors;
    [SerializeField] private Material LifeIndicatorMat;

    //PUBLIC GETTERS
    public float _currentLife { get { return CurrentLife; } private set { } }
    public Vector3 _bulletDirection { get { return (AimObject.position - BulletSpawner.position).normalized; } private set { } }
    public Transform _transform { get { return transform; } private set { } }

    void Awake()
    {
        CurrentLife = MaxLife;
    }
    void Start()
    {
        _myRB = GetComponent<Rigidbody>();
        _myAS = GetComponent<AudioSource>();
        CurrentSpeed = Speed;
        LifeIndicatorMat.SetColor("_LifeEmission", LifeColors[LifeColors.Length - 1]);
        LifeIndicatorMat.SetFloat("_EmissionIntensity", MaxStamina);
        CurrentStamina = MaxStamina;
        IsShooting = false;
        IsAiming = false;
    }
    private void OnEnable()
    {
        eventManagerData._EventManager.Defeat += Death;
    }
    private void OnDisable()
    {
        eventManagerData._EventManager.Defeat -= Death;
    }
    void Update()
    {
        SetLifeIndicatorColor();
        SetSprint();
    }

    void FixedUpdate()
    {
        //LEVITATION
        /*if (transform.position.y < Top)
        {
            float displacementMultiplier = Mathf.Clamp01((Top - transform.position.y) / DepthFromPoint) * DisplacementAmount;
            _myRB.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
        }*/

        //MOVEMENT
        RelativeMovement.position = _camera.transform.position;
        RelativeMovement.rotation = new Quaternion(0f, _camera.transform.rotation.y, 0f, _camera.transform.rotation.w);
        if (Direction != Vector3.zero)
        {
            _myRB.velocity = Vector3.Lerp(_myRB.velocity, new Vector3(RelativeMovement.TransformDirection(Direction.normalized).x * CurrentSpeed, _myRB.velocity.y, RelativeMovement.TransformDirection(Direction.normalized).z * CurrentSpeed), (RotationSpeed / 2) * Time.deltaTime);
        }
        else
        {
            _myRB.velocity = new Vector3(Mathf.Lerp(_myRB.velocity.x, 0f, (RotationSpeed / 5f) * Time.deltaTime), _myRB.velocity.y, Mathf.Lerp(_myRB.velocity.z, 0f, (RotationSpeed / 5f) * Time.deltaTime));
        }

        //ROTATION
        if (IsAiming)
        {
            LookAtCenter();
            ThirdPersonCamera.m_Lens.FieldOfView = Mathf.Lerp(ThirdPersonCamera.m_Lens.FieldOfView, 50f - AimZoom, 10f * Time.deltaTime);
        }
        else
        {
            if (IsShooting)
            {
                LookAtCenter();
            }
            else
            {
                LookAtDirection();
            }
            ThirdPersonCamera.m_Lens.FieldOfView = Mathf.Lerp(ThirdPersonCamera.m_Lens.FieldOfView, 50f, 10f * Time.deltaTime);
        }

        //JUMP
        if (Physics.Raycast(transform.position, Vector3.down, out JumpHit, JumpRayLenght, JumpLayerMask))
        {
            Debug.DrawRay(transform.position, Vector3.down * JumpHit.distance, Color.red);
            CurrentJumps = MaxJumps;
            CanJump = true;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down * JumpRayLenght, Color.white);
        }

        //AIM
        CharacterLookAt.position = Vector3.SmoothDamp(CharacterLookAt.position, transform.position, ref Velocity, CameraReincorporationTime);

        Ray aim = _camera.ScreenPointToRay(MousePositionOnScreen);
        if (Physics.Raycast(aim, out AimHit, Mathf.Infinity, AimLayerMask))
        {
            AimObject.position = AimHit.point;
        }

        //======================== EXTERNAL EXTREMITIES ========================//

        // LEFTARM ROTATION
        if (IsShooting == true)
        {
            LeftArm.transform.rotation = Quaternion.Slerp(LeftArm.transform.rotation, Quaternion.LookRotation(AimObject.position - LeftArm.transform.position), 30f * Time.deltaTime);
        }
        else if (IsShooting == false)
        {
            LeftArm.transform.rotation = Quaternion.Slerp(LeftArm.transform.rotation, Quaternion.LookRotation(Vector3.down), 20f * Time.deltaTime);
        }

        //LEFT ARM POSITION
        LeftArm.transform.position = Vector3.Slerp(LeftArm.transform.position, LeftArmTargetPosition.position, LeftArmSpeed * Time.deltaTime);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            if (collision.gameObject.tag == "Enemy")
            {
                GetDamage(collision.gameObject.GetComponent<EnemyClass>()._meleeDamage);
            }
            else if (collision.gameObject.tag == "Boss")
            {
                GetDamage(collision.gameObject.GetComponentInParent<EnemyClass>()._meleeDamage);

            }
            collision.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3((transform.position - collision.transform.position).x, 3.5f, (transform.position - collision.transform.position).z) * 250f);
        }
    }
    public void OnMovement(InputAction.CallbackContext context)
    {
        Direction = context.ReadValue<Vector3>();
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsSprinting = true;
        }
        else if (context.canceled)
        {
            IsSprinting = false;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (CanJump && CurrentJumps > 0)
            {
                _myAS.PlayOneShot(soundData.Jump);
                _myRB.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                StartCoroutine(ResetRayCast());
                CurrentJumps--;
            }
            else if (CurrentJumps <= 0)
            {
                CanJump = false;
            }
        }
    }
    public void GetMousePosition(InputAction.CallbackContext context)
    {
        try
        {
            MousePositionOnScreen = context.ReadValue<Vector2>();
            MousePositionOnWorld = Camera.main.ScreenToWorldPoint(MousePositionOnScreen);
        }
        catch(System.NullReferenceException)
        {

        }
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsAiming = true;
        }
        else if (context.canceled)
        {
            IsAiming = false;
        }
    }
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (eventManagerData._UIManager._isPaused == false)
            {
                IsShooting = true;
                SetSpawnBulletsReferene(SpawnBullets(ShotCadency));
                StartCoroutine(SpawnBulletsRef);
            }
        }
        else if (context.canceled)
        {
            IsShooting = false;
            try
            {
                StopCoroutine(SpawnBulletsRef);
            }
            catch (System.NullReferenceException)
            {

            }
        }
    }
    public void OnLaunchExplosive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Instantiate(Explosive, ProjectileSpawner.position, transform.rotation);
        }
    }
    public void OnConcentration(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsConcentrated = true;
            SetConcentration();
        }
        else if (context.canceled)
        {
            IsConcentrated = false;
            SetConcentration();
        }
    }
    public IEnumerator SpawnBullets(float cadency)
    {
        while (true)
        {
            if (IsShooting == true)
            {
                _myAS.PlayOneShot(soundData.Shot);
                GameObject CurrentBullet = Instantiate(Bullet, BulletSpawner.transform.position, transform.rotation);
                yield return new WaitForSeconds(cadency);
            }
        }
    }
    public IEnumerator ResetRayCast()
    {
        JumpRayLenght = 0;
        yield return new WaitForSeconds(0.5f);
        JumpRayLenght = 0.5f;
        StopCoroutine(ResetRayCast());
    }
    public void SetSpawnBulletsReferene(IEnumerator spawnBullets)
    {
        SpawnBulletsRef = spawnBullets;
    }
    public void GetDamage(float Damage)
    {
        _myAS.PlayOneShot(soundData.Hurt);
        CurrentLife = CurrentLife - Damage;
    }
    public void SetLifeIndicatorColor()
    {
        for (int i = 0; i < LifeColors.Length; i++)
        {
            if (CurrentLife <= 0)
            {
                LifeIndicatorMat.SetColor("_LifeEmission", Color.Lerp(LifeIndicatorMat.GetColor("_LifeEmission"), LifeColors[0], 10f * Time.deltaTime));
            }
            else if (CurrentLife >= MaxLife - ((MaxLife - ((MaxLife / LifeColors.Length) * i))) && CurrentLife < MaxLife - ((MaxLife - ((MaxLife / LifeColors.Length) * (i + 1)))))
            {
                LifeIndicatorMat.SetColor("_LifeEmission", Color.Lerp(LifeIndicatorMat.GetColor("_LifeEmission"), LifeColors[i], 10f * Time.deltaTime));
            }
        }
    }
    public void SetConcentration()
    {
        if (eventManagerData._UIManager._isPaused == false)
        {
            if (IsAiming)
            {
                if (IsConcentrated)
                {
                    Time.timeScale = 0.25f;
                    Time.fixedDeltaTime = Time.timeScale * 0.02f;
                }
                else
                {
                    Time.timeScale = 1f;
                    Time.fixedDeltaTime = 0.02f;
                }
            }
            else
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
            }
        }
    }
    public void LookAtDirection()
    {
        if (Direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(_camera.transform.TransformDirection(Direction).x, transform.TransformDirection(Vector3.forward).y, _camera.transform.TransformDirection(Direction).z)), RotationSpeed * Time.deltaTime);
        }
    }
    public void LookAtCenter()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(_camera.transform.TransformDirection(Vector3.forward).x, transform.TransformDirection(Vector3.forward).y, _camera.transform.TransformDirection(Vector3.forward).z)), RotationSpeed * Time.deltaTime);
    }
    public void Death()
    {
        _myAS.PlayOneShot(soundData.Death);
        GetComponent<PlayerInput>().enabled = false;
    }
    public void SetSprint()
    {
        if (IsSprinting)
        {
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, SprintSpeed, 10 * Time.deltaTime);
            LifeIndicatorMat.SetFloat("_EmissionIntensity", Mathf.Lerp(LifeIndicatorMat.GetFloat("_EmissionIntensity"), 0f, 0.5f * Time.deltaTime));
        }
        else
        {
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, Speed, 10 * Time.deltaTime);
            LifeIndicatorMat.SetFloat("_EmissionIntensity", Mathf.Lerp(LifeIndicatorMat.GetFloat("_EmissionIntensity"), MaxStamina, 0.1f * Time.deltaTime));
        }
    }
}