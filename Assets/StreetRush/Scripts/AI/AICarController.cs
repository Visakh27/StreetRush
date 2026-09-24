using UnityEngine;

namespace StreetRush.AI
{
    public class AICarController : MonoBehaviour
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float acceleration = 32f;
        [SerializeField] private float steering = 6f;
        [SerializeField] private float waypointReachDistance = 8f;

        private Rigidbody rb;
        private int currentWaypoint;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = new Vector3(0f, -0.35f, 0f);
        }

        private void FixedUpdate()
        {
            if (waypoints == null || waypoints.Length == 0) return;

            Transform target = waypoints[currentWaypoint];
            Vector3 localTarget = transform.InverseTransformPoint(target.position);

            float steerInput = Mathf.Clamp(localTarget.x / Mathf.Max(localTarget.magnitude, 1f), -1f, 1f);
            rb.AddTorque(Vector3.up * steerInput * steering, ForceMode.Acceleration);
            rb.AddForce(transform.forward * acceleration, ForceMode.Acceleration);

            Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
            rb.AddForce(-transform.right * localVelocity.x * 6f, ForceMode.Acceleration);

            if (Vector3.Distance(transform.position, target.position) < waypointReachDistance)
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }
}
