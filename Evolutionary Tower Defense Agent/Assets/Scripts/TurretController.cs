using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{

    [SerializeField] public float HitPoints /*{ get; set; }*/ = 100f;
    [SerializeField] float Range /*{ get; set; }*/ = 5f;
    [SerializeField] float FieldOfViewAngle/* { get; set; }*/ = 90f;
    [SerializeField] float RotationSpeed /*{ get; set; }*/ = 10f;
    [SerializeField] float Damage /*{ get; set; }*/ = 10f;

    public Transform partToRotate;
    public Transform head;
    public Transform target;

    public string enemyTag = "Enemy";
    // Start is called before the first frame update
    void Start()
    {
        Vector3 mapCenter = new Vector3(35, head.position.y, 35);
        Vector3 dir = mapCenter - head.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;


        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(head.position, enemy.transform.position);

            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        if(closestEnemy != null && shortestDistance <= Range/* && IsTargetInFOV(closestEnemy)*/)
        {
            target = closestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        /*Vector3 dir = head.forward;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);*/

        UpdateTarget();

        if (target == null)
        {
            TurretScanningForTarget();
            return;
        }

        Vector3 dir = target.position - head.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void TurretScanningForTarget()
    {
        partToRotate.RotateAround(partToRotate.position, transform.up, Time.deltaTime * RotationSpeed);
    }

    bool IsTargetInFOV(GameObject target)
    {
        Vector3 directionToObject = (target.transform.position - head.position);
        Debug.Log("directionToObject: " + directionToObject);

        float targetToTurretForwardAngle = Vector3.Angle(directionToObject, head.forward);
        bool isInsideAngle = targetToTurretForwardAngle <= FieldOfViewAngle / 2f;

        return isInsideAngle;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(head.position, Range);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(head.position, head.position + head.forward * Range);

        Vector3 FOVRightBound = Quaternion.Euler(0f, FieldOfViewAngle / 2f, 0f) * head.forward;
        Gizmos.DrawLine(head.position, head.position + FOVRightBound * Range);

        Vector3 FOVLeftBound = Quaternion.Euler(0f, - FieldOfViewAngle / 2f, 0f) * head.forward;
        Gizmos.DrawLine(head.position, head.position + FOVLeftBound * Range);

    }
}
