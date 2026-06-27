using UnityEngine;
using System.Threading;

namespace WorldGame.SceneManagement
{
    public interface ISceneCommand
    {
        /// <summary>
        /// False pour les transitions marquées non-réversibles (ex. fin de partie, splash).
        /// </summary>
        bool CanUndo { get; }
 
        Awaitable ExecuteAsync(CancellationToken ct);
 
        /// <summary>
        /// Rejoue la requête inverse sans la pousser dans l'historique.
        /// </summary>
        Awaitable UndoAsync(CancellationToken ct);
    }
}