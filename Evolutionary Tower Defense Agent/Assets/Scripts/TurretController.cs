using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulator;
using BehaviorTree;

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

    Turret turret;
    public TurretAgent turretAgent;
    public IState state;
    // Start is called before the first frame update
    void Start()
    {
        turret = new Turret(HitPoints, Range, FieldOfViewAngle, RotationSpeed, Damage);
        //turretAgent = new TurretAgent();

        Vector3 mapCenter = new Vector3(35, head.position.y, 35);
        Vector3 dir = mapCenter - head.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void UpdateTarget()
    {

        if (target != null && Vector3.Distance(head.position, target.position) <= Range)
        {
            //Debug.Log("KeepTarget");
            return; //Keep old target
        }

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

    public void LookTowardsTarget(IAgent target)
    {
        var targetGridPosition = state.PositionOf(target);
        Debug.Log("targetGridPosition: " + targetGridPosition);
        Vector3 targetWorldPosition = new Vector3(targetGridPosition.x * 5, 3, targetGridPosition.y * 5);
        Debug.Log("targetWorldPosition: " + targetWorldPosition);
        Vector3 dir = targetWorldPosition - head.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        /*state.GetTargetOf(turretAgent).Apply(target =>
        {
            var targetGridPosition = state.PositionOf(target);
            Debug.Log("targetGridPosition: " + targetGridPosition);
            Vector3 targetWorldPosition = new Vector3(targetGridPosition.x * 5, 3, targetGridPosition.y * 5);
            Debug.Log("targetWorldPosition: " + targetWorldPosition);
            Vector3 dir = targetWorldPosition - head.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = lookRotation.eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        });*/

    }
    // Update is called once per frame
    void Update()
    {
        /*Vector3 dir = head.forward;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);*/

        /*UpdateTarget();

        if (target == null)
        {
            //TurretScanningForTarget();
            return;
        }

        Vector3 dir = target.position - head.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);*/
    }

    void TurretScanningForTarget()
    {
        partToRotate.RotateAround(partToRotate.position, transform.up, Time.deltaTime * RotationSpeed);
    }

    public void DoScanForTargetRotation()
    {
        partToRotate.RotateAround(partToRotate.position, transform.up, RotationSpeed);
    }

    public void UndoScanForTargetRotation()
    {
        partToRotate.RotateAround(partToRotate.position, transform.up, -RotationSpeed);
    }

    public void DealDamageToTarget()
    {
        if(target != null)
        {
            var targetedEnemy = target.GetComponent<EnemyController>().enemy;
            if (targetedEnemy.GetHitPoints() > turret.Damage)
            {
                targetedEnemy.GetDamaged(turret.Damage);
                Debug.Log("Deal Damage to Enemy: " + target + " " + targetedEnemy.GetHitPoints());
            }
            else
            {
                Debug.Log("Destroy enemy: " + target);
                target.gameObject.SetActive(false);
                //Destroy(target.gameObject);
                //TODO: remove the destroyed enemy's Agent from the enemyAgent list?
            }
        }
    }

    //Heal Target on Undo action Un-dealing the damage
    public void HealTarget()
    {
        if (target != null)
        {
            var targetedEnemy = target.GetComponent<EnemyController>().enemy;
            targetedEnemy.GetHealed(turret.Damage);

            Debug.Log("Heal Enemy: " + target + " " + targetedEnemy.GetHitPoints());
            //Here we should check if it heals over its maxhealth then cap the HP at maxhealth
            //Also it is a bit tricky to handle if an enemy just got destroyed then we want to Undo the action.
        }
    }
    bool IsTargetInFOV(GameObject target)
    {
        Vector3 directionToObject = (target.transform.position - head.position);
        //Debug.Log("directionToObject: " + directionToObject);

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
