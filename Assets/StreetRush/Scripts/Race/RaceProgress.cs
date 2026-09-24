using UnityEngine;

namespace StreetRush.Race
{
    public class RaceProgress : MonoBehaviour
    {
        [SerializeField] private int totalCheckpoints = 12;
        [SerializeField] private int totalLaps = 3;

        public int CurrentLap { get; private set; } = 1;
        public int NextCheckpoint { get; private set; }
        public float ProgressScore => ((CurrentLap - 1) * totalCheckpoints) + NextCheckpoint;

        public bool Finished { get; private set; }

        public void TryPassCheckpoint(int checkpointIndex)
        {
            if (Finished || checkpointIndex != NextCheckpoint) return;

            NextCheckpoint++;

            if (NextCheckpoint >= totalCheckpoints)
            {
                NextCheckpoint = 0;

                if (CurrentLap >= totalLaps)
                {
                    Finished = true;
                    RaceManager.Instance?.CarFinished(this);
                }
                else
                {
                    CurrentLap++;
                }
            }
        }
    }
}
