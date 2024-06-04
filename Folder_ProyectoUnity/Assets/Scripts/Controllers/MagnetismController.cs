using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetismController : MonoBehaviour
{
    [SerializeField] private float RayLenght;
    [SerializeField] private float MagnetismForce;
    private Transform _transform;
    private Rigidbody _myRB;
    private LineRenderer _myLR;
    private RaycastHit hit;
    [SerializeField] private LayerMask Layers;
    [SerializeField] private Material AttractionMaterial;
    [SerializeField] private Material RepulsionMaterial;
    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _myRB = GetComponent<Rigidbody>();
        _myLR = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        SetMaterial();
    }
    void FixedUpdate()
    {
        _myLR.SetPosition(0, _transform.position);
        if (Physics.Raycast(_transform.position, _transform.TransformDirection(Vector3.forward), out hit, RayLenght, Layers))
        {
            _myLR.SetPosition(1, _transform.position + (_transform.TransformDirection(Vector3.forward) * hit.distance));
            Magnetism();
            
        }
        else
        {
            _myLR.SetPosition(1, _transform.position + (_transform.TransformDirection(Vector3.forward) * RayLenght));
            _myRB.velocity = Vector3.Lerp(_myRB.velocity, Vector3.zero, 5f * Time.deltaTime);
        }
    }
    public void SetMaterial()
    {
        if(MagnetismForce < 0)
        {
            _myLR.material = RepulsionMaterial;
        }
        else
        {
            _myLR.material = AttractionMaterial;
        }
    }
    public void Magnetism()
    {
        if (MagnetismForce != 0)
        {
            if (hit.transform.GetComponent<Rigidbody>())
            {
                hit.transform.GetComponent<Rigidbody>().velocity = (transform.position - hit.transform.position).normalized * MagnetismForce;
            }
            _myRB.velocity = (hit.point - transform.position).normalized * MagnetismForce;
            _transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation((hit.point - _transform.position).normalized), 4f * Time.deltaTime);
        }
    }
}