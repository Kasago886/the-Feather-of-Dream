using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class FinalBossSteps : MonoBehaviour
{
    public string sneakAttackDialog;
    public string phase2Dialog;
    public string phase3Dialog;
    public string phase3_2Dialog;
    public string endDialog;
    public float force;
    public Transform targetLU;
    public Transform targetMU;
    public Transform targetRU;
    public Transform targetLD;
    public Transform targetRD;
    public GameObject attacks;
    public GameObject childAttacks1;
    public GameObject childAttacks2;
    public GameObject codeAttack;
    public GameObject fireParticle;
    public BulletsRain bulletsRain;

    bool moving = false;
    int step = 0;
    Transform target;

    float timer1 = 0;
    float timer2 = 0;

    Dialog dialog;
    Player player;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    FinalBoss finalBoss;
    ExitPanelManager exitPanelManager;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Player>();
        dialog = FindAnyObjectByType<Dialog>();
        finalBoss = GetComponent<FinalBoss>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        exitPanelManager = FindAnyObjectByType<ExitPanelManager>();
        
        target = targetMU;

        StartCoroutine(ExecuteAfterStart());
    }

    /// <summary>
    /// -=| steps |=-
    ///     0      对话
    ///     
    /// 1-4循环
    ///     1、3   空中发射攻击
    ///     2、4   地面冲刺攻击
    ///     
    ///     5      对话
    ///     
    /// 6-9循环
    ///     6、8   空中释放小怪
    ///     7、9   地面发射代码
    ///     
    ///     10      对话
    ///     
    ///     11     地面战斗
    ///     
    ///     12      对话
    ///     13      Ending
    /// </summary>

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            Vector2 direction = target.position - transform.position;
            direction += new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));

            direction = direction.normalized;

            rb.AddForce(direction*force,ForceMode2D.Impulse);


            direction = player.transform.position - transform.position;
            if (direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = true;
            }
        }

        switch (step)
        {
            case 1:
                SkyAttacks(2);

                break;

            case 2:
                LandAttack(3, 1);

                break;

            case 3:
                SkyAttacks(4);

                break;

            case 4:
                LandAttack(1, -1);
                break;

            case 6:
                ChildAttacks(7,childAttacks1);
                break;

            case 7:
                CodeAttack(8);
                break;

            case 8:
                ChildAttacks(9,childAttacks2);
                break;

            case 9:
                CodeAttack(6);
                break;

        }
    }

    void SetStep(int step)
    {
        this.step = step;

        switch (step)
        {
            case 1:
                bulletsRain.rainType = BulletsRain.RainType.RtoL;
                SetSkyAttacks(targetLU);
                break;

            case 2:
                bulletsRain.rainType = BulletsRain.RainType.pause;
                SetLandAttack(targetLD);
                break;

            case 3:
                bulletsRain.rainType = BulletsRain.RainType.LtoR;
                SetSkyAttacks(targetRU);
                break;
            
            case 4:
                bulletsRain.rainType = BulletsRain.RainType.pause;
                SetLandAttack(targetRD);
                break;

            case 6:
                bulletsRain.rainType = BulletsRain.RainType.BothToBoth;
                SetChildAttacks(targetMU);
                break;

            case 7:
                bulletsRain.rainType = BulletsRain.RainType.pause;
                SetLandAttack(targetRD);
                break;

            case 8:
                bulletsRain.rainType = BulletsRain.RainType.BothToBoth;
                SetChildAttacks(targetMU);
                break;

            case 9:
                bulletsRain.rainType = BulletsRain.RainType.pause;
                SetLandAttack(targetLD);
                break;

            case 11:
                moving = false;
                rb.gravityScale = 2;
                finalBoss.unattackable = false;
                finalBoss.enemyAI = true;
                finalBoss.AddFeather(new DefautFeather(100));
                finalBoss.runSpeed = 7;

                fireParticle.SetActive(false);

                break;

            case 13:
                exitPanelManager.LoadScene("EndingScene");
                break;
        }
    }

    public void NextPhase()
    {
        if (step >= 0 && step <= 4)
        {
            bulletsRain.rainType = BulletsRain.RainType.pause;
            finalBoss.collisionAttack = false;
            finalBoss.OnMove(0);
            moving = true;
            target = targetMU;
            UnityEvent unityEvent = new();
            unityEvent.AddListener(() => { SetStep(6); });
            dialog.Read(phase2Dialog, unityEvent);

            SetStep(5);
        }
        else if (step >= 5 && step <= 9)
        {
            bulletsRain.rainType = BulletsRain.RainType.pause;
            finalBoss.collisionAttack = false;
            finalBoss.OnMove(0);
            moving = true;
            target = targetMU;
            UnityEvent unityEvent = new();
            unityEvent.AddListener(() => {StartCoroutine(Phase3_2()); });
            dialog.Read(phase3Dialog, unityEvent);
            finalBoss.animator.Play("DreamPowerAppear");

            SetStep(10);
        }
        else if (step == 11)
        {
            bulletsRain.rainType = BulletsRain.RainType.pause;
            finalBoss.enemyAI = false;
            finalBoss.unattackable = true;
            finalBoss.isDead = true;
            finalBoss.animator.SetBool(Consts.IsDeadAnimatorArgument, true);

            GameObject[] enemys = GameObject.FindGameObjectsWithTag(Consts.EnemyTag);
            foreach (GameObject enemy in enemys)
            {
                if (enemy.GetComponent<FinalBoss>() == null)
                {
                    enemy.SetActive(false);
                }
            }

            UnityEvent unityEvent = new();
            unityEvent.AddListener(() => { SetStep(13); });
            dialog.Read(endDialog, unityEvent);

            SetStep(12);
        }
    }

    void SetSkyAttacks(Transform targetTransform)
    {
        finalBoss.OnMove(0);
        moving = true;
        timer1 = 3;
        timer2 = 5;
        target = targetTransform;
        player.UnlockFeather(1, 5);
    }
    void SkyAttacks(int nextStep)
    {
        timer1 -= Time.deltaTime;
        timer2 -= Time.deltaTime;
        if (timer2 < 0)
        {
            SetStep(nextStep);
            return;
        }
        if (timer1 < 0)
        {
            GameObject attackbodys = Instantiate(attacks);
            attackbodys.transform.position = transform.position;

            timer1 = timer2 + 1;
        }
    }

    void SetLandAttack(Transform targetTransform)
    {
        timer1 = 3;
        timer2 = 6;
        player.UnlockFeather(1, 5);
        target = targetTransform;
    }
    void LandAttack(int nextStep, int direction)
    {
        timer1 -= Time.deltaTime;
        timer2 -= Time.deltaTime;
        if (timer2 < 0)
        {
            finalBoss.collisionAttack = false;
            SetStep(nextStep);
            return;
        }
        if (timer1 < 0)
        {
            finalBoss.collisionAttack = true;
            moving = false;
            finalBoss.OnMove(direction);
        }
    }

    void SetCodeAttack(Transform targetTransform)
    {
        timer1 = 3;
        timer2 = 10;
        player.UnlockFeather(1, 5);
        target = targetTransform;
    }
    void CodeAttack(int nextStep)
    {
        timer1 -= Time.deltaTime;
        timer2 -= Time.deltaTime;
        if (timer2 < 0)
        {
            SetStep(nextStep);
            return;
        }
        if (timer1 < 0)
        {
            GameObject attackbodys = Instantiate(codeAttack);
            attackbodys.transform.position = transform.position;
            attackbodys.GetComponent<AttackBody>().isleft = spriteRenderer.flipX;

            timer1 = timer2 + 1;
        }
    }

    void SetChildAttacks(Transform targetTransform)
    {
        finalBoss.OnMove(0);
        moving = true;
        timer1 = 3;
        timer2 = 10;
        target = targetTransform;
    }
    void ChildAttacks(int nextStep, GameObject childAttack)
    {
        timer1 -= Time.deltaTime;
        timer2 -= Time.deltaTime;
        if (timer2 < 0)
        {
            SetStep(nextStep);
            return;
        }
        if (timer1 < 0)
        {
            GameObject attackbodys = Instantiate(childAttack);
            attackbodys.transform.position = transform.position;

            timer1 = timer2 + 1;
        }
    }

    private IEnumerator ExecuteAfterStart()
    {
        yield return null; // 等待一帧

        if (Vector2.Distance(player.transform.position, gameObject.transform.position) < 100)
        {
            player.UnlockFeather(1, 5);

            UnityEvent unityEvent = new();
            unityEvent.AddListener(() => { SetStep(1); });
            dialog.Read(sneakAttackDialog,unityEvent);

            moving = true;
        }
    }

    private IEnumerator Phase3_2()
    {
        yield return null; // 等待一帧

        UnityEvent unityEvent = new();
        unityEvent.AddListener(() => { SetStep(11); });
        dialog.Read(phase3_2Dialog, unityEvent);
        finalBoss.animator.SetBool("DreamPowerDisappear", true);
    }

}
