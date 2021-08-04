using UnityEngine;

public class IAPlayer : MonoBehaviour
{
    public Waypoints Waypoints;
    public float speed;
    private Transform target;
    private int waypointIndex = 1;

    private void Update()
    {
        target = Waypoints.points[waypointIndex % Waypoints.points.Count];
        
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * (speed * Time.deltaTime), Space.World);
        
        if (Vector3.Distance(transform.position, target.position) <= 0.2) 
            waypointIndex++;

        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, angle, 0);
    }
}