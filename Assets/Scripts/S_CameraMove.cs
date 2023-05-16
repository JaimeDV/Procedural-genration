using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CameraMove : MonoBehaviour
{


    /// <summary>
    /// Moves the camera + and - zooms
    /// unsued since is now first person
    /// </summary>
    /// 
    private Vector3 startPosition;

    [SerializeField]
    private float factor;
    private void Start()
    {
        startPosition=gameObject.transform.position;
    }
    void Update()
    {
 
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-factor , 0,0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(factor , 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 0, factor );
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, 0, -factor );
        }
        transform.position -= new Vector3(0, Input.mouseScrollDelta.y * factor,0);

    }
}
