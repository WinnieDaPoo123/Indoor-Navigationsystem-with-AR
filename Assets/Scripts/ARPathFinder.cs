using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ARPathFinder : MonoBehaviour
{
    public Camera ARCamera; 
    public Transform target; 
    private NavMeshPath path;
    private LineRenderer lineRenderer;
    public float maxNavMeshDistance = 50.0f; 
    private static Dictionary<string, LineRenderer> navMeshDict = new Dictionary<string, LineRenderer>();  //string-> name of object(target)
    public float stopDistance = 1.5f; 
    private bool isPathActive = false; 
    private bool isFinished = false;

    void Start()
    {
        isFinished = false;
        path = new NavMeshPath();
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 0;
        Debug.LogWarning("Target: " + target);


        if (target == null)
        {
            Debug.LogError("Target is not assigned in Start.");
            return;
        }
        if (!navMeshDict.ContainsKey(target.name))
        {
            navMeshDict[target.name] = lineRenderer; //add to dictionary
        }
        //TODO: move to a method
        Debug.LogWarning("DISABLING target" + navMeshDict.Count);
        foreach (var kvp in navMeshDict)
        {

            if (kvp.Key != target.name)
            {
                (kvp.Value as LineRenderer).enabled = false;
                Debug.LogWarning("target Key: " + kvp.Key + " Value:" + kvp.Value + "false");
            }
            else
            {
                if (!(kvp.Value as LineRenderer).enabled) //is it already enabled?
                {
                    (kvp.Value as LineRenderer).enabled = true;
                    Debug.LogWarning("target Key: " + kvp.Key + " Value:" + kvp.Value + "true");
                }
            }
        }

    }

    void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned.");
            return;
        }
        else if (isFinished)
        {
            Debug.LogWarning("it is finished!!!!");
            navMeshDict = new Dictionary<string, LineRenderer>(); //new dictionary (re-initialize)
            SceneManager.LoadScene("SampleScene");

            return;
        }
        else
        {
            foreach (var kvp in navMeshDict)
            {
                
                if (kvp.Key != target.name)
                {
                    (kvp.Value as LineRenderer).enabled = false;
                    Debug.LogWarning("target Key: " + kvp.Key + " Value:" + kvp.Value + "false");
                }
                else
                {
                    if (!(kvp.Value as LineRenderer).enabled)
                    {
                        (kvp.Value as LineRenderer).enabled = true;
                        Debug.LogWarning("target Key: " + kvp.Key + " Value:" + kvp.Value + "true");
                    }
                }
            }
        }

        
        float distanceToTarget = Vector3.Distance(ARCamera.transform.position, target.position);
        if (distanceToTarget <= stopDistance)
        {
            lineRenderer.positionCount = 0;
            if (isPathActive)
            {
                isPathActive = false;
                Debug.LogWarning("You have reached the target.");
                lineRenderer.enabled = false;
                isPathActive = false;
                isFinished = true;
            }
            return;
        }

        NavMeshHit hit;   //length to target
        if (NavMesh.SamplePosition(target.position, out hit, maxNavMeshDistance, NavMesh.AllAreas))
        {
            if (NavMesh.CalculatePath(ARCamera.transform.position, hit.position, NavMesh.AllAreas, path))
            {
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    DrawPath(path);
                    isPathActive =true;
                }
                else
                {
                    Debug.LogWarning("Path calculation failed or path is incomplete.");
                    lineRenderer.positionCount = 0;
                    isPathActive = false;
                }
            }
            else
            {
                Debug.LogWarning("Path calculation failed.");
                lineRenderer.positionCount = 0;
                isPathActive = false;
            }
        }
        else
        {
            Debug.LogWarning("Target position is not on the NavMesh.");
            lineRenderer.positionCount = 0;
            isPathActive = false;
        }
    }

    void DrawPath(NavMeshPath path)
    {
        if (path.corners.Length < 2)
        {
            Debug.LogWarning("Path does not have enough corners.");
            return;
        }

        lineRenderer.positionCount = path.corners.Length;
        for (int i = 0; i < path.corners.Length; i++)
        {
            lineRenderer.SetPosition(i, path.corners[i]);
        }
    }
}