using System.Threading;
using UnityEngine;

namespace WorldGame.SceneManagement
{
    public interface ISceneTransitionService
    {
        bool CanGoBack { get; }
 
        Awaitable TransitionToAsync(STransitionRequest request, CancellationToken ct);
 
        Awaitable GoBackAsync(CancellationToken ct);
    }
}