using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerIn21 : MonoBehaviour
{
    public float speed;
    List<Vector3> targetPositions = new();
    List<PlayerPushBox> pushBoxes = new();
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
        //transform移动
        if (targetPositions.Count > 0)
        {
            PlayerPushBox box = pushBoxes[0];
            if (box != null)
            {
                box.SetMoving(true);
            }

            Vector3 targetPosition = targetPositions[0];
            Vector3 direction = targetPosition - transform.position;
            transform.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector3.down, direction));

            transform.position += direction.normalized * speed * Time.fixedDeltaTime;
            if (Vector2.Distance(transform.position, targetPosition) < 0.5f)
            {
                transform.position = targetPosition;

                targetPositions.RemoveAt(0);
                pushBoxes.RemoveAt(0);
            }
        }
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Vector3 direction)
    {
        direction = direction.normalized * 2;
        
        Vector3 originPosition;
        if (targetPositions.Count > 0)
        {
            originPosition = targetPositions[^1];
        }
        else
        {
            originPosition = transform.position;
        }

        //撞墙
        Collider2D[] hitWall = Physics2D.OverlapPointAll(originPosition + direction, LayerMask.GetMask(Consts.WallLayer));
        if(hitWall.Length <= 0)
        {
            bool canMove = true;
            //推箱子
            Collider2D[] hitBox = Physics2D.OverlapPointAll(originPosition + direction, LayerMask.GetMask(Consts.PushBoxLayer));
            if (hitBox.Length > 1)
            {
                Debug.LogWarning("PushBoxes Overlapped!");
            }
            if (hitBox.Length >= 1)
            {
                PlayerPushBox box = hitBox[0].GetComponent<PlayerPushBox>();
                if (box != null)
                {
                    if (!box.CheckMove(direction))
                    {
                        canMove = false;
                    }
                    else
                    {
                        box.SetTargetPositionByDirection(direction);
                        pushBoxes.Add(box);
                    }
                }
            }
            else
            {
                pushBoxes.Add(null);
            }

            if (canMove)
            {
                targetPositions.Add(originPosition + direction);
            }
        }
    }
}
