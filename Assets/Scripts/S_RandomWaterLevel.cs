using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_RandomWaterLevel : MonoBehaviour
{
    
    void Start()
    {
        var random = Random.Range(0.1f, 5);
        this.gameObject.transform.localScale +=new Vector3(0,random,0);
    }

}
