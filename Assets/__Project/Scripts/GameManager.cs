using System;
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
        [SerializeField]
        private Ground ground;

        public int CurrentPlayerPoints { get; set; }

        public event EventHandler PlayerScored;

        private void OnEnable()
        {
            player.PlayerDied += OnPlayerDied;
            invasionCommander.InvaderKilled += OnInvaderKilled;
            ground.InvadersReachedGround += OnInvadersReachedGround;
        }

        private void OnDisable()
        {
            player.PlayerDied -= OnPlayerDied;
            invasionCommander.InvaderKilled -= OnInvaderKilled;
            ground.InvadersReachedGround -= OnInvadersReachedGround;
        }

        private void OnPlayerDied(object sender, System.EventArgs e)
        {
            GameOver();
        }

        private void OnInvaderKilled(object sender, InvaderKilledEventArgs e)
        {
            CurrentPlayerPoints += e.Invader.PointsWhenKilled;
            PlayerScored?.Invoke(this, EventArgs.Empty);
        }

        private void OnInvadersReachedGround(object sender, EventArgs e)
        {
            GameOver();
        }

        private void GameOver()
        {
            SceneManager.LoadScene(ScenesName.GameOver);
        }
    }
}
