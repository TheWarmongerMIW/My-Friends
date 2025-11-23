using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class FriendMovement : MonoBehaviour
{
    [Header("Genral Components")]
    public NavMeshAgent agent;
    public float jumpDuration;
    public bool onNavMeshLink = false;

    [Header("Animation Event")]
    public UnityEvent OnLand, OnStartJump;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetDestination(Vector3 destination)
    {
        if (onNavMeshLink) return;
        agent.destination = destination;
    }
    public void Move(Vector3 des)
    {
        agent.SetDestination(des); 
    }
    public float CalculateSpeed()
    {
        return agent.velocity.magnitude / agent.speed;
    }
    public void MoveToFurn()
    {
        if (agent.isOnOffMeshLink && !onNavMeshLink) StartNavMeshLinkMovement();
        if (onNavMeshLink) FaceTarget(agent.currentOffMeshLinkData.endPos);
    }
    private void StartNavMeshLinkMovement()
    {
        onNavMeshLink = true;
        NavMeshLink link = (NavMeshLink)agent.navMeshOwner;
        Spline spline = link.GetComponentInChildren<Spline>();

        PerformJump(link, spline);
    }
    private void PerformJump(NavMeshLink link, Spline spline)
    {
        bool reverseDirection = CheckIfJumpingFromEndToStart(link);
        StartCoroutine(MoveOnOffMeshLink(spline, reverseDirection));

        OnStartJump?.Invoke();
    }
    private bool CheckIfJumpingFromEndToStart(NavMeshLink link)
    {
        Vector3 startPosWorld
            = link.gameObject.transform.TransformPoint(link.startPoint);
        Vector3 endPosWorld
            = link.gameObject.transform.TransformPoint(link.endPoint);

        float distancePlayerToStart
            = Vector3.Distance(agent.transform.position, startPosWorld);
        float distancePlayerToEnd
            = Vector3.Distance(agent.transform.position, endPosWorld);

        return distancePlayerToStart > distancePlayerToEnd;
    }
    private IEnumerator MoveOnOffMeshLink(Spline spline, bool reverseDirection)
    {
        float currentTime = 0;
        Vector3 agentStartPosition = agent.transform.position;

        while (currentTime < jumpDuration)
        {
            currentTime += Time.deltaTime;

            float amount = Mathf.Clamp01(currentTime / jumpDuration);
            amount = reverseDirection ? 1 - amount : amount;

            agent.transform.position =
                reverseDirection ?
                spline.CalculatePositionCustomEnd(amount, agentStartPosition)
                : spline.CalculatePositionCustomStart(amount, agentStartPosition);

            yield return new WaitForEndOfFrame();
        }

        agent.CompleteOffMeshLink();

        OnLand?.Invoke();
        yield return new WaitForSeconds(0.1f);
        onNavMeshLink = false;
    }
    void FaceTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation
            = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation
            = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

}
