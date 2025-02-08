using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    CinemachineVirtualCamera cinemachineVirtualCamera;
    CinemachineConfiner2D confiner;

    float targetSize;
    // Start is called before the first frame update
    void Start()
    {
        cinemachineVirtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        confiner = FindAnyObjectByType<CinemachineConfiner2D>();
        targetSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (cinemachineVirtualCamera.m_Lens.OrthographicSize - targetSize > 0.1f)
        {
            cinemachineVirtualCamera.m_Lens.OrthographicSize -= 0.1f;
        }
        else if (cinemachineVirtualCamera.m_Lens.OrthographicSize - targetSize < -0.1f)
        {
            cinemachineVirtualCamera.m_Lens.OrthographicSize += 0.1f;
        }
        else if (cinemachineVirtualCamera.m_Lens.OrthographicSize != targetSize)
        {
            cinemachineVirtualCamera.m_Lens.OrthographicSize = targetSize;
        }
        confiner.InvalidateCache();
    }

    public void SetCameraSizeSmooth(float size)
    {
        targetSize = size;
    }

    public void SetCameraSize(float size)
    {
        cinemachineVirtualCamera.m_Lens.OrthographicSize = size;
        targetSize = size;
    }

    public void TeleportCamera()
    {
        Transform follow = cinemachineVirtualCamera.Follow;
        cinemachineVirtualCamera.Follow = null;

        Collider2D collider = confiner.m_BoundingShape2D;
        confiner.m_BoundingShape2D = null;

        Camera.main.transform.position = follow.position;
        cinemachineVirtualCamera.transform.position = follow.position;
        StartCoroutine(SetFollow(follow,collider));
    }

    IEnumerator SetFollow(Transform follow, Collider2D collider)
    {
        yield return null;
        cinemachineVirtualCamera.Follow = follow;
        confiner.m_BoundingShape2D = collider;
        confiner.InvalidateCache();
    }
}
