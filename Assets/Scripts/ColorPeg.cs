using UnityEngine;

public class ColorPeg : MonoBehaviour
{
    [Header("Peg setup")]
    public int colorID ;           
    public Transform stackPoint;      
    public float ringHeight = 0.12f;  

    int stackCount = 0;               

    public Vector3 GetNextStackPosition()
    {
        var pos = stackPoint.position + Vector3.up * (ringHeight * stackCount);
        stackCount++;                 
        return pos;
    }
}
