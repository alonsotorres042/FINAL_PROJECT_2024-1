using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //COMPONENTS
    private Rigidbody _myRB;

    //MOVEMENT
    [SerializeField] private float Speed;

    //ROTATION
    [SerializeField] private float TurnSensitivity;
    private Vector3 Direction;

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
    [SerializeField] private float AimSpeed;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask AimLayerMask;
    RaycastHit AimHit;
    private Vector3 MousePositionOnScreen;
    private Vector3 MousePositionOnWorld;
    private Vector3 AimDirection;
    [SerializeField] private Transform Aim;

    //SHOOTING (ONLY FOR THE TESTING!)
    [SerializeField] private float BulletSpeed;
    [SerializeField] private GameObject BulletSpawner;
    [SerializeField] private GameObject Bullet;

    // Start is called before the first frame update
    void Start()
    {
        _myRB = GetComponent<Rigidbody>();
        Cursor.visible = false;
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

        //PREVIOUS MOVEMENT SETTINGS
        /*if(Direction.z > 0f)
        {
            _myRB.velocity = transform.TransformDirection(Vector3.forward.x * Speed, _myRB.velocity.y, Vector3.forward.z * Speed);
        }
        else if(Direction.z < 0f)
        {
            _myRB.velocity = transform.TransformDirection(Vector3.back.x * Speed, _myRB.velocity.y, Vector3.back.z * Speed);
        }*/

        //CURRENT MOVEMENT SETTINGS
        _myRB.velocity = new Vector3(_camera.transform.TransformDirection(Vector3.forward).x * Speed, _myRB.velocity.y, _camera.transform.TransformDirection(Vector3.forward).z * Speed);


        //ROTATION
        if (Direction.x > 0f)
        {
            transform.rotation *= Quaternion.Euler(0, TurnSensitivity, 0);
        }
        else if (Direction.x < 0f)
        {
            transform.rotation *= Quaternion.Euler(0, -TurnSensitivity, 0);
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
            Aim.position = AimHit.point;
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
        if(context.performed)
        {
            GameObject CurBullet = Instantiate(Bullet, BulletSpawner.transform.position, Quaternion.identity);
            CurBullet.GetComponent<Rigidbody>().velocity = (Aim.position - CurBullet.transform.position).normalized * BulletSpeed;
        }
    }
    public IEnumerator ResetRayCast()
    {
        JumpRayLenght = 0;
        yield return new WaitForSeconds(0.5f);
        JumpRayLenght = 2;
        StopCoroutine(ResetRayCast());
    }
}