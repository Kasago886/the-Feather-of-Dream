using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPushBox : MonoBehaviour
{
    public float speed;

    List<Vector3> targetPositions = new();
    bool moving = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            if (targetPositions.Count > 0)
            {
                Vector3 targetPosition = targetPositions[0];
                Vector3 direction = targetPosition - transform.position;

                transform.position += direction.normalized * speed * Time.fixedDeltaTime;
                if (Vector2.Distance(transform.position, targetPosition) < 0.5f)
                {
                    transform.position = targetPosition;

                    targetPositions.RemoveAt(0);
                }
            }
        }
    }

    /// <summary>
    /// 检查能否移动
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public bool CheckMove(Vector3 direction)
    {
        Vector3 originPosition;
        if (targetPositions.Count > 0)
        {
            originPosition = targetPositions[^1];
        }
        else
        {
            originPosition = transform.position;
        }
        //推箱子
        Collider2D[] hit = Physics2D.OverlapPointAll(originPosition + direction, LayerMask.GetMask(Consts.PushBoxLayer,Consts.WallLayer));
        if (hit.Length > 0)
        {
            return false;
        }
        return true;
    }

    public void SetTargetPositionByDirection(Vector3 direction)
    {
        Vector3 originPosition;
        if (targetPositions.Count > 0)
        {
            originPosition = targetPositions[^1];
        }
        else
        {
            originPosition = transform.position;
        }
        targetPositions.Add(originPosition + direction);
    }

    public void SetMoving(bool moving)
    {
        this.moving = moving;
    }
}
