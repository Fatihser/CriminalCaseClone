using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

public class bots : MonoBehaviour
{
    public static List<Transform> botList = new List<Transform>();
    private NavMeshAgent agent;
    public int index;
    private float distanceFromTarget;
    public float stopingDistance = 3.0f;
    public bool chaseTarget = true;
    public Transform target;
    Vector3 velocity = Vector3.zero;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = botList.ElementAt(index - 1);
        agent.updatePosition = false;
    }

    private void FixedUpdate()
    {
        distanceFromTarget = Vector3.Distance(target.position, transform.position);
        if (distanceFromTarget >= stopingDistance)
        {
            chaseTarget = true;
        }
        else
        {
            chaseTarget = false;
        }
        if (chaseTarget)
        {
            agent.SetDestination(target.position);

            //jittering porblemi icin eklendi.
            transform.position = Vector3.SmoothDamp(transform.position, agent.nextPosition, ref velocity, 0.1f);

            //myAnimator.SetBool("yuru", true);
        }
        else
        {
            //myAnimator.SetBool("yuru", false);
        }
    }


}
