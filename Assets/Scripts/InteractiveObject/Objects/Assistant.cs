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
            //ΩÃ≥Ã
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
            //∏˙ÀÊÕÊº“
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

                        //”–«Ω◊Ë∞≠ ±∫ˆ¬‘≈ˆ◊≤
                        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, direction, direction.magnitude, LayerMask.GetMask(Consts.WallLayer));
                        if (hit.Length > 0)
                        {
                            rb.excludeLayers = LayerMask.GetMask(Consts.WallLayer, Consts.PlayerLayer, Consts.EnemyLayer, Consts.InteractiveObjectLayer);
                        }
                        else
                        {
                            rb.excludeLayers = LayerMask.GetMask(Consts.PlayerLayer, Consts.EnemyLayer, Consts.InteractiveObjectLayer);
                        }


                        // ƒ£ƒ‚µØª…¡¶ f = kx
                        //øøΩ¸
                        if (direction.sqrMagnitude > followDistanceMax * followDistanceMax)
                        {
                            Vector2 force = direction.normalized * (direction.magnitude - followDistanceMax) * followForce * Time.deltaTime;
                            rb.AddForce(force, ForceMode2D.Force);
                        }
                        //‘∂¿Î
                        else if (direction.sqrMagnitude < followDistanceMin * followDistanceMin)
                        {
                            //¥¶”⁄Ω≈œ¬ ±Õ˘…œ∑…
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
            
        }
    }

    public void Tutorial(int step)
    {
        if (!ArchiveManager.CheckFlag(FlagType.tutorialDone))
        {
            dialog.Read("Tutorial/Tutorial" + step);
        }
    }

    /// <summary>
    /// ±¨’®
    /// </summary>
    public void Boom()
    {
        isBoom = true;

        rb.gravityScale = 1;
        rb.drag = 0;

        animator.Play("AssistantBoom");
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
