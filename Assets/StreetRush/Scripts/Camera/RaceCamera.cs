using UnityEngine;

namespace StreetRush.CameraSystem
{
    public class RaceCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 followOffset = new Vector3(0f, 2.2f, -6.5f);
        [SerializeField] private float positionSmooth = 8f;
        [SerializeField] private float rotationSmooth = 10f;
        [SerializeField] private float normalFov = 65f;
        [SerializeField] private float boostFov = 76f;
        [SerializeField] private float lookAhead = 7f;

        private Camera cam;

        private void Awake() => cam = GetComponent<Camera>();

        public void SetTarget(Transform newTarget) => target = newTarget;

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desiredPosition = target.TransformPoint(followOffset);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSmooth * Time.deltaTime);

            Vector3 lookPoint = target.position + target.forward * lookAhead + Vector3.up * 0.8f;
            Quaternion desiredRotation = Quaternion.LookRotation(lookPoint - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmooth * Time.deltaTime);

            float speed = target.GetComponent<Rigidbody>()?.linearVelocity.magnitude ?? 0f;
            float speed01 = Mathf.Clamp01(speed / 58f);
            cam.fieldOfView = Mathf.Lerp(normalFov, boostFov, speed01);
        }
    }
}
