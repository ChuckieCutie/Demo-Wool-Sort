using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Scene Refs")]
    public ColorPeg[] allPegs;        
    public ColorPeg spawnColumn;       

    [Header("Prefabs")]
    public WoolRing ringPrefab;

    [Header("Settings")]
    public int ringsPerColor = 4;       

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

    void SpawnRingsStackedRandomOnSpawnColumn()
    {
        if (ringPrefab == null || spawnColumn == null)
        {
            Debug.LogError("Thiếu ringPrefab hoặc spawnColumn!");
            return;
        }

        List<int> ringColors = new List<int>();

        foreach (var peg in allPegs)
        {
            for (int i = 0; i < ringsPerColor; i++)
                ringColors.Add(peg.colorID);
        }

        for (int i = 0; i < ringColors.Count; i++)
        {
            int rand = Random.Range(0, ringColors.Count);
            (ringColors[i], ringColors[rand]) = (ringColors[rand], ringColors[i]);
        }

        foreach (int colorID in ringColors)
        {
            Vector3 pos = spawnColumn.GetNextStackPosition(); 

            WoolRing ring = Instantiate(ringPrefab, pos, Quaternion.identity);

            ring.SetColor(colorID); 
            ring.transform.SetParent(spawnColumn.stackPoint, worldPositionStays: true);
        }
    }

    public void SortRing(WoolRing ring)
    {
        if (!pegByColor.TryGetValue(ring.colorID, out var peg)) return;
        ring.MoveToPeg(peg);
    }
}
