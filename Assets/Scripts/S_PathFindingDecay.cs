using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PathFindingDecay : MonoBehaviour
{

    /// <summary>
    /// unused
    /// </summary>
    private float timer = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer-= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
