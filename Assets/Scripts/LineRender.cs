using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class LineRender : MonoBehaviour
{
    LineRenderer line; 
    Transform target; 
    NavMeshAgent agent;

   
    void Start()
    {
        line = GameObject.Find("Cylinder").GetComponent<LineRenderer>();
        agent = GameObject.Find("Cylinder").GetComponent<NavMeshAgent>(); 
        getPath();
       
    }


    IEnumerator getPath()
    {
        line.SetPosition(0, transform.position); 

        agent.SetDestination(target.position);
        yield return new WaitForEndOfFrame();

        DrawPath(agent.path);

        agent.Stop();
        
    }

    
    void DrawPath(NavMeshPath path)
    {
        if (path.corners.Length < 2) //if the path has 1 or no corners, there is no need
            return;

        line.SetVertexCount(path.corners.Length); //set the array of positions to the amount of corners

        for (var i = 1; i < path.corners.Length; i++)
        {
            line.SetPosition(i, path.corners[i]); //go through each corner and set that to the line renderer's position
        }
    }





    // Update is called once per frame
    void Update()
    {
        
    }
}
