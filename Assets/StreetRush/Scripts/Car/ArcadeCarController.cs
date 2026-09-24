using UnityEngine;
using StreetRush.Input;

namespace StreetRush.Car
{
    [RequireComponent(typeof(Rigidbody))]
    public class ArcadeCarController : MonoBehaviour
    {
        [Header("Performance")]
        [SerializeField] private float maxSpeedKph = 210f;
        [SerializeField] private float acceleration = 38f;
        [SerializeField] private float brakeForce = 55f;
        [SerializeField] private float reverseAcceleration = 16f;

        [Header("Handling")]
        [SerializeField] private float steeringTorque = 7f;
        [SerializeField] private float lateralGrip = 8f;
        [SerializeField] private float driftGrip = 3.2f;
        [SerializeField] private float downforce = 35f;

        [Header("Nitro")]
        [SerializeField] private float nitroAcceleration = 75f;
        [SerializeField] private float nitroCapacity = 100f;
        [SerializeField] private float nitroDrainPerSecond = 32f;
        [SerializeField] private float nitroRechargePerSecond = 7f;

        private Rigidbody rb;
        private float nitro;
        private bool nitroActive;

        public float SpeedKph => rb.linearVelocity.magnitude * 3.6f;
        public float Nitro01 => nitroCapacity <= 0f ? 0f : nitro / nitroCapacity;
        public bool IsDrifting { get; private set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.centerOfMass = new Vector3(0f, -0.35f, 0f);
            nitro = nitroCapacity;
        }

        private void FixedUpdate()
        {
            float steering = MobileInput.Instance != null ? MobileInput.Instance.Steering : UnityEngine.Input.GetAxisRaw("Horizontal");
            bool brake = MobileInput.Instance != null && MobileInput.Instance.BrakeHeld;
            bool nitroInput = MobileInput.Instance != null ? MobileInput.Instance.NitroHeld : UnityEngine.Input.GetKey(KeyCode.LeftShift);

            float speed01 = Mathf.Clamp01(SpeedKph / maxSpeedKph);
            bool drifting = brake && SpeedKph > 45f && Mathf.Abs(steering) > 0.25f;
            IsDrifting = drifting;

            ApplyDrive(brake, speed01);
            ApplySteering(steering, speed01);
            ApplyLateralGrip(drifting);
            ApplyDownforce();

            nitroActive = nitroInput && nitro > 0.5f && SpeedKph > 25f;

            if (nitroActive)
            {
                rb.AddForce(transform.forward * nitroAcceleration, ForceMode.Acceleration);
                nitro = Mathf.Max(0f, nitro - nitroDrainPerSecond * Time.fixedDeltaTime);
            }
            else
            {
                nitro = Mathf.Min(nitroCapacity, nitro + nitroRechargePerSecond * Time.fixedDeltaTime);
            }

            LimitSpeed();
        }

        private void ApplyDrive(bool brake, float speed01)
        {
            if (brake)
            {
                rb.AddForce(-transform.forward * brakeForce, ForceMode.Acceleration);
                return;
            }

            if (speed01 < 0.98f)
                rb.AddForce(transform.forward * acceleration * (1f - speed01 * 0.45f), ForceMode.Acceleration);
            else
                rb.AddForce(-rb.linearVelocity.normalized * 4f, ForceMode.Acceleration);
        }

        private void ApplySteering(float steering, float speed01)
        {
            if (rb.linearVelocity.sqrMagnitude < 4f) return;

            float torque = steering * steeringTorque * Mathf.Lerp(0.35f, 1f, speed01);
            rb.AddTorque(Vector3.up * torque, ForceMode.Acceleration);
        }

        private void ApplyLateralGrip(bool drifting)
        {
            Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
            float grip = drifting ? driftGrip : lateralGrip;
            Vector3 lateralVelocity = transform.right * localVelocity.x;
            rb.AddForce(-lateralVelocity * grip, ForceMode.Acceleration);
        }

        private void ApplyDownforce()
        {
            rb.AddForce(-transform.up * downforce * Mathf.Clamp(rb.linearVelocity.magnitude, 0f, 60f), ForceMode.Acceleration);
        }

        private void LimitSpeed()
        {
            float maxMs = maxSpeedKph / 3.6f;
            if (rb.linearVelocity.magnitude > maxMs)
                rb.linearVelocity = rb.linearVelocity.normalized * maxMs;
        }
    }
}
