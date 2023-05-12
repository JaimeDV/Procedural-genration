using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroudDestroyer : MonoBehaviour
{

    private void Eliminate()
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        WorldGenerator.destroyWorld += Eliminate;
    }
    private void OnDisable()
    {
        WorldGenerator.destroyWorld -= Eliminate;
    }
}
