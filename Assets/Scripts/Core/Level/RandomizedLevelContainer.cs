using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomizedLevelContainer", menuName = "Randomized Level Container")]
public class RandomizedLevelContainer : ScriptableObject
{
    public List<GridVO> Grids;

    public GridVO this[int index]
    {
        get
        {
            return Grids[index];
        }
    }
}
