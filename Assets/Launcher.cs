using System.Collections;
using UnityEngine;
using Assets.Scripts;
using GameStateManagement;
using Assets.GameStateManagement;

namespace Assets
{
    public class Launcher : MonoBehaviour
    {
        #region Editor Variables
        public float maxPullDistance = 2.0f;
        public GameObject ball;
        public GameObject ballPouch;
        public Transform center;
        public float pullAmount = 3.0f;
        public float springConstant = 500.0f;
        #endregion        

        private Rigidbody2D _ballRigidBody;
        private float _originalBallGravityScale;
        
        private bool IsBallGravityOn
        {
            set
            {
                _ballRigidBody.gravityScale = value ? _originalBallGravityScale : 0;
            }
        }

        #region Unity Methods
        void Awake()
        {
            _ballRigidBody = ball.GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            _originalBallGravityScale = _ballRigidBody.gravityScale;
            IsBallGravityOn = false;
            _ballRigidBody.transform.position = center.transform.position;

            ResetBall();
        }

        void Update()
        {
            if(GameStateManager.Instance.CurrentGameState.Is<PlayingGameState>() && Input.GetMouseButtonDown(0))
                StartCoroutine(MonitorDragging());
        }
        #endregion

        private void OnBallLaunched()
        {
            StartCoroutine(ApplyForce());
        }

        private void ResetBall()
        {
            ball.transform.position = center.position;
            ball.transform.MatchUpVector(Vector2.up);
            IsBallGravityOn = false;
            ballPouch.transform.position = ball.transform.position;
            ballPouch.transform.MatchUpVector(ball.transform.up);
            ballPouch.transform.parent = ball.transform;
        }

        #region Coroutines
        IEnumerator MonitorDragging()
        {
            ResetBall();

            while (Input.GetMouseButton(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 mouseDisplacement = center.position - mousePos;
                Vector2 ballUp = new Vector2(-mouseDisplacement.y, mouseDisplacement.x);

                //set ball position based on touch location
                float ballDispMagnitude = Mathf.Min(mouseDisplacement.magnitude / pullAmount, maxPullDistance);
                Vector2 ballPos = center.position + (-mouseDisplacement.normalized) * ballDispMagnitude;

                ball.transform.position = new Vector3(ballPos.x, ballPos.y, ball.transform.position.z);
                ball.transform.MatchUpVector(ballUp);


                //
                Debug.DrawLine(center.transform.position, ball.transform.position);

                yield return null;
            }

            OnBallLaunched();
        }
        
        /// <summary>
        /// This coroutine simulates a rubber band gradually applying force to the ball,
        /// using Hooke's law every frame to apply force. F = kX
        /// </summary>
        /// <returns></returns>
        IEnumerator ApplyForce()
        {
            // have to detect when to stop applying force - when the ball has gone past
            // the center. 
            Vector2 originalBallDisplacement = (center.position - ball.transform.position).normalized; //center to ball 
            Vector2 _ballDisplacement = originalBallDisplacement;

            WaitForSeconds dt = new WaitForSeconds(Time.fixedDeltaTime);

            while(Vector2.Dot(originalBallDisplacement, _ballDisplacement) > 0.1f)
            {
                _ballDisplacement = (center.position - ball.transform.position);

                // since last frame, we might have gone past center already
                if (Vector2.Dot(originalBallDisplacement, _ballDisplacement) < 0.1f)
                    break;

                float fMag = springConstant * _ballDisplacement.magnitude;

                _ballDisplacement.Normalize();

                _ballRigidBody.AddForce(originalBallDisplacement * fMag);

                Debug.DrawLine(center.transform.position, ball.transform.position);
                yield return dt;
            }

            IsBallGravityOn = true;
            ballPouch.transform.parent = ball.transform.parent;
        }

        IEnumerator KeepPouchAlignedWithBall()
        {
            while(Vector2.Distance(ballPouch.transform.position, center.position) < maxPullDistance)
            {
                ballPouch.transform.position = ball.transform.position;
                ballPouch.transform.MatchUpVector(ball.transform.up);
                yield return null;
            }
        }
        #endregion
    }
}

