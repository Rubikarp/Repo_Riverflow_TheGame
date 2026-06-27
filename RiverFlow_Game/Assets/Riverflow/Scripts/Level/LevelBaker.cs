using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RiverFlow
{
    /// <summary>
    /// Variante minimale : bake les Tilemaps peints dans un LevelSO cible déjà existant.
    /// Pour la création de nouveaux niveaux dans la scène d'édition, préférer <see cref="LevelSaver"/>.
    /// </summary>
    public sealed class LevelBaker : MonoBehaviour
    {
        [SerializeField, Required] private Tilemap _floorTilemap;
        [SerializeField, Required] private Tilemap _obstacleTilemap;
        [SerializeField] private Tilemap _sourceTilemap;
        [SerializeField, Required] private BiomeCatalogSO _biomes;
        [SerializeField] private string _levelName = "Level";
        [SerializeField, Range(0, 100000)] private int _pointThreshold = 1000;
        [SerializeField, Required] private LevelSO _target;
 
        [Button("Bake -> LevelSO", ButtonSizes.Large)]
        public void Bake()
        {
            var level = LevelBakeUtility.ReadLevel(
                _floorTilemap, _obstacleTilemap, _sourceTilemap,
                _biomes, _levelName, _pointThreshold);
 
            _target.SetLevel(level);
            EditorUtility.SetDirty(_target);
            AssetDatabase.SaveAssets();
            Debug.Log($"Bake OK : {level.Cells.Length} cases, {level.Sources.Length} sources -> {_target.name}", _target);
        }
    }
}