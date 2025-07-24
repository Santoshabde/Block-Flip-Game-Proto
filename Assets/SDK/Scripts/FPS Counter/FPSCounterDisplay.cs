using UnityEngine;

namespace SNGames.CommonModule
{
    public class FPSCounterDisplay : MonoBehaviour
    {
        // for ui.
        private int screenLongSide;
        private Rect boxRect;
        private GUIStyle style = new GUIStyle();

        // for fps calculation.
        private int frameCount;
        private float elapsedTime;
        private double frameRate;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            UpdateUISize();
        }

        private void Update()
        {
            // FPS calculation
            frameCount++;
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 0.5f)
            {
                frameRate = System.Math.Round(frameCount / elapsedTime, 1, System.MidpointRounding.AwayFromZero);
                frameCount = 0;
                elapsedTime = 0;

                // Update the UI size if the resolution has changed
                if (screenLongSide != Mathf.Max(Screen.width, Screen.height))
                {
                    UpdateUISize();
                }
            }
        }

        private void UpdateUISize()
        {
            screenLongSide = Mathf.Max(Screen.width, Screen.height);
            var rectLongSide = screenLongSide / 10;
            boxRect = new Rect(30, 200, rectLongSide, rectLongSide / 3);
            style.fontSize = (int)(screenLongSide / 36.8);
            style.normal.textColor = Color.white;
        }

        private void OnGUI()
        {
            GUI.Box(boxRect, "");
            GUI.Label(boxRect, " " + frameRate + "fps", style);
        }
    }
}