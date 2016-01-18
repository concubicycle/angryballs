using System;
using System.Collections;
using UnityEngine;

namespace Assets
{
    public class Launcher : MonoBehaviour
    {
        #region Editor Variables
        public float maxPullDistance = 2.0f;
        public GameObject ball;
        public Transform center;
        public float pullAmount = 3.0f;
        public float springConstant = 1000.0f;
        #endregion        

        private Rigidbody2D _ballRigidBody;
        private float _originalBallGravityScale;
        
        
        void Awake()
        {
            _ballRigidBody = ball.GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            _originalBallGravityScale = _ballRigidBody.gravityScale;
            _ballRigidBody.gravityScale = 0;
            _ballRigidBody.transform.position = center.transform.position;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                StartCoroutine(MonitorDragging());
        }

        IEnumerator MonitorDragging()
        {
            while (Input.GetMouseButton(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 mouseDisplacement = center.position - mousePos;

                //set ball position based on touch location
                float ballDispMagnitude = Mathf.Min(mouseDisplacement.magnitude / pullAmount, maxPullDistance);
                Vector2 ballPos = center.position + (-mouseDisplacement.normalized) * ballDispMagnitude;

                ball.transform.position = new Vector3(ballPos.x, ballPos.y, ball.transform.position.z);                

                yield return null;
            }

            OnBallLaunched();
        }

        private void OnBallLaunched()
        {
            _ballRigidBody.gravityScale = _originalBallGravityScale;
            StartCoroutine(ApplyForce());
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

            WaitForSeconds dt = new WaitForSeconds(0.01f);

            while(Vector2.Dot(originalBallDisplacement, _ballDisplacement) > 0)
            {
                _ballDisplacement = (center.position - ball.transform.position);
                float fMag = springConstant * _ballDisplacement.magnitude;

                _ballDisplacement.Normalize();

                _ballRigidBody.AddForce(_ballDisplacement * fMag);


                yield return dt;
            }
            
        }
    }
}

