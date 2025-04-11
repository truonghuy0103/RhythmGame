using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float timerDestroy = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timerDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
