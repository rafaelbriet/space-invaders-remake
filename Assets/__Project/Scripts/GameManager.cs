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
        [SerializeField]
        private InvasionCommander invasionCommander;

        private int currentPlayerPoints = 0;

        private void OnEnable()
        {
            player.PlayerDied += OnPlayerDied;
            invasionCommander.InvaderKilled += OnInvaderKilled;
        }

        private void OnDisable()
        {
            player.PlayerDied -= OnPlayerDied;
            invasionCommander.InvaderKilled -= OnInvaderKilled;
        }

        private void OnPlayerDied(object sender, System.EventArgs e)
        {
            SceneManager.LoadScene(ScenesName.GameOver);
        }

        private void OnInvaderKilled(object sender, InvaderKilledEventArgs e)
        {
            currentPlayerPoints += e.Invader.PointsWhenKilled;
        }
    }
}
