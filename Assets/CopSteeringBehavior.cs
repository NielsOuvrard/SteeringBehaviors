using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CopSteeringBehavior : MonoBehaviour
{
    private SteeringBehaviour steeringBehaviour;
    private Vector3 center_circle;

    public GameObject target; // target could change according to the closest thief

    void Start()
    {
        steeringBehaviour = new SteeringBehaviour();
        steeringBehaviour.Init(this.GetComponent<UnityEngine.AI.NavMeshAgent>(), target, transform);
        center_circle = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        Debug.Log("Circle center: " + center_circle);
    }

    bool is_a_thief_near(int distance)
    {
        // Check if a thief is near
        // reset center_circle if true
        return false;
    }

    // The cops by default will be in "Wander" prowling the map, and when they
    // approach a thief, they will switch to "Seek" to catch the thief
    void Update()
    {
        if (is_a_thief_near(10)) {
            steeringBehaviour.Seek(target.transform.position);
        } else {
            steeringBehaviour.Wander();
        }
    }
}
