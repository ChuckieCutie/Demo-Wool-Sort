using System.Collections;
using UnityEngine;

public class WoolRing : MonoBehaviour
{
    [Header("Ring setup")]
    public int colorID; 
    public MeshRenderer rend;
    public Material[] colorMaterials;          

    [Header("Movement")]
    public float liftHeight = 1.2f;   
    public float moveSpeed  = 5f;     

    bool isSorted = false;            

    void OnMouseDown()
    {
        if (isSorted) return;        
        GameManager.Instance.SortRing(this);
    }

    public void MoveToPeg(ColorPeg targetPeg)
    {
        if (isSorted || targetPeg == null) return;
        StopAllCoroutines();
        StartCoroutine(MoveRoutine(targetPeg));
    }

    IEnumerator MoveRoutine(ColorPeg peg)
    {
        Vector3 start = transform.position;
        Vector3 end   = peg.GetNextStackPosition();

        Vector3 up    = start + Vector3.up * liftHeight;
        Vector3 mid   = new Vector3(end.x, up.y, end.z);

        yield return MoveStep(up);
        yield return MoveStep(mid);
        yield return MoveStep(end);

        transform.SetParent(peg.stackPoint, worldPositionStays: true);
        transform.position = end;

        isSorted = true; 
    }

    IEnumerator MoveStep(Vector3 target)
    {
        while ((transform.position - target).sqrMagnitude > 0.0001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
    }
}
