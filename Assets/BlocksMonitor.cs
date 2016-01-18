using UnityEngine;
using System.Collections;
using Assets;
using System.Collections.Generic;

namespace AngryBalls.Assets
{
    /// <summary>
    /// Monitors the state of all the blocks in a level. 
    /// </summary>
    public class BlocksMonitor : MonoBehaviour
    {
        #region Editor Variables
        // the blocks are concidered table if it's been this many seconds since any
        // of them moved.
        public float StableNoMovementTime = 1.0f;
        #endregion


        public bool IsStable { get; private set; }
        public bool HaveBlocksMoved { get;  private set; }

        public int NumberKnockedDown { get; private set; }

        private IEnumerable<MovementMonitor> SceneBuildingBlocks { get; set; }

        // Use this for initialization
        void Start()
        {
            SceneBuildingBlocks = Object.FindObjectsOfType<MovementMonitor>();
            HaveBlocksMoved = false;

            StartCoroutine(BeginMonitoring());
        }      

        IEnumerator BeginMonitoring()
        {                     
            int numInMotion = 0;
            HashSet<MovementMonitor> knockedDown = new HashSet<MovementMonitor>();


            // vars for seeing how long it's been since any movement
            // keep track of this manually - check the time delta between
            // each frame and the previous frame, using realTimeSinceStartup.
            float timeSinceMovement = 0;
            float lastTime = Time.realtimeSinceStartup;


            foreach (var block in SceneBuildingBlocks)
                block.BeginMonitoring();         

            while(true)
            {
                numInMotion = 0;                

                foreach(var block in SceneBuildingBlocks)
                {
                    if (block.IsInMotion)
                    {
                        numInMotion++;
                        HaveBlocksMoved = true;                        
                    }

                    if(!block.IsUpright)
                    {
                        knockedDown.Add(block);
                        NumberKnockedDown = knockedDown.Count;
                    }
                }

                // keep track of how long it's been since any blocks moved.
                if(numInMotion == 0)
                {
                    float dt = Time.realtimeSinceStartup - lastTime;
                    timeSinceMovement += dt;
                }
                else                
                    timeSinceMovement = 0;                
                
                if (timeSinceMovement > StableNoMovementTime) IsStable = true;
                else IsStable = false;

                lastTime = Time.realtimeSinceStartup;

                yield return null; // this waits 1 frame.
            }
        }        
    }
}