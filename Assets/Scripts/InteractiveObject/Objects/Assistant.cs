using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Assistant : MonoBehaviour
{
    public bool followPlayer;
    public float followDistanceMax;
    public float followDistanceMin;
    public float followForce;

    bool isBoom = false;
    bool startTutorialed = false;
    bool pauseEverything = false;
    float pauseSpeed = 0.0001f;
    float beforePauseSpeed = 1;

    Player player;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator animator;
    ArchiveManager archiveManager;
    Dialog dialog;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Player>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        archiveManager = FindAnyObjectByType<ArchiveManager>();
        dialog = FindAnyObjectByType<Dialog>();

        if (archiveManager.currentArchive.levelInfo.AssistantDestroyed)
        {
            isBoom = true;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBoom)
        {
            //教程
            if (archiveManager.currentArchive.levelInfo.level == 0)
            {
                if (!startTutorialed && !ArchiveManager.CheckFlag(FlagType.tutorialDone))
                {
                    PlayerController playerController = player.GetComponent<PlayerController>();
                    playerController.SetPause(true);

                    UnityEvent unityEvent = new();
                    unityEvent.AddListener(playerController.SwitchPause);

                    dialog.Read("Tutorial/Tutorial1",unityEvent);

                    startTutorialed = true;
                    followPlayer = true;
                }
            }
            //跟随玩家
            if (followPlayer)
            {
                if (player != null)
                {
                    Vector2 direction = (player.transform.position + new Vector3(0, 1) - transform.position);
                    if (direction.sqrMagnitude == 0)
                    {
                        transform.position += Vector3.left * 0.001f;
                    }
                    else
                    {
                        if (direction.x > 0)
                        {
                            spriteRenderer.flipX = false;
                        }
                        else
                        {
                            spriteRenderer.flipX = true;
                        }

                        //有墙阻碍时忽略碰撞
                        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, direction, direction.magnitude, LayerMask.GetMask(Consts.WallLayer));
                        if (hit.Length > 0)
                        {
                            rb.excludeLayers = LayerMask.GetMask(Consts.WallLayer, Consts.PlayerLayer, Consts.EnemyLayer, Consts.InteractiveObjectLayer);
                        }
                        else
                        {
                            rb.excludeLayers = LayerMask.GetMask(Consts.PlayerLayer, Consts.EnemyLayer, Consts.InteractiveObjectLayer);
                        }


                        // 模拟弹簧力 f = kx
                        //靠近
                        if (direction.sqrMagnitude > followDistanceMax * followDistanceMax)
                        {
                            Vector2 force = direction.normalized * (direction.magnitude - followDistanceMax) * followForce * Time.deltaTime;
                            rb.AddForce(force, ForceMode2D.Force);
                        }
                        //远离
                        else if (direction.sqrMagnitude < followDistanceMin * followDistanceMin)
                        {
                            //处于脚下时往上飞
                            if (direction.y > 0)
                            {
                                direction += new Vector2(0, followDistanceMin * 2);
                            }
                            Vector2 force = direction.normalized * (direction.magnitude - followDistanceMin) * followForce * Time.deltaTime;
                            rb.AddForce(force, ForceMode2D.Force);
                        }
                    }
                }
            }

            //近似暂停（为了部分动画正常播放）
            if (pauseEverything)
            {
                if (Time.timeScale != beforePauseSpeed && Time.timeScale != pauseSpeed && Time.timeScale != 0)
                {
                    beforePauseSpeed = Time.timeScale;
                }
                Time.timeScale = pauseSpeed;
            }
            
        }
    }

    public void Tutorial(int step)
    {
        if (!ArchiveManager.CheckFlag(FlagType.tutorialDone))
        {
            beforePauseSpeed = Time.timeScale;
            pauseEverything = true;

            UnityEvent unityEvent = new();
            unityEvent.AddListener(() => { 
                Time.timeScale = GetBeforePauseSpeed(); 
                pauseEverything = false; 
            });
            dialog.Read("Tutorial/Tutorial" + step,unityEvent);
        }
    }

    /// <summary>
    /// 爆炸
    /// </summary>
    public void Boom()
    {
        isBoom = true;

        rb.gravityScale = 1;
        rb.drag = 0;

        animator.Play("AssistantBoom");
    }

    float GetBeforePauseSpeed()
    {
        return beforePauseSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        player = FindAnyObjectByType<Player>();
        Gizmos.DrawLine(transform.position, player.transform.position + new Vector3(0, 1));

        float r = followDistanceMax;
        for (int i = 0; i < 360; i++)
        {
            Gizmos.DrawLine(new Vector3(transform.position.x + r * Mathf.Cos((i * Mathf.PI) / 180), transform.position.y + r * Mathf.Sin((i * Mathf.PI) / 180), 0), new Vector3(transform.position.x + r * Mathf.Cos(((i + 1) * Mathf.PI) / 180), transform.position.y + r * Mathf.Sin(((i + 1) * Mathf.PI) / 180), 0));
        }
        r = followDistanceMin;
        for (int i = 0; i < 360; i++)
        {
            Gizmos.DrawLine(new Vector3(transform.position.x + r * Mathf.Cos((i * Mathf.PI) / 180), transform.position.y + r * Mathf.Sin((i * Mathf.PI) / 180), 0), new Vector3(transform.position.x + r * Mathf.Cos(((i + 1) * Mathf.PI) / 180), transform.position.y + r * Mathf.Sin(((i + 1) * Mathf.PI) / 180), 0));
        }
    }
}
