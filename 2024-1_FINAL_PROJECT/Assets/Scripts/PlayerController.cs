using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    //COMPONENTS
    private Rigidbody _myRB;

    //MOVEMENT
    [SerializeField] private float Speed;

    //ROTATION
    [SerializeField] private float TurnSensitivity;
    private Vector3 Direction;

    //JUMP
    [SerializeField] private float JumpForce;
    [SerializeField] private float RayLenght;
    private bool CanJump;
    [SerializeField] private LayerMask layerMask;
    RaycastHit hit;

    //LEVITATION
    [SerializeField] private float Top;
    [SerializeField] private float DepthFromPoint;
    [SerializeField] private float DisplacementAmount;

    //AIM
    private Vector2 MousePosition;
    [SerializeField] private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _myRB = GetComponent<Rigidbody>();
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
        if(Direction.z > 0f)
        {
            _myRB.velocity = transform.TransformDirection(Vector3.forward.x * Speed, _myRB.velocity.y, Vector3.forward.z * Speed);
        }
        else if(Direction.z < 0f)
        {
            _myRB.velocity = transform.TransformDirection(Vector3.back.x * Speed, _myRB.velocity.y, Vector3.back.z * Speed);
        }

        //ROTATION
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * RayLenght, Color.white);
        if (Direction.x > 0f)
        {
            transform.rotation *= Quaternion.Euler(0, TurnSensitivity, 0);
        }
        else if (Direction.x < 0f)
        {
            transform.rotation *= Quaternion.Euler(0, -TurnSensitivity, 0);
        }

        //JUMP
        if (Physics.Raycast(transform.position, Vector3.down, out hit, RayLenght, layerMask))  
        {
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.red);
            CanJump = true;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down * RayLenght, Color.white);
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
            if (CanJump)
            {
                _myRB.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                CanJump = false;
                StartCoroutine(ResetRayCast());
            }
        }
    }
    public void GetMousePositionOnScreen(InputAction.CallbackContext context)
    {
        MousePosition = context.ReadValue<Vector2>();
    }
    public IEnumerator ResetRayCast()
    {
        RayLenght = 0;
        yield return new WaitForSeconds(1f);
        RayLenght = 2;
        StopCoroutine(ResetRayCast());
    }
}