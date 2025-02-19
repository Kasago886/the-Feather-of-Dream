using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeprizAttackBody : MonoBehaviour
{
    public GameObject bullet;
    private Transform attackBodyTransform;
    private LineRenderer lineRenderer;
    private Rigidbody2D rb;
    private float timer;
    private Player player;
    private bool ismove;
    void Start()
    {
        Destroy(gameObject,60);
        player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
        attackBodyTransform = GetComponent<Transform>();
        lineRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        lineRenderer.positionCount = 2;
        ismove = true;
        if (player != null)
        {
            attackBodyTransform.position = new Vector2(player.transform.position.x, Camera.main.transform.position.y + Camera.main.orthographicSize+10);
        }
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(Mathf.Abs(player.transform.position.x - transform.position.x) <= 1&&timer>5)
        {
            ChangeColor(bullet);
            timer = 0;
            ismove=false;
            rb.velocity = Vector3.zero;
            Invoke("ReturnMove", 1f);
        }
        if (Mathf.Abs(player.transform.position.x - transform.position.x) > 1 && timer > 5)
        {
            lineRenderer.startColor = Color.yellow;
            lineRenderer.endColor = Color.yellow;
        }
        if (ismove) 
        {
            rb.velocity = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(Camera.main.transform.position.y + Camera.main.orthographicSize + 10 - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg)) * new Vector2(4, 0);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, new Vector2(transform.position.x, transform.position.y - 1000));
        }
        if(GameObject.Find("Dr.Nepriz1")==null&& GameObject.Find("Dr.Nepriz2(Clone)") == null)
        {
            Destroy(gameObject);
        }
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
            Rigidbody2D bulletRB = bulletInstance.GetComponent<Rigidbody2D>();
            bulletRB.velocity = new Vector3(0, -1000, 0);
        }
    }
}
