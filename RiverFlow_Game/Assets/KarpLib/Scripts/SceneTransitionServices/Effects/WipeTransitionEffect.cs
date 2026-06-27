using System.Threading;
using DG.Tweening;
using UnityEngine;

namespace WorldGame.SceneManagement
{
    public class WipeTransitionEffect : MonoBehaviour, ITransitionEffect
    {
        [Header("Config")]
        [SerializeField, Range(0.1f, 2f)] private float _wipeDuration = 0.35f;
        [SerializeField] private RectTransform _wipeRect;

        public ETransitionEffect EffectType => ETransitionEffect.Wipe;

        private Vector2 _hiddenPosition;
        private Vector2 _visiblePosition;


        private void Awake()
        {
            // Suppose un panneau plein écran décalé hors cadre vers le haut au repos.
            float screenHeight = _wipeRect.rect.height;
            _hiddenPosition = new Vector2(0f, screenHeight);
            _visiblePosition = Vector2.zero;

            _wipeRect.anchoredPosition = _hiddenPosition;
        }


        /// <summary>Wipe in — fait glisser le panneau sur l'écran.</summary>
        public async Awaitable PlayInAsync(CancellationToken ct)
        {
            await AwaitTween(
                _wipeRect.DOAnchorPos(_visiblePosition, _wipeDuration).SetEase(Ease.InCubic),
                ct);
        }

        /// <summary>Wipe out — fait glisser le panneau hors de l'écran.</summary>
        public async Awaitable PlayOutAsync(CancellationToken ct)
        {
            await AwaitTween(
                _wipeRect.DOAnchorPos(_hiddenPosition, _wipeDuration).SetEase(Ease.OutCubic),
                ct);
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