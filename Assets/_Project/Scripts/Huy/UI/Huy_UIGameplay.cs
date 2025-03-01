using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Huy_UIGameplay : MonoBehaviour
{
    [SerializeField] private List<Image> lsArrowTops = new List<Image>();
    
    [SerializeField] private Transform transTarget;
    public List<Transform> lsTransSpawnArrowBot = new List<Transform>();

    public float GetDistanceMoveArrow()
    {
        return Vector2.Distance(transTarget.position, lsTransSpawnArrowBot[0].position);
    }

    public List<Transform> GetListTransformSpawnArrow()
    {
        return lsTransSpawnArrowBot;
    }

    public List<Transform> GetListTargetArrow()
    {
        List<Transform> lsTransTarget = new List<Transform>();
        for (int i = 0; i < lsArrowTops.Count; i++)
        {
            lsTransTarget.Add(lsArrowTops[i].transform);
        }
        
        return lsTransTarget;
    }
}
