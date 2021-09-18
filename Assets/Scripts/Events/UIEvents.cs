using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public static UIEvents Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public event Action<UIManager.InputDelegate> OnAssignInputEvent;
    public void AssignInput(UIManager.InputDelegate inputDel)
    {
        if (OnAssignInputEvent != null)
        {
            OnAssignInputEvent(inputDel);
        }
    }

    public event Action<UIManager.EmptyDelegate> OnAssignStartUIEvent;
    public void AssignUI(UIManager.EmptyDelegate emptyDel)
    {
        if (OnAssignStartUIEvent != null)
        {
            OnAssignStartUIEvent(emptyDel);
        }
    }
}
