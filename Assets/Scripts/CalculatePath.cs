
using UnityEngine;
using UnityEngine.AI;

public class CalculatePath : MonoBehaviour
{
    public Transform target;
    private NavMeshPath path;
    private float elapsed = 0.0f;
    void Start()
    {
        Debug.Log("START");
        path = new NavMeshPath();
        elapsed = 0.0f;
        GameObject go = new GameObject("Target");
        Vector3 sourcePostion = new Vector3(-0.05f, -0.449f, -0.04f);//The position you want to place your agent
        NavMeshHit closestHit;
        if (NavMesh.SamplePosition(sourcePostion, out closestHit, 500, 1))
        {
            go.transform.position = closestHit.position;
            go.AddComponent<NavMeshAgent>();
            //TODO
        }
        else
        {
            Debug.Log("...");
        }
    }

    void Update()
    {
        
        // Update the way to the goal every second.
        elapsed += Time.deltaTime;
        if (elapsed > 1.0f)
        {
            elapsed -= 1.0f;
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        }
        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
    }
}