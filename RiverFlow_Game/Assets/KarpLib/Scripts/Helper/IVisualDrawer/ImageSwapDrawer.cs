using System.Threading;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwapDrawer : MonoBehaviour, IVisualDrawer<Sprite>
{
    [Header("References")]
    [SerializeField] private Image _imageA;
    [SerializeField] private Image _imageB;
 
    [Header("Settings")]
    [HorizontalGroup("Show Settings")]
    [SerializeField, Range(0.1f, 2f)] private float _showDuration = 0.3f;
    [SerializeField] private Ease _showEase = Ease.InOutSine;
    [HorizontalGroup("Hide Settings")]
    [SerializeField, Range(0.1f, 2f)] private float _hideDuration = 0.3f;
    [SerializeField] private Ease _hideEase = Ease.InOutSine;
    
    [Header("Internal")]
    private Image _current;
    private Image _next;
    private Tween _activeTween;
    
    private void Awake()
    {
        _current = _imageA;
        _next    = _imageB;
 
        _current.SetAlpha(1f);
        _next.SetAlpha(0f);
 
        _next.transform.SetAsFirstSibling();
        _current.transform.SetAsLastSibling();
    }
    private void OnDestroy()
    {
        _activeTween?.Kill();
    }

    public void ShowImmediate(Sprite sprite)
    {
        _activeTween?.Kill(complete: true);
 
        _current.sprite = sprite;
        _current.SetAlpha(1f);
        _next.SetAlpha(0f);
    }
    public async Awaitable ShowAsync(Sprite sprite, CancellationToken ct = default)
    {
        _activeTween?.Kill(complete: true);
 
        _next.SetAlpha(0f);
        _next.sprite = sprite;
        _next.transform.SetAsLastSibling();
 
        var seq = DOTween.Sequence()
            .Join(_next.DOFade(1f, _showDuration).SetEase(_showEase))
            .Join(_current.DOFade(0f, _hideDuration).SetEase(_hideEase))
            .OnComplete(() => 
            {
                (_current, _next) = (_next, _current);
            });
 
        _activeTween = seq;
        await seq.WaitAsAwaitable(ct);
    }
 
    public void HideImmediate()
    {
        _activeTween?.Kill();
        _current.SetAlpha(0f);
        _next.SetAlpha(0f);    
    }
    public async Awaitable HideAsync(CancellationToken ct = default)
    {
        _activeTween?.Kill();
 
        var seq = DOTween.Sequence()
            .Join(_current.DOFade(0f, _hideDuration).SetEase(_hideEase))
            .Join(_next.DOFade(0f, _hideDuration).SetEase(_hideEase));
 
        _activeTween = seq;
        await seq.WaitAsAwaitable(ct);
    }
}