using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SteeringBehaviour
{
    NavMeshAgent agent;
    GameObject target;
    Transform transform;

    public float wanderRadius = 10;
    public float wanderDistance = 10;
    public float wanderJitter = 10;
    Vector3 wanderTarget = Vector3.zero;

    float getSpeedTarget()
    {
        if (target.GetComponent<NavMeshAgent>()) {
            return target.GetComponent<NavMeshAgent>().speed;
        } else if (target.GetComponent<Drive>()) {
            return target.GetComponent<Drive>().speed;
        } else {
            return 0;
        }
    }

    public void Init(NavMeshAgent _agent, GameObject _target, Transform _transform)
    {
        agent = _agent;
        target = _target;
        transform = _transform;
    }

    public void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    public void Flee(Vector3 location)
    {
        agent.SetDestination(this.transform.position - (location - this.transform.position));
    }

    public void Pursue()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        float lookAhead = targetDir.magnitude / (agent.speed + getSpeedTarget());
        Vector3 pursueLocation = target.transform.position + target.transform.forward * lookAhead * 3;

        if (getSpeedTarget() <= 0.01f) {
            Seek(target.transform.position);
        } else {
            Seek(pursueLocation);
        }
    }

    public void Evade()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        float lookAhead = targetDir.magnitude / (agent.speed + getSpeedTarget());
        Vector3 pursueLocation = target.transform.position + target.transform.forward * lookAhead * 3;

        Flee(pursueLocation);
    }

    
    public void Wander()
    {
        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                    0,
                                    Random.Range(-1.0f, 1.0f) * wanderJitter);

        wanderTarget.Normalize();

        wanderTarget *= wanderRadius;
        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = agent.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }
    
    GameObject[] getObjectsWithTag(string tag)
    {
        return GameObject.FindGameObjectsWithTag(tag);
    }

    public void Hide()
    {
        float closestDistance = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        GameObject[] hidingPlaces = getObjectsWithTag("hide");

        foreach(GameObject hidingPlace in hidingPlaces) {
            Vector3 hideDirection = hidingPlace.transform.position - target.transform.position;
            Vector3 hidePosition = hidingPlace.transform.position + hideDirection.normalized * 3;

            if (closestDistance > Vector3.Distance(this.transform.position, hidePosition)) {
                closestDistance = Vector3.Distance(this.transform.position, hidePosition);
                chosenSpot = hidePosition;
            }
        }
        Seek(chosenSpot);
    }
}
