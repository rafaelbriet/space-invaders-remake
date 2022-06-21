using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceInvadersRemake
{
    public class MainMenu : MonoBehaviour
    {
        public void StartNewGame()
        {
            SceneManager.LoadScene(ScenesName.Game);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            Debug.Log("Quitting the game!");
#else
            Application.Quit();
#endif
        }
    }
}
