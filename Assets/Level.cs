using AngryBalls.Assets;
using System;
using System.Collections;
using UnityEngine;

namespace Assets
{
    [RequireComponent(typeof(BlocksMonitor))]
    public class Level : MonoBehaviour
    {
        #region Editor Variables
        public MovementMonitor ball;
        #endregion

        private BlocksMonitor _blocksMonitor;        

        void Awake()
        {
            _blocksMonitor = GetComponent<BlocksMonitor>();            
        }

        public void OnPlayerShoot()
        {

        }

        IEnumerator WaitForLevelResult()
        {
            // wait while blocks and ball are stable
            while (!_blocksMonitor.IsStable && !ball.IsInMotion)
                yield return null;            
        }
    }
}
