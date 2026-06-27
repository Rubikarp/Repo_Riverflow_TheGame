using System.Threading;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ImageDrawer : MonoBehaviour, IVisualDrawer<Sprite>
{
    [Header("References")]
    [SerializeField] private Image _image;
 
    [Header("Settings")]
    [HorizontalGroup("Show Settings")]
    [SerializeField, Range(0.1f, 2f)] private float _showDuration = 0.3f;
    [SerializeField] private Ease _showEase = Ease.InOutSine;
    [HorizontalGroup("Hide Settings")]
    [SerializeField, Range(0.1f, 2f)] private float _hideDuration = 0.3f;
    [SerializeField] private Ease _hideEase = Ease.InOutSine;
    
    private Tween _activeTween;
    
    private void OnDestroy()
    {
        _activeTween?.Kill();
    }

    public void ShowImmediate(Sprite sprite)
    {
        _activeTween?.Kill();
        _image.SetAlpha(1f);
        
        _image.sprite = sprite;
    }
    public async Awaitable ShowAsync(Sprite sprite, CancellationToken ct = default)
    {
        _activeTween?.Kill();
 
        _image.sprite = sprite;
        _activeTween = _image.DOFade(1f, _showDuration).SetEase(_showEase);
 
        await _activeTween.WaitAsAwaitable(ct);
    }
 
    public void HideImmediate()
    {
        _activeTween?.Kill();
        _image.SetAlpha(0f);
    }
    public async Awaitable HideAsync(CancellationToken ct = default)
    {
        _activeTween?.Kill();
        _activeTween = _image.DOFade(0f, _hideDuration).SetEase(_showEase);
 
        await _activeTween.WaitAsAwaitable(ct);
    }
}