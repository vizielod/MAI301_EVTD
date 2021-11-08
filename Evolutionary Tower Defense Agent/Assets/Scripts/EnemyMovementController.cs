using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{

    public float speed = 10f;
    public float stepSize = 5f;

    private Transform previousTarget;
    private Transform nextTarget;
    private int waypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        previousTarget = WaypointController.wayPoints[0];
        nextTarget = WaypointController.wayPoints[1];
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if(Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }*/
    }

    void GetNextWaypoint()
    {
        Debug.Log("GetNextWaypoint");
        if(waypointIndex >= WaypointController.wayPoints.Length - 1)
        {
            Destroy(gameObject);
            Debug.Log("Enemy reached the END object!");
            return;
        }

        waypointIndex++;
        previousTarget = nextTarget; // We keep the old nextTarget as the new previous target
        nextTarget = WaypointController.wayPoints[waypointIndex];
        Debug.Log("GetNextWaypoint previous target: " + previousTarget);
        Debug.Log("GetNextWaypoint next target: " + nextTarget);
    }

    void GetPreviousWaypoint()
    {
        Debug.Log("GetPreviousWaypoint");
        if (waypointIndex == 0)
        {
            Debug.Log("Enemy is at START position!");
            return;
        }

        waypointIndex--;
        nextTarget = previousTarget; // We keep the old previousTarget as the new nexttarget
        previousTarget = WaypointController.wayPoints[waypointIndex];
        Debug.Log("GetNextWaypoint previous target: " + previousTarget);
        Debug.Log("GetNextWaypoint next target: " + nextTarget);
    }

    public void PerformStepForward()
    {
        Debug.Log("PerformStepForward next target: " + nextTarget);
        if (nextTarget.position.z > transform.position.z)
        {
            GoEast();
        }
        if (nextTarget.position.z < transform.position.z)
        {
            GoWest();
        }
        if (nextTarget.position.x > transform.position.x)
        {
            GoSouth();
        }
        if (nextTarget.position.x < transform.position.x)
        {
            GoNorth();
        }

        if(nextTarget.position.x == transform.position.x && nextTarget.position.z == transform.position.z)
        {
            GetNextWaypoint();
        }

    }
    public void PerformStepBackward()
    {
        Debug.Log("PerformStepForward previous target: " + previousTarget);
        if (previousTarget.position.z > transform.position.z)
        {
            GoEast();
        }
        if (previousTarget.position.z < transform.position.z)
        {
            GoWest();
        }
        if (previousTarget.position.x > transform.position.x)
        {
            GoSouth();
        }
        if (previousTarget.position.x < transform.position.x)
        {
            GoNorth();
        }

        if (previousTarget.position.x == transform.position.x && previousTarget.position.z == transform.position.z)
        {
            GetPreviousWaypoint();
        }
    }

    void GoEast()
    {
        Debug.Log("GoEast");
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + stepSize);
    }

    void GoWest()
    {
        Debug.Log("GoWest");
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - stepSize);
    }

    void GoSouth()
    {
        Debug.Log("GoSouth");
        transform.position = new Vector3(transform.position.x + stepSize, transform.position.y, transform.position.z);
    }

    void GoNorth()
    {
        Debug.Log("GoNorth");
        transform.position = new Vector3(transform.position.x - stepSize, transform.position.y, transform.position.z);
    }
}
