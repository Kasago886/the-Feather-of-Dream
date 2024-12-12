using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Controllers/PlayerController")]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float jumpSpeed;

    public float bottomCenterX, bottomCenterY;
    Vector2 bottomCenterGlobal;
    public Vector2 bottomSize;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 左右移动
    /// </summary>
    /// <param name="horizontal">Horizontal speed scale. Range from -1 to 1.</param>
    public void OnMove(float horizontal)
    {
        rb.velocity = new Vector2(horizontal * walkSpeed, rb.velocity.y);
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    public void OnJump()
    {
        //检测是否踩在地面上
        bottomCenterGlobal = transform.position + new Vector3(bottomCenterX,bottomCenterY);
        Collider2D[] hit = Physics2D.OverlapBoxAll(bottomCenterGlobal,bottomSize,0,LayerMask.GetMask(Consts.GroundLayer));
        if (hit.Length > 0)
        {
            Debug.Log(hit[0]);
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        bottomCenterGlobal = transform.position + new Vector3(bottomCenterX, bottomCenterY);
        Gizmos.DrawWireCube(bottomCenterGlobal,bottomSize);
    }
}
