using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Huy_TargetArrow : MonoBehaviour
{
    private bool isPress;

    public bool IsPress
    {
        get => isPress;
        set
        {
            isPress = value;
            if (isPress)
            {
                int countCollider = 0;
                for (int i = 0; i < lsArrows.Count; i++)
                {
                    if (lsArrows[i].isCollider)
                    {
                        countCollider++;
                    }
                }

                if (countCollider == 0)
                {
                    //Set animation fail for Main
                    //Sub HP bar for Main
                }

                if (lsArrows.Count > 0)
                {
                    //Set correct for the first arrow
                    lsArrows[0].SetCorrect();
                }
            }
            else
            {
                if (lsArrows.Count > 0)
                {
                    if (lsArrows[0].isCorrectArrow && lsArrows[0].timerAnim > 0)
                    {
                        lsArrows[0].SetInvisibleTail();
                    }
                }
            }
        }
    }
    
    public List<Huy_Arrow> lsArrows = new List<Huy_Arrow>();

    private int index;

    public void SetCollider(Huy_Arrow arrow)
    {
        if (lsArrows.Count == 0 || !lsArrows.Contains(arrow))
        {
            lsArrows.Add(arrow);
        }
    }

    public void ExitCollider(Huy_Arrow arrow)
    {
        if (lsArrows.Contains(arrow))
        {
            lsArrows.Remove(arrow);
        }
    }
}
