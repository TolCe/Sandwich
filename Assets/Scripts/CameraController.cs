using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Start()
    {
        GameEvents.Instance.OnLevelSuccessEvent += SetCameraToSuccessPoint;
    }

    private void SetCameraToSuccessPoint()
    {

    }
}
