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
        float lookAhead = targetDir.magnitude / (agent.speed + target.GetComponent<NavMeshAgent>().speed);
        Vector3 pursueLocation = target.transform.position + target.transform.forward * lookAhead * 3;

        if (target.GetComponent<NavMeshAgent>().speed <= 0.01f) {
            Seek(target.transform.position);
        } else {
            Seek(pursueLocation);
        }
    }

    public void Evade()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        float lookAhead = targetDir.magnitude / (agent.speed + target.GetComponent<NavMeshAgent>().speed);
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
    
    GameObject[] getHidingPlaces()
    {
        return GameObject.FindGameObjectsWithTag("hide");
    }

    public void Hide()
    {
        float closestDistance = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        GameObject[] hidingPlaces = getHidingPlaces();

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
