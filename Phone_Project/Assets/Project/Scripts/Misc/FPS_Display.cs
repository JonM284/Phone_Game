using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Project.Scripts.Misc
{
    public class FPS_Display : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI FpsText = null;

        int frameCount;
        float prevTime;

        private void Start()
        {
            frameCount = 0;
            prevTime = 0.0f;
        }

        void Update()
        {
            ++frameCount;
            float time = Time.realtimeSinceStartup - prevTime;

            if (time >= 0.5f)
            {
                float fps = frameCount / time;
                FpsText.text = fps.ToString();

                frameCount = 0;
                prevTime = Time.realtimeSinceStartup;
            }
        }
    }
}

