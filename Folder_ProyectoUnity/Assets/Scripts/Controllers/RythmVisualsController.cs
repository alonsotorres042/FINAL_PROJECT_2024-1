using System.Collections;
using UnityEngine;
public class RythmVisualsController : MonoBehaviour
{
    [SerializeField] private Transform Bass;
    [SerializeField] private Transform Other;
    private bool IsOnBeat;
 
    private IEnumerator VisualBeatRef;
    // Start is called before the first frame update
    void Start()
    {
        VisualBeatRef = VisualBeat(Other, 87f);
        StartCoroutine(VisualBeatRef);
    }

    // Update is called once per frame
    void Update()
    {
        VisualBehaviour(Bass, 0.025f, "Left");
        VisualBehaviour(Other, 0.025f, "Right");
    }
    public IEnumerator VisualBeat(Transform victim, float bpm)
    {
        while (true)
        {
            victim.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            yield return new WaitForSeconds(60f / bpm);
        }
    }
    public void VisualBehaviour(Transform victim, float RotationSpeed, string direction)
    {
        ReachRotationValue(victim, RotationSpeed, direction);
        ReachScaleValue(victim);
    }
    public void ReachRotationValue(Transform victim, float Speed, string direction)
    {
        if(direction == "Left")
        {
            victim.rotation *= Quaternion.Euler(0, 0, Speed);
        }
        else if(direction == "Right")
        {
            victim.rotation *= Quaternion.Euler(0, 0, -Speed);
        }
    }
    public void ReachScaleValue(Transform victim)
    {
        victim.localScale = Vector3.Lerp(victim.transform.localScale, Vector3.one, 15f * Time.deltaTime);
    }
    public void DecreaseBassScale()
    {
        Bass.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }
    public void DecreaseOtherScale()
    {
        Other.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }
}