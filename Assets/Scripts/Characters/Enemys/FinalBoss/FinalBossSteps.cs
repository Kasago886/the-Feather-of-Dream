using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class FinalBossSteps : MonoBehaviour
{
    public string sneakAttackDialog;
    public float force;
    public Transform targetLU;
    public Transform targetMU;
    public Transform targetRU;
    public Transform targetLD;
    public Transform targetRD;
    public GameObject attacks;

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
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Player>();
        dialog = FindAnyObjectByType<Dialog>();
        finalBoss = GetComponent<FinalBoss>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        target = targetMU;

        StartCoroutine(ExecuteAfterStart());
    }

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
        }
    }

    void SetStep(int step)
    {
        this.step = step;

        switch (step)
        {
            case 1:
                SetSkyAttacks(targetLU);
                break;

            case 2:
                SetLandAttack(targetLD);
                break;

            case 3:
                SetSkyAttacks(targetRU);
                break;
            
            case 4:
                SetLandAttack(targetRD);
                break;
        }
    }

    public void NextPhase()
    {
        if (step >= 0 && step <= 4)
        {
            SetStep(5);
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

    private IEnumerator ExecuteAfterStart()
    {
        yield return null; // µÈ´ýÒ»Ö¡

        if (Vector2.Distance(player.transform.position, gameObject.transform.position) < 100)
        {
            player.UnlockFeather(1, 5);

            UnityEvent unityEvent = new();
            unityEvent.AddListener(() => { SetStep(1); });
            dialog.Read(sneakAttackDialog,unityEvent);

            moving = true;
        }
    }

}
