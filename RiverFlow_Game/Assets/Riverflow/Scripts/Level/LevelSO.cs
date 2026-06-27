using UnityEngine;

namespace RiverFlow
{
    /// <summary>
    /// Asset d'un niveau : enveloppe Unity autour des données pures <see cref="SLevelData"/>.
    /// La scène d'édition est peinte à la main puis bakée ici ; la partie consomme cet asset.
    /// </summary>
    [CreateAssetMenu(menuName = "RiverFlow/Level", fileName = "Level")]
    public sealed class LevelSO : ScriptableObject
    {
        [field: SerializeField] public SLevelData Level { get; private set; }

#if UNITY_EDITOR
        public void SetLevel(SLevelData level) => Level = level;
#endif
    }
}