using Assets.GameStateManagement;
using GameStateManagement;
using System.Collections;
using UnityEngine;

namespace Assets
{
    public class MainCameraController : MonoBehaviour
    {
        #region Editor Variables
        public float cameraWidthPadding = 4.0f;
        public Renderer ball;
        #endregion

        private Camera _thisCamera;
        private Level _currentLevel;        

        private void Awake()
        {
            _thisCamera = GetComponent<Camera>();
            _currentLevel = GameController.FindObjectOfType<Level>();
        }

        private void Start()
        {
            GameStateBase<PlayingGameState>.Instance.OnEnter += () =>
            {
                StartCoroutine(ZoomAndPan());
            };
        }

        private void FindLevel()
        {
            _currentLevel = GameObject.FindObjectOfType<Level>();
        }

        IEnumerator ZoomAndPan()
        {
            while (true)
            {
                Bounds levelBounds = _currentLevel.LevelBounds;
                Bounds ballBounds = ball.bounds;

                float minX = Mathf.Min(ballBounds.min.x, levelBounds.min.x);
                float maxX = Mathf.Max(ballBounds.max.x, levelBounds.max.x);

                float halfXRange = (maxX - minX) / 2.0f;
                float halfYrange = (levelBounds.max.y - levelBounds.min.y) / 2.0f;

                //we want the width of the camera to fit the level and ball
                // orthographicsSize is half the height of the camera. 
                _thisCamera.orthographicSize = (halfXRange / _thisCamera.aspect) + cameraWidthPadding * 2;

                float midX = minX + halfXRange;                
                float midY = levelBounds.min.y + halfYrange;

                _thisCamera.transform.position = new Vector3(midX, midY, _thisCamera.transform.position.z);

                yield return null;
            }
        }
    }
}
