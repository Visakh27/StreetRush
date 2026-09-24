using UnityEngine;

namespace StreetRush.Input
{
    public class MobileInput : MonoBehaviour
    {
        public static MobileInput Instance { get; private set; }

        public float Steering { get; private set; }
        public bool BrakeHeld { get; private set; }
        public bool NitroHeld { get; private set; }

        [SerializeField] private bool useTilt = false;
        [SerializeField] private float tiltSensitivity = 1.6f;

        private bool leftHeld;
        private bool rightHeld;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Update()
        {
            float keyboard = UnityEngine.Input.GetAxisRaw("Horizontal");
            float buttons = (rightHeld ? 1f : 0f) - (leftHeld ? 1f : 0f);

            float target = Mathf.Clamp(keyboard + buttons, -1f, 1f);

            if (useTilt && Mathf.Abs(keyboard) < 0.01f && !leftHeld && !rightHeld)
                target = Mathf.Clamp(Input.acceleration.x * tiltSensitivity, -1f, 1f);

            Steering = Mathf.MoveTowards(Steering, target, Time.deltaTime * 8f);
            BrakeHeld = UnityEngine.Input.GetKey(KeyCode.Space) || BrakeHeld;
            NitroHeld = UnityEngine.Input.GetKey(KeyCode.LeftShift) || NitroHeld;
        }

        public void PressLeft() => leftHeld = true;
        public void ReleaseLeft() => leftHeld = false;
        public void PressRight() => rightHeld = true;
        public void ReleaseRight() => rightHeld = false;

        public void PressBrake() => BrakeHeld = true;
        public void ReleaseBrake() => BrakeHeld = false;

        public void PressNitro() => NitroHeld = true;
        public void ReleaseNitro() => NitroHeld = false;
    }
}
