using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberSteeringBehavior : MonoBehaviour
{
    private SteeringBehaviour steeringBehaviour;

    public GameObject target; // target could change according to the closest cop

    public enum Behavior
    {
        Wander,
        Flee,
        Evade,
        Hide,
        Seek,
        Pursue,
        None
    }

    public Behavior currentBehavior;

    void Start()
    {
        steeringBehaviour = new SteeringBehaviour();
        steeringBehaviour.Init(this.GetComponent<UnityEngine.AI.NavMeshAgent>(), target, transform);
    }

    bool is_a_pedestrian_near(int distance)
    {
        // Check if a pedestrian is near
        return false;
    }

    // private void OnTriggerEnter (Collider collision)
    // {
    //     if (collision.tag == "Player") {
    //         Destroy(collision.gameObject);
    //         Destroy(this.gameObject);
    //         Debug.Log("Hit: " + collision.transform.name);
    //     }
    // }

    // They will have the default "Evade" behavior towards police officers and the player
    // until a pedestrian enters their range, when that happens they will switch to "Seek" to catch
    // the pedestrian
    void Update()
    {
        switch (currentBehavior)
        {
            case Behavior.Wander:
                steeringBehaviour.Wander();
                break;
            case Behavior.Flee:
                steeringBehaviour.Flee(target.transform.position);
                break;
            case Behavior.Evade:
                steeringBehaviour.Evade();
                break;
            case Behavior.Hide:
                steeringBehaviour.Hide();
                break;
            case Behavior.Seek:
                steeringBehaviour.Seek(target.transform.position);
                break;
            case Behavior.Pursue:
                steeringBehaviour.Pursue();
                break;
            case Behavior.None:
                break;
        }
    }
}
