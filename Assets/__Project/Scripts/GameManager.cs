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
        private GameObject bunkerPrefab;
        [SerializeField]
        private Transform[] bunkerBuildingSites;
        [SerializeField]
        private Ground ground;
        [SerializeField]
        private GameObject invasionEndedCanvas;

        private List<GameObject> bunkers = new List<GameObject>();

        public int CurrentPlayerPoints { get; set; }

        public event EventHandler PlayerScored;

        private void OnEnable()
        {
            player.PlayerDied += OnPlayerDied;
            invasionCommander.InvaderKilled += OnInvaderKilled;
            invasionCommander.InvasionEnded += OnInvasionEnded;
            ground.InvadersReachedGround += OnInvadersReachedGround;
        }

        private void Start()
        {
            StartGame();
        }

        private void OnDisable()
        {
            player.PlayerDied -= OnPlayerDied;
            invasionCommander.InvaderKilled -= OnInvaderKilled;
            invasionCommander.InvasionEnded -= OnInvasionEnded;
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

        private void OnInvasionEnded(object sender, EventArgs e)
        {
            player.enabled = false;
            invasionEndedCanvas.SetActive(true);
        }

        private void OnInvadersReachedGround(object sender, EventArgs e)
        {
            GameOver();
        }

        private void GameOver()
        {
            SceneManager.LoadScene(ScenesName.GameOver);
        }

        public void StartGame()
        {
            player.enabled = true;
            invasionCommander.CreateInvasion();
            invasionEndedCanvas.SetActive(false);

            DemolishBunkers();
            BuildBunkers();
        }

        private void DemolishBunkers()
        {
            bunkers.RemoveAll(bunker => bunker == null);

            foreach (GameObject bunker in bunkers)
            {
                DestroyImmediate(bunker);
            }

            bunkers.Clear();
        }

        private void BuildBunkers()
        {
            foreach (Transform buildingSite in bunkerBuildingSites)
            {
                GameObject bunker = Instantiate(bunkerPrefab, buildingSite);
                bunkers.Add(bunker);
            }
        }
    }
}
