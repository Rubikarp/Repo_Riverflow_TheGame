using System.Threading;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WorldGame.SceneManagement
{
    public class LoadingScreenController : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TMP_Text _progressLabel;
 
        [Header("Config")]
        [SerializeField, Range(0.1f, 1f)] private float _fadeDuration = 0.25f;
 
 
        private void Awake()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
        }
 
 
        public async Awaitable Show(CancellationToken ct)
        {
            gameObject.SetActive(true);
            UpdateProgress(0f);
            _canvasGroup.blocksRaycasts = true;
            await AwaitTween(_canvasGroup.DOFade(1f, _fadeDuration), ct);
        }
 
        public async Awaitable Hide(CancellationToken ct)
        {
            await AwaitTween(_canvasGroup.DOFade(0f, _fadeDuration), ct);
            _canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
        }
 
        public void UpdateProgress(float progress)
        {
            _progressBar.value = progress;
            _progressLabel.text = $"{Mathf.RoundToInt(progress * 100f)} %";
        }
 
 
        private static async Awaitable AwaitTween(Tween tween, CancellationToken ct)
        {
            var src = new AwaitableCompletionSource<bool>();
            tween.OnComplete(() => src.TrySetResult(true));
            ct.Register(() =>
            {
                tween.Kill();
                src.TrySetCanceled();
            });
            await src.Awaitable;
        }
    }
}