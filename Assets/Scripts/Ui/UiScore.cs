using TMPro;
using UnityEngine;

namespace Ui
{
    public class UiScore : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _scoretext;

        private void Start()
        {
            UpdateScore(GameEvents.Points.Value);
            GameEvents.Points.OnValueChanged += UpdateScore;
        }

        private void UpdateScore(int value)
        {
            _scoretext.text = $"{value}";
        }
    }
}