using System.Threading;
using DG.Tweening;
using UnityEngine;

namespace WorldGame.SceneManagement
{
    public class FadeTransitionEffect : MonoBehaviour, ITransitionEffect
    {
        [Header("Config")]
        [SerializeField, Range(0.1f, 2f)] private float _fadeDuration = 0.4f;
        [SerializeField] private CanvasGroup _canvasGroup;
 
        public ETransitionEffect EffectType => ETransitionEffect.Fade;
 
 
        private void Awake()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }
 
 
        /// <summary>Fade in — couvre l'écran (alpha 0 → 1).</summary>
        public async Awaitable PlayInAsync(CancellationToken ct)
        {
            _canvasGroup.blocksRaycasts = true;
            await AwaitTween(
                _canvasGroup.DOFade(1f, _fadeDuration).SetEase(Ease.InQuad),
                ct);
        }
 
        /// <summary>Fade out — dévoile la nouvelle scène (alpha 1 → 0).</summary>
        public async Awaitable PlayOutAsync(CancellationToken ct)
        {
            await AwaitTween(
                _canvasGroup.DOFade(0f, _fadeDuration).SetEase(Ease.OutQuad),
                ct);
            _canvasGroup.blocksRaycasts = false;
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