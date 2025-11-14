using System.Collections;
using UnityEngine;

public class WoolRing : MonoBehaviour
{
    [Header("Ring setup")]
    public int colorID;                     // ID màu
    public MeshRenderer rend;              // Renderer của ring
    public Material[] colorMaterials;      // Mảng material, index = colorID

    [Header("Movement")]
    public float liftHeight = 1.2f;
    public float moveSpeed  = 5f;

    bool isSorted = false;

    void Start()
    {
        ApplyColor();                      // Lúc mới spawn thì tự gán material theo ID hiện tại
    }

    void OnMouseDown()
    {
        if (isSorted) return;
        GameManager.Instance.SortRing(this);
    }

    // Hàm để set màu từ code (GameManager gọi)
    public void SetColor(int id)
    {
        colorID = id;
        ApplyColor();
    }

    // Gán material theo colorID
    public void ApplyColor()
    {
        if (rend == null || colorMaterials == null || colorMaterials.Length == 0)
            return;

        if (colorID >= 0 && colorID < colorMaterials.Length)
        {
            rend.material = colorMaterials[colorID];
        }
        else
        {
            Debug.LogWarning($"colorID {colorID} out of range on {name}");
        }
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

        Vector3 up  = start + Vector3.up * liftHeight;
        Vector3 mid = new Vector3(end.x, up.y, end.z);

        // đi lên
        yield return MoveStep(up);
        // đi ngang
        yield return MoveStep(mid);
        // hạ xuống
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
