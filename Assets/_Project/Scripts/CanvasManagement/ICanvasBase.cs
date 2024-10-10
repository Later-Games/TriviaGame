using UnityEngine;

namespace TriviaGame.CanvasManagement
{
    public abstract class CanvasBase : MonoBehaviour
    {
        public abstract float ShowCanvas(); // Return enter animation duration
        public abstract float HideCanvas(); // Return exit animation duration
        public abstract void HideOnStart(); // Use to hide all canvasses on start if not needed open and forget to close
    }
}