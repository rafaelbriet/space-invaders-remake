using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceInvadersRemake
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Player player;

        private void OnEnable()
        {
            player.PlayerDied += OnPlayerDied;
        }

        private void OnDisable()
        {
            player.PlayerDied -= OnPlayerDied;
        }

        private void OnPlayerDied(object sender, System.EventArgs e)
        {
            SceneManager.LoadScene(ScenesName.GameOver);
        }
    }
}
