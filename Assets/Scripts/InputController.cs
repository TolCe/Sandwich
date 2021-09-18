using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private bool _pressing;

    private void Start()
    {
        UIEvents.Instance.AssignInput(GetInput);
    }

    private void SendRay()
    {
        int mask = (1 << 6);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            GameEvents.Instance.TouchDown(hit.collider.GetComponent<Ingredient>());
        }
    }

    private void GetInput(bool state)
    {
        if (state)
        {
            _pressing = true;
            StartCoroutine(DetectDirection());
            SendRay();
        }
        else
        {
            _pressing = false;
        }
    }

    private IEnumerator DetectDirection()
    {
        Vector3 initMousePos = Input.mousePosition;
        Vector3 direction = Vector3.zero;

        while (_pressing)
        {
            direction = Input.mousePosition - initMousePos;
            yield return new WaitForEndOfFrame();
        }

        SendDirection(new Vector3(direction.x / Screen.width, direction.y / Screen.height, 0));
    }

    private void SendDirection(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                direction = Vector3.right;
            }
            else if (direction.x < 0)
            {
                direction = Vector3.left;
            }
        }
        else if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y > 0)
            {
                direction = Vector3.up;
            }
            else if (direction.y < 0)
            {
                direction = Vector3.down;
            }
        }

        GameEvents.Instance.IngredientSelected(direction);
    }
}
