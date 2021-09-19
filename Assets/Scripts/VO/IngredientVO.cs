using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class IngredientVO
{
    public bool IsIngredient;
    [ShowIf("IsIngredient")]
    public IngredientTypes IngredientType;
    [HideIf("IsIngredient")]
    public int Value = 2;
}
