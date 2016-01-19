using UnityEngine;
using UnityEngine.UI;
using GameStateManagement;
using Assets.GameStateManagement;

namespace Assets.UI
{
    public delegate void OnGameStart();

    public class StartMenuController : MonoBehaviour
    {
        public Button start;

        private void Start()
        {
            start.onClick.AddListener(() =>
            {
                GameStateManager.Instance.TransitionToGameState<PlayingGameState>();
                gameObject.SetActive(false);
            });
        }

        private void OnEnable()
        {
            GameStateManager.Instance.TransitionToGameState<MainMenuGameState>();
        }
    }
}
