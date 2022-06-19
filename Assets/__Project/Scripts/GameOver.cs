using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceInvadersRemake
{
    public class GameOver : MonoBehaviour
    {
        public void StartNewGame()
        {
            SceneManager.LoadScene(ScenesName.Game);
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene(ScenesName.MainMenu);
        }
    }
}
