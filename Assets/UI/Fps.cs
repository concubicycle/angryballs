using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class Fps : MonoBehaviour
    {
        public Text text; 
        private float _prevT;

        private int _frames;


        public void Start()
        {
            StartCoroutine(UpdateFps());
        }

        private IEnumerator UpdateFps()
        {
            WaitForSeconds wait = new WaitForSeconds(1.0f);

            while (true)
            {
                float dt = Time.realtimeSinceStartup - _prevT;                
                float frameRate = Mathf.RoundToInt(_frames / dt);
                text.text = frameRate + " fps";

                if (0 <= frameRate && frameRate <= 30.0f)
                    text.color = Color.red;
                else if (30.0f <= frameRate && frameRate <= 45.0f)
                    text.color = Color.yellow;
                else
                    text.color = Color.green;
                
                _frames = 0;
                _prevT = Time.realtimeSinceStartup;

                yield return wait;
            }
        }
        

        public void Update()
        {
            _frames++;            
        }
    }
}

