using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StreetRush.Car;
using StreetRush.Race;

namespace StreetRush.UI
{
    public class RaceHUD : MonoBehaviour
    {
        [SerializeField] private ArcadeCarController playerCar;
        [SerializeField] private RaceProgress playerProgress;
        [SerializeField] private TMP_Text positionText;
        [SerializeField] private TMP_Text lapText;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text speedText;
        [SerializeField] private Slider nitroSlider;

        private void Update()
        {
            if (playerCar == null) return;

            if (positionText != null && RaceManager.Instance != null)
                positionText.text = $"{RaceManager.Instance.GetPosition(playerProgress)}/4";

            if (lapText != null)
                lapText.text = $"LAP {playerProgress.CurrentLap}/3";

            if (timeText != null && RaceManager.Instance != null)
                timeText.text = RaceManager.Instance.RaceTime.ToString("00:00.00");

            if (speedText != null)
                speedText.text = $"{Mathf.RoundToInt(playerCar.SpeedKph)}\nKM/H";

            if (nitroSlider != null)
                nitroSlider.value = playerCar.Nitro01;
        }
    }
}
