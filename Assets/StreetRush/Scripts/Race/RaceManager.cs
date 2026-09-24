using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StreetRush.Race
{
    public class RaceManager : MonoBehaviour
    {
        public static RaceManager Instance { get; private set; }

        [SerializeField] private List<RaceProgress> racers = new();
        [SerializeField] private float countdown = 3f;

        public bool RaceStarted { get; private set; }
        public float RaceTime { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            Invoke(nameof(StartRace), countdown);
        }

        private void Update()
        {
            if (RaceStarted) RaceTime += Time.deltaTime;
        }

        private void StartRace()
        {
            RaceStarted = true;
            Debug.Log("STREET RUSH: GO!");
        }

        public int GetPosition(RaceProgress racer)
        {
            return racers
                .OrderByDescending(r => r.Finished ? float.MaxValue : r.ProgressScore)
                .ThenBy(r => r.Finished ? 0 : 1)
                .ToList()
                .IndexOf(racer) + 1;
        }

        public void CarFinished(RaceProgress racer)
        {
            Debug.Log($"Finished: {racer.name}");
        }
    }
}
