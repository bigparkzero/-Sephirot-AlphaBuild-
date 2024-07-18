using UnityEngine;

public class PatrolNode : INode
{
    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private Transform characterTransform;
    private float speed;

    public PatrolNode(Transform characterTransform, Transform[] waypoints, float speed)
    {
        this.characterTransform = characterTransform;
        this.waypoints = waypoints;
        this.speed = speed;
    }

    public NodeStatus Execute()
    {
        if (waypoints.Length == 0)
        {
            return NodeStatus.Failure;
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        characterTransform.position = Vector3.MoveTowards(characterTransform.position, targetWaypoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(characterTransform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        return NodeStatus.Running;
    }
}
