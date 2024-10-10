using System.Collections;
using System.Collections.Generic;
using TriviaGame.Audio;
using UnityEngine;

namespace TriviaGame.CanvasManagement
{
    public class CanvasManager : MonoBehaviour
    {
        [Header("Canvas Base Classes")]
        [SerializeField] CanvasBase mainMenuCanvas;
        [SerializeField] CanvasBase leaderboardCanvas;
        [SerializeField] CanvasBase categoryCanvas;
        [SerializeField] CanvasBase questionCanvas;
        
        public static CanvasManager instance;

        private CanvasType _currentCanvasType = CanvasType.MainMenu;
        private Dictionary<CanvasType, CanvasBase> _typeToCanvas;

        private void Awake()
        {
            // Singleton
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            
            
            SetupCanvasDictionary();
            HideAllCanvassesOnStart();
        }

        public void SwitchCanvas(CanvasType to)
        {
            // Hide current canvas and then show target canvas
            StartCoroutine(SwitchCanvasWithAnimation(to));
        }
        
        public void SwitchCanvasSync(CanvasType to)
        {
            // Switch canvases synchronously
            StartCoroutine(SwitchCanvasWithAnimationSync(to));
        }

        private IEnumerator SwitchCanvasWithAnimation(CanvasType to)
        {
            AudioManager.instance.PlaySoundFx(SoundType.PageOut);
            
            // Hide current canvas and get hide animation duration
            float exitTime = _typeToCanvas[_currentCanvasType].HideCanvas();
            
            // wait for current canvas hide animation to complete
            yield return new WaitForSeconds(exitTime);

            // Show target canvas and get show animation duration
            float enterTime = _typeToCanvas[to].ShowCanvas();
            AudioManager.instance.PlaySoundFx(SoundType.PageIn);
            
            // Wait show animation duration an then complete the switch process
            yield return new WaitForSeconds(enterTime);


            _currentCanvasType = to;
        }

        private IEnumerator SwitchCanvasWithAnimationSync(CanvasType to)
        {
            AudioManager.instance.PlaySoundFx(SoundType.PageOut);

            // hide current canvas and show target canvas at the same time
            float exitTime = _typeToCanvas[_currentCanvasType].HideCanvas();
            float enterTime = _typeToCanvas[to].ShowCanvas();
            
            // wait for canvasses to do their animation and then complete switch process
            yield return new WaitForSeconds(Mathf.Max(exitTime, enterTime));

            AudioManager.instance.PlaySoundFx(SoundType.PageIn);
            _currentCanvasType = to;
        }

        private void SetupCanvasDictionary()
        {
            _typeToCanvas = new Dictionary<CanvasType, CanvasBase>()
            {
                { CanvasType.MainMenu, mainMenuCanvas },
                { CanvasType.Leaderboard, leaderboardCanvas },
                { CanvasType.Category, categoryCanvas },
                { CanvasType.Question, questionCanvas },
            };
        }

        private void HideAllCanvassesOnStart()
        {
            foreach (CanvasBase canvas in _typeToCanvas.Values)
            {
                canvas.HideOnStart();
            }
        }


        public enum CanvasType
        {
            MainMenu,
            Leaderboard,
            Category,
            Question
        }
    }

}