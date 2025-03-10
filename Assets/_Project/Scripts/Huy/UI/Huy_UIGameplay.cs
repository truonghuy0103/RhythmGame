using System.Collections;
using System.Collections.Generic;
using Huy;
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

    public void OnButtonClickDown(int index)
    {
        Huy_GameManager.Instance.OnButtonClickDown(index);
    }

    public void OnButtonClickUp(int index)
    {
        Huy_GameManager.Instance.OnButtonClickUp(index);
    }

    private void Update()
    {
        //Key down
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnButtonClickDown(0);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnButtonClickDown(1);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnButtonClickDown(2);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            OnButtonClickDown(3);
        }
        
        //Key up
        if (Input.GetKeyUp(KeyCode.A))
        {
            OnButtonClickUp(0);
        }
        
        if (Input.GetKeyUp(KeyCode.S))
        {
            OnButtonClickUp(1);
        }
        
        if (Input.GetKeyUp(KeyCode.W))
        {
            OnButtonClickUp(2);
        }
        
        if (Input.GetKeyUp(KeyCode.D))
        {
            OnButtonClickUp(3);
        }
    }
}
