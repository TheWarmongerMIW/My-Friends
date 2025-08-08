using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FriendMovement : MonoBehaviour
{
    [Header("Genral Components")]
    [SerializeField] private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();   
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(Vector3 des)
    {
        agent.SetDestination(des); 
    }
    public float CalculateSpeed()
    {
        return agent.velocity.magnitude / agent.speed;
    }
}
