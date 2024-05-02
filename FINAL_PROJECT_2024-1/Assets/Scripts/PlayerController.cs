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
    private Vector3 direction;

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
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector2 MousePosition;

    // Start is called before the first frame update
    void Start()
    {
        _myRB = GetComponent<Rigidbody>();

        GetMousePosition();
    }
    void FixedUpdate()
    {
        if (transform.position.y < Top)
        {
            float displacementMultiplier = Mathf.Clamp01((Top - transform.position.y) / DepthFromPoint) * DisplacementAmount;
            _myRB.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
        }

        _myRB.velocity = new Vector3(direction.x, _myRB.velocity.y, direction.z);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, RayLenght, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
            CanJump = true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * RayLenght, Color.white);
        }
    }
    public void OnMovement(InputAction.CallbackContext context)
    {
        direction = transform.TransformDirection(context.ReadValue<Vector3>() * Speed);
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
    public void GetMousePosition()
    {
        MousePosition = Mouse.current.position.ReadValue();
    }
    public IEnumerator ResetRayCast()
    {
        RayLenght = 0;
        yield return new WaitForSeconds(1f);
        RayLenght = 2;
        StopCoroutine(ResetRayCast());
    }
}