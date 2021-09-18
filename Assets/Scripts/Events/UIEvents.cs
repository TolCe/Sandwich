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

    public event Action<UIManager.StartDelegate> OnAssignStartUIEvent;
    public void AssignStartUI(UIManager.StartDelegate startDel)
    {
        if (OnAssignStartUIEvent != null)
        {
            OnAssignStartUIEvent(startDel);
        }
    }
    public event Action<UIManager.NextLevelDelegate, UIManager.RestartDelegate> OnAssignUILevelButtonsEvent;
    public void AssignUILevelButtons(UIManager.NextLevelDelegate nextDel, UIManager.RestartDelegate restartDel)
    {
        if (OnAssignUILevelButtonsEvent != null)
        {
            OnAssignUILevelButtonsEvent(nextDel, restartDel);
        }
    }
}
