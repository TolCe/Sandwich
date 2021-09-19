using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _moveTime;
    [SerializeField] private float _offset;

    void Start()
    {
        GameEvents.Instance.OnCameraTriggeredEvent += SetCameraToSuccessPoint;
    }

    private void SetCameraToSuccessPoint(Transform target)
    {
        StartCoroutine(MoveCamera(target));
    }

    private IEnumerator MoveCamera(Transform target)
    {
        Vector3 targetPosition = target.position + _offset * Vector3.back + _offset * 0.5f * Vector3.up;
        float distance = Vector3.Distance(targetPosition, transform.position);

        while (Vector3.Distance(targetPosition, transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, distance / _moveTime * Time.fixedDeltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 180 / _moveTime * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
}
