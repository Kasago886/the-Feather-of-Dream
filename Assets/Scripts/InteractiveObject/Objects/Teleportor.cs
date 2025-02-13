using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Teleportor : InteractiveObject
{
    [Header("传送至相对位置")]
    public Vector2 TeleportPosition;
    Vector3 TeleportPosition3;
    [Header("传送至某物体")]
    public bool objectTeleport;
    public GameObject targetObj;
    public bool locked;
    [Header("改变相机大小")]
    public bool isCameraChange;
    public float size;

    CameraManager cameraManager;
    Assistant assistant;
    protected override void Start()
    {
        base.Start();
        cameraManager = FindAnyObjectByType<CameraManager>();
        assistant = FindAnyObjectByType<Assistant>();

        TeleportPosition3 = TeleportPosition;
    }
    public override void Interact()
    {
        base.Interact();
        if (!locked)
        {
            if (!objectTeleport)
            {
                StartCoroutine(Teleport(transform.position + TeleportPosition3));
            }
            else
            {
                //Debug.Log("this:"+gameObject+"  target:"+targetObj);
                StartCoroutine(Teleport(targetObj.transform.position));
            }
        }
    }

    //使用协程防止同一帧内触发两个地点的传送
    IEnumerator Teleport(Vector3 position)
    {
        yield return null;
        player.transform.position = position;
        if (assistant != null)
        {
            assistant.transform.position = position + new Vector3(-1,1);
        }

        if (isCameraChange)
            cameraManager.SetCameraSize(size);
    }

    protected override void OnDrawGizmos()
    {
        base .OnDrawGizmos();
        Gizmos.color = Color.yellow;

        if (!objectTeleport)
        {
            TeleportPosition3 = TeleportPosition;
            Gizmos.DrawLine(transform.position, transform.position + TeleportPosition3);
        }
        else if (targetObj != null) 
        {
            Gizmos.DrawLine(transform.position, targetObj.transform.position);
        }
    }
}
