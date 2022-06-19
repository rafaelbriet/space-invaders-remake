using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SpaceInvadersRemake
{
    public class HUD : MonoBehaviour
    {
        [SerializeField]
        private GameManager gameManager;
        [SerializeField]
        private Player player;
        [SerializeField]
        private TextMeshProUGUI playerLivesText;
        [SerializeField]
        private TextMeshProUGUI playerPointsText;

        private void OnEnable()
        {
            gameManager.PlayerScored += OnPlayerScored;
            player.PlayerDamaged += OnPlayerDamaged;
        }

        private void Start()
        {
            SetPlayerPointsText();
            SetPlayerLivesText();
        }

        private void OnDisable()
        {
            gameManager.PlayerScored += OnPlayerScored;
            player.PlayerDamaged -= OnPlayerDamaged;
        }

        private void OnPlayerScored(object sender, System.EventArgs e)
        {
            SetPlayerPointsText();
        }

        private void SetPlayerPointsText()
        {
            playerPointsText.text = $"Score: {gameManager.CurrentPlayerPoints}";
        }

        private void OnPlayerDamaged(object sender, System.EventArgs e)
        {
            SetPlayerLivesText();
        }

        private void SetPlayerLivesText()
        {
            playerLivesText.text = player.CurrentLives.ToString();
        }
    }
}
