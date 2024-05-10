using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //GENERAL
    [SerializeField] private BulletDirection_SO bulletDirection_SO;
    [SerializeField] private EventManager eventManager;

    //COMPONENTS
    private Rigidbody _myRB;

    //MOVEMENT
    [SerializeField] private float Speed;
    private Vector3 Direction;

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
    [SerializeField] private GameObject BulletSpawner;
    [SerializeField] private GameObject Bullet;
    private Vector3 BulletDirection;
    public Vector3 _bulletDirection { get { return BulletDirection; } private set { } }

    //AIM
    private bool isAiming;
    [SerializeField] private float CameraReincorporationSpeed;
    [SerializeField] private float AimZoom;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask AimLayerMask;
    RaycastHit AimHit;
    private Vector3 MousePositionOnScreen;
    private Vector3 MousePositionOnWorld;
    [SerializeField] private Transform AimObject;
    [SerializeField] private Transform CharacterLookAt;
    [SerializeField] private CinemachineFreeLook ThirdPersonCamera;

    //EXTERNAL PARTS
    [SerializeField] private float LeftArmSpeed;
    [SerializeField] private GameObject LeftArm;
    [SerializeField] private Transform LeftArmTargetPosition;

    void Start()
    {
        _myRB = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        IsShooting = false;
        isAiming = false;
        StartCoroutine(SpawnBullets());
    }
    void FixedUpdate()
    {
        //LEVITATION
        if (transform.position.y < Top)
        {
            float displacementMultiplier = Mathf.Clamp01((Top - transform.position.y) / DepthFromPoint) * DisplacementAmount;
            _myRB.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
        }

        //MOVEMENT
        if (Direction != Vector3.zero)
        {
            _myRB.velocity = new Vector3(_camera.transform.TransformDirection(Direction).x * Speed, _myRB.velocity.y, _camera.transform.TransformDirection(Direction).z * Speed);
        }

        //ROTATION
        if (isAiming)
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
        CharacterLookAt.position = Vector3.Slerp(CharacterLookAt.position, transform.position, CameraReincorporationSpeed * Time.deltaTime);

        Ray aim = _camera.ScreenPointToRay(MousePositionOnScreen);
        if (Physics.Raycast(aim, out AimHit, Mathf.Infinity, AimLayerMask))
        {
            AimObject.position = AimHit.point;
        }

        //======================== EXTERNAL PARTS =========================

        // LEFTARM ROTATION
        if (IsShooting == true)
        {
            LeftArm.transform.rotation = Quaternion.Slerp(LeftArm.transform.rotation, Quaternion.LookRotation(AimObject.position - LeftArm.transform.position), 10f * Time.deltaTime);
        }
        else if (IsShooting == false)
        {
            LeftArm.transform.rotation = Quaternion.Slerp(LeftArm.transform.rotation, Quaternion.LookRotation(Vector3.down), 10f * Time.deltaTime);
        }

        //LEFT ARM POSITION
        LeftArm.transform.position = Vector3.Slerp(LeftArm.transform.position, LeftArmTargetPosition.position, LeftArmSpeed * Time.deltaTime);
    }
    public void OnMovement(InputAction.CallbackContext context)
    {
        Direction = context.ReadValue<Vector3>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (CanJump && CurrentJumps > 0)
            {
                _myRB.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                StartCoroutine(ResetRayCast());
                CurrentJumps--;
            }
            else if(CurrentJumps <= 0)
            {
                CanJump = false;
            }
        }
    }
    public void GetMousePosition(InputAction.CallbackContext context)
    {
        MousePositionOnScreen = context.ReadValue<Vector2>();
        MousePositionOnWorld = Camera.main.ScreenToWorldPoint(MousePositionOnScreen);
    }
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsShooting = true;
            BulletDirection = (AimObject.position - BulletSpawner.transform.position).normalized;
        }
        else if (context.canceled)
        {
            IsShooting = false;
        }
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isAiming = true;
        }
        else if (context.canceled)
        {
            isAiming = false;
        }
    }
    public IEnumerator ResetRayCast()
    {
        JumpRayLenght = 0;
        yield return new WaitForSeconds(0.5f);
        JumpRayLenght = 2;
        StopCoroutine(ResetRayCast());
    }
    public IEnumerator SpawnBullets()
    {
        while (true)
        {
            if (IsShooting == true)
            {
                GameObject CurrentBullet = Instantiate(Bullet, BulletSpawner.transform.position, transform.rotation);
                bulletDirection_SO.BulletDirection = (AimObject.position - BulletSpawner.transform.position).normalized;
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }
    }
    public void LookAtCenter()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(_camera.transform.TransformDirection(Vector3.forward).x, transform.TransformDirection(Vector3.forward).y, _camera.transform.TransformDirection(Vector3.forward).z)), RotationSpeed * Time.deltaTime);
    }
    public void LookAtDirection()
    {
        if (Direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(_camera.transform.TransformDirection(Direction).x, transform.TransformDirection(Vector3.forward).y, _camera.transform.TransformDirection(Direction).z)), RotationSpeed * Time.deltaTime);
        }
    }
}