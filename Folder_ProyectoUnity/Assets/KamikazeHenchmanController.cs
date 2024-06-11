using DG.Tweening;

public class KamikazeHenchmanController : EnemyClass
{
    // Start is called before the first frame update
    void Awake()
    {
        AttackTarget = eventManagerData.Player.transform;
        CurrentLife = MaxLife;
    }
    void Start()
    {
        DOTween.Init();
        transform.DOMove(AttackTarget.position, 1.5f).SetEase(Ease.InOutElastic);
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentLife <= 0)
        {
            Death();
        }
    }
}