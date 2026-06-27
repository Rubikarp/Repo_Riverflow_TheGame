using System.Threading;
using UnityEngine;

namespace WorldGame.SceneManagement
{
    /// <summary>
    /// Contrat bas niveau utilisé par GoToSceneCommand pour déléguer l'exécution physique
    /// de la transition au service, sans créer de dépendance circulaire.
    /// </summary>
    public interface ITransitionExecutor
    {
        Awaitable ExecuteTransitionAsync(STransitionRequest request, CancellationToken ct);
    }
}