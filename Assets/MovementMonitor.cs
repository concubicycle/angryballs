using System.Collections;
using UnityEngine;

namespace Assets
{
    public class MovementMonitor: MonoBehaviour
    {
        #region Editor Variables
        // Interval for checking if block is falling
        public float MovementMonitorUpdateFreq = 0.25f;

        // A building block will be considered fallen if it has moved 
        // more than this amount
        public float isFallenDistanceTreshold = 0.1f;

        // A building block will be considered fallen if it has a 
        // difference in orientation greater than this
        public float isFallenOrientationDiff = 15.0f; 
        #endregion

        public bool IsInMotion { get; private set; }
        public bool IsUpright { get; private set; }


        private Vector3 _initialPosition;
        private float _initialOrientation;
        private Rigidbody2D _rigidBody2D;

        private float _displacementMagnitude;
        private float _orientationDiff;

        void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
        }
        
        void Start()
        {
            IsInMotion = false;
            IsUpright = true;

            _initialPosition = _rigidBody2D.transform.position;
            _initialOrientation = _rigidBody2D.rotation;            
        }

        public void BeginMonitoring()
        {
            StartCoroutine(MonitorMovement());
        }
           
        IEnumerator MonitorMovement()
        {
            WaitForSeconds wait = new WaitForSeconds(MovementMonitorUpdateFreq);

            while (true)
            {
                // the block is in motion if velocity or angular velocity is non-0
                IsInMotion = _rigidBody2D.angularVelocity == 0 && _rigidBody2D.velocity.magnitude == 0;

                if (!IsInMotion) yield return wait;

                _displacementMagnitude = (transform.position - _initialPosition).magnitude;
                _orientationDiff = Mathf.Abs(_rigidBody2D.rotation - _initialOrientation);

                if (_displacementMagnitude > isFallenDistanceTreshold && _orientationDiff > isFallenOrientationDiff)
                    IsUpright = false;
                else
                    IsUpright = true;

                yield return wait;
            }            
        }
    }
}
