using UnityEngine;

public class TheMisunderstoodWerewolfSkill : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [HideInInspector]
    public static bool useSkill;
    [HideInInspector]
    public static float speed;
    [HideInInspector]
    public static float limitR;
    private float r;
    [HideInInspector]
    public static int number;
    private float timer;
    private Player player;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        player= GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(useSkill)
        {
            Debug.Log("useSkill");
            timer += Time.deltaTime;
            lineRenderer.enabled = true;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            r += speed*Time.deltaTime;
            Debug.Log("R="+r);
            lineRenderer.positionCount = 361;
            for (int i = 0; i < 361; i++)
            {
                float x = transform.position.x + r * Mathf.Cos(i * Mathf.PI / 180f);
                float y = transform.position.y + r * Mathf.Sin(i * Mathf.PI / 180f);
                lineRenderer.SetPosition(i, new Vector3(x, y, 0)); 
            }
            if (Vector2.Distance(transform.position, player.transform.position) < r && timer > 1 && number > 0)
            {
                player.AddBuff("¾ª»Ì");
                timer = 0;
            }
            if (r > limitR)
            {
                r = 0;
                useSkill = false;
                lineRenderer.enabled = false;
            }
        }
    }
}
