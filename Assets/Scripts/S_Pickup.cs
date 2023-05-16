using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Pickup : MonoBehaviour
{
    [SerializeField]
    private GameObject father;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Destroy(father);   
        }
    }
}
