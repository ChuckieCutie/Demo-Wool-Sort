using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Scene Refs")]
    public ColorPeg[] allPegs;          // Các peg mục tiêu
    public ColorPeg spawnColumn;        // Cột spawn (stackPoint của nó dùng để xếp ring)

    [Header("Prefabs")]
    public WoolRing ringPrefab;

    [Header("Settings")]
    public int ringsPerColor = 4;       // số vòng mỗi màu

    Dictionary<int, ColorPeg> pegByColor;

    void Awake()
    {
        Instance = this;

        pegByColor = new Dictionary<int, ColorPeg>();
        foreach (var p in allPegs)
        {
            if (!pegByColor.ContainsKey(p.colorID))
                pegByColor.Add(p.colorID, p);
        }
    }

    void Start()
    {
        SpawnRingsStackedRandomOnSpawnColumn();
    }

    // ⭐ Spawn thẳng hàng trên cột SpawnColumn theo kiểu xếp chồng, nhưng màu bị random
    void SpawnRingsStackedRandomOnSpawnColumn()
    {
        if (ringPrefab == null || spawnColumn == null)
        {
            Debug.LogError("Thiếu ringPrefab hoặc spawnColumn!");
            return;
        }

        // 1) Tạo danh sách tất cả colorID cần spawn
        List<int> ringColors = new List<int>();

        foreach (var peg in allPegs)
        {
            for (int i = 0; i < ringsPerColor; i++)
                ringColors.Add(peg.colorID);
        }

        // 2) Shuffle màu
        for (int i = 0; i < ringColors.Count; i++)
        {
            int rand = Random.Range(0, ringColors.Count);
            (ringColors[i], ringColors[rand]) = (ringColors[rand], ringColors[i]);
        }

        // 3) Spawn theo thứ tự random, nhưng xếp đúng stackPosition
        foreach (int colorID in ringColors)
        {
            Vector3 pos = spawnColumn.GetNextStackPosition(); // ⭐ thẳng hàng

            WoolRing ring = Instantiate(ringPrefab, pos, Quaternion.identity);

            ring.SetColor(colorID); // gán màu
            ring.transform.SetParent(spawnColumn.stackPoint, worldPositionStays: true);
        }
    }

    // ⭐ Sort — giữ nguyên
    public void SortRing(WoolRing ring)
    {
        if (!pegByColor.TryGetValue(ring.colorID, out var peg)) return;
        ring.MoveToPeg(peg);
    }
}
