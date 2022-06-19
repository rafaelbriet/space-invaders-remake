using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SpaceInvadersRemake
{
    public class HUD : MonoBehaviour
    {
        [SerializeField]
        private Player player;
        [SerializeField]
        private TextMeshProUGUI playerLivesText;

        private void OnEnable()
        {
            player.PlayerDamaged += OnPlayerDamaged;
        }

        private void Start()
        {
            SetPlayerLivesText();
        }

        private void OnDisable()
        {
            player.PlayerDamaged -= OnPlayerDamaged;
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
