using UnityEngine;

public class S_RandomMove : MonoBehaviour
{
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private GameObject endpoint;

    [SerializeField]
    private float timer;

    [SerializeField]
    private float minTimer;

    [SerializeField]
    private float maxTimer;
     /// <summary>
     /// updated version of random move, only puts the endpoint on nonwalls 
     /// </summary>

    private void Start()
    {
        //grid = GetComponent<Grid>();
    }

    private void Update()
    {
       
            timer -= 1 * Time.deltaTime;
            if (timer < 0)
            {
                changePath();
                float random = Random.Range(minTimer, maxTimer);
                timer += random;
            }
        
    }

    private Node setPaths()
    {
        if (grid.nodeArray != null)
        {
            foreach (Node node in grid.nodeArray)
            {
                float random = Random.Range(0, 10000);
                if (random < 5 && node.isNotWall)
                {
                    return node;
                }
            }
        }
        return null;
    }

    private void changePath()
    {
        var newEnd = setPaths();

        if (newEnd != null)
        {
            endpoint.transform.position = newEnd.position;
        }
        else
        {
            changePath();
        }
    }
}