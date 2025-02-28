using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public GameObject bullet;
    [HideInInspector]
    public List<string> buffNameAndNumbers = new List<string>();
    [HideInInspector]
    public float damage;
    private List<GameObject> enemys;
    private GameObject enemy;
    private GameObject player;
    private Rigidbody2D rb;
    private float timer;
    private LineRenderer lineRenderer;
    private bool ismove;
    // Start is called before the first frame update
    void Start()
    {
        ismove = true;
        enemys = new List<GameObject>();
        enemys.AddRange(GameObject.FindGameObjectsWithTag(Consts.EnemyTag));
        player=GameObject.FindGameObjectWithTag(Consts.PlayerTag);
        rb=GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag(Consts.EnemyTag).Length != enemys.Count)
        {
            enemys.Clear();
            enemys.AddRange(GameObject.FindGameObjectsWithTag(Consts.EnemyTag));
        }
        if (player != null)
        {
            enemy = GetTheMinDes(player,enemys,25);
            if (enemy != null)
            {
                timer += Time.deltaTime;
                if (Mathf.Abs(enemy.transform.position.x - transform.position.x) <= 0.5f && timer > 10)
                {
                    ChangeColor(bullet);
                    timer = 0;
                    ismove = false;
                    rb.velocity = Vector3.zero;
                    Invoke("ReturnMove", 1f);
                }
                if (Mathf.Abs(enemy.transform.position.x - transform.position.x) > 1 && timer > 5)
                {
                    lineRenderer.startColor = Color.yellow;
                    lineRenderer.endColor = Color.yellow;
                }
                if (ismove)
                {
                    rb.velocity = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(Camera.main.transform.position.y + Camera.main.orthographicSize + 10 - transform.position.y, enemy.transform.position.x - transform.position.x) * Mathf.Rad2Deg)) * new Vector2(4, 0);
                }
            }
            else
            {
                if (transform.position.x<player.transform.position.x-5&& transform.position.x > player.transform.position.x - 6)
                {
                    rb.velocity = Vector3.zero;
                }
                else
                {
                    rb.velocity = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(Camera.main.transform.position.y + Camera.main.orthographicSize + 10 - transform.position.y, player.transform.position.x-5 - transform.position.x) * Mathf.Rad2Deg)) * new Vector2(4, 0);
                }
            }
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, new Vector2(transform.position.x, transform.position.y - 1000));
        }
        else
        {
            Destroy(gameObject);
        }
    }
    GameObject GetTheMinDes(GameObject center, List<GameObject> others, float minDistance)
    {
        int min = 0;
        float distance = 0;
        for (int i = 0; i < others.Count; i++)
        {
            if (i == 0)
            {
                distance = Vector2.Distance(center.transform.position, others[i].transform.position);
            }
            if (i > 0 && distance > Vector2.Distance(center.transform.position, others[i].transform.position))
            {
                distance = Vector2.Distance(center.transform.position, others[i].transform.position);
                min = i;
            }
        }
        if (distance < minDistance)
        {
            return others[min];
        }
        return null;
    }
    private void ChangeColor(GameObject bullet)
    {
        RedColor();
        Invoke("BaseColor", 0.1f);
        Invoke("RedColor", 0.2f);
        Invoke("BaseColor", 0.3f);
        Invoke("RedColor", 0.4f);
        Invoke("BaseColor", 0.5f);
        Invoke("RedColor", 0.6f);
        Invoke("Fire", 0.5f);
        Invoke("BaseColor", 1f);
    }
    private void BaseColor()
    {
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
    }
    private void RedColor()
    {
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }
    private void ReturnMove()
    {
        ismove = true;
    }
    private void Fire()
    {
        if (bullet != null)
        {
            GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
            NeprizBullet neprizBullet=bulletInstance.GetComponent<NeprizBullet>();
            neprizBullet.damage += damage;
            foreach (var buff in buffNameAndNumbers)
            {
                neprizBullet.buffs.Add(buff);
            }
            Rigidbody2D bulletRB = bulletInstance.GetComponent<Rigidbody2D>();
            bulletRB.velocity = new Vector3(0, -1000, 0);
        }
    }
}
