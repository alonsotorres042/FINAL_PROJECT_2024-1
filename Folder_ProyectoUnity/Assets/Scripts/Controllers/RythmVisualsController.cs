using System.Collections;
using UnityEngine;
public class RythmVisualsController : MonoBehaviour
{
    [SerializeField] private Transform Bass;
    [SerializeField] private Transform Other;
    private bool IsOnBass;
 
    private IEnumerator VisualBeatRef;
    // Start is called before the first frame update
    void Start()
    {
        VisualBeatRef = VisualBeat(87f);
        //StartCoroutine(VisualBeatRef);
    }

    // Update is called once per frame
    void Update()
    {
        VisualBehaviour(Bass, 0.05f, "Left");
        VisualBehaviour(Other, 0.05f, "Right");
    }
    public IEnumerator VisualBeat(float bpm)
    {
        while (true)
        {
            IsOnBass = true;
            yield return new WaitForSeconds((60f / bpm) - ((60f / bpm) / 10f));
            IsOnBass = false;
            yield return new WaitForSeconds((60f / bpm) / 10f);
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