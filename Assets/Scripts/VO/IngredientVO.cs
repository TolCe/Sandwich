using Sirenix.OdinInspector;

[System.Serializable]
public class IngredientVO
{
    public bool IsIngredient;
    [ShowIf("IsIngredient")]
    public IngredientTypes IngredientType;
    [HideIf("IsIngredient")]
    public int Value = 2;
}
