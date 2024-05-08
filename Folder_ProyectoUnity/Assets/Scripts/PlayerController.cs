using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //GENERAL
    [SerializeField] private EventManager _eventManager;

    //COMPONENTS
    private Rigidbody _myRB;

    //MOVEMENT
    [SerializeField] private float Speed;
    private Vector3 Direction;

    //ROTATION
    [SerializeField] private float RotationSpeed;

    //JUMPING
    private int CurrentJumps;
    [SerializeField] private int MaxJumps;
    [SerializeField] private float JumpForce;
    [SerializeField] private float JumpRayLenght;
    private bool CanJump;
    [SerializeField] private LayerMask JumpLayerMask;
    RaycastHit JumpHit;

    //LEVITATION
    [SerializeField] private float Top;
    [SerializeField] private float DepthFromPoint;
    [SerializeField] private float DisplacementAmount;

    //AIM
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask AimLayerMask;
    RaycastHit AimHit;
    private Vector3 MousePositionOnScreen;
    private Vector3 MousePositionOnWorld;
    [SerializeField] private Transform AimObject;

    //SHOOTING
    private bool IsShooting;
    //private bool CanShoot;
    [SerializeField] private float BulletSpeed;
    [SerializeField] private GameObject BulletSpawner;
    [SerializeField] private GameObject Bullet;
    void Start()
    {
        _myRB = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //CanShoot = true;
        IsShooting = false;
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
        if(Direction != Vector3.zero)
        {
            _myRB.velocity = new Vector3(_camera.transform.TransformDirection(Direction).x * Speed, _myRB.velocity.y, _camera.transform.TransformDirection(Direction).z * Speed);
        }

        //ROTATION
        if(Direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(_camera.transform.TransformDirection(Direction).x, transform.TransformDirection(Vector3.forward).y, _camera.transform.TransformDirection(Direction).z)), RotationSpeed * Time.deltaTime);
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
        Ray aim = _camera.ScreenPointToRay(MousePositionOnScreen);
        if(Physics.Raycast(aim, out AimHit, Mathf.Infinity, AimLayerMask))
        {
            AimObject.position = AimHit.point;
        }
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
            }else if(CurrentJumps <= 0)
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
        }
        else if (context.canceled)
        {
            IsShooting = false;
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
                CurrentBullet.GetComponent<Rigidbody>().velocity = (AimObject.position - BulletSpawner.transform.position).normalized * BulletSpeed;
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }
    }
}