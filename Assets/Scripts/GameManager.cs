using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Scene Refs")]
    public ColorPeg[] allPegs; 

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

    public void SortRing(WoolRing ring)
    {
        if (!pegByColor.TryGetValue(ring.colorID, out var peg)) return;
        ring.MoveToPeg(peg);
    }
}
