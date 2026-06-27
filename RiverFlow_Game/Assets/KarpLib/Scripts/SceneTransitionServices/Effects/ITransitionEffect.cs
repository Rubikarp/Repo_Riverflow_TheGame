using System.Threading;
using UnityEngine;

namespace WorldGame.SceneManagement
{
    public interface ITransitionEffect
    {
        ETransitionEffect EffectType { get; }
 
        Awaitable PlayInAsync(CancellationToken ct);
 
        Awaitable PlayOutAsync(CancellationToken ct);
    }
}