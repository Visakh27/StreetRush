using UnityEngine;

namespace StreetRush.Race
{
    public class RaceCheckpoint : MonoBehaviour
    {
        [SerializeField] private int index;
        public int Index => index;

        private void OnTriggerEnter(Collider other)
        {
            RaceProgress progress = other.GetComponentInParent<RaceProgress>();
            if (progress != null)
                progress.TryPassCheckpoint(index);
        }
    }
}
