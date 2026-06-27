using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RiverFlow
{
    /// <summary>
    /// À poser dans la SCÈNE D'ÉDITION de niveau. Le designer peint les Tilemaps, puis sauvegarde :
    /// « Save » écrase le LevelSO cible, « Save As New… » crée un nouvel asset via un panneau de fichier.
    /// Aucun effet à l'exécution : outil de conception uniquement.
    /// </summary>
    public sealed class LevelSaver : MonoBehaviour
    {
        [Title("Tilemaps")]
        [SerializeField, Required] private Tilemap _floorTilemap;
        [SerializeField, Required] private Tilemap _obstacleTilemap;
        [SerializeField, Tooltip("Optionnel : cases peintes = points de depart d'eau.")]
        private Tilemap _sourceTilemap;
 
        [Title("Config")]
        [SerializeField, Required] private BiomeCatalogSO _biomes;
        [SerializeField] private EHexOrientation _orientation = EHexOrientation.PointyTop;
        [SerializeField] private string _levelName = "Level";
        [SerializeField, Range(0, 100000)] private int _pointThreshold = 1000;
 
        [Title("Cible")]
        [SerializeField, Tooltip("Si renseigne, 'Save' ecrit dedans. Sinon utiliser 'Save As New'.")]
        private LevelSO _target;
        [SerializeField, FolderPath, Tooltip("Dossier propose par defaut pour 'Save As New'.")]
        private string _defaultFolder = "Assets/Levels";
 
        [Button("Save", ButtonSizes.Large), EnableIf("@_target != null")]
        public void Save()
        {
            if (_target == null)
            {
                SaveAsNew();
                return;
            }
 
            _target.SetLevel(ReadLevel());
            EditorUtility.SetDirty(_target);
            AssetDatabase.SaveAssets();
            EditorGUIUtility.PingObject(_target);
            Debug.Log($"Niveau sauvegarde -> {_target.name}", _target);
        }
 
        [Button("Save As New...", ButtonSizes.Large)]
        public void SaveAsNew()
        {
            string suggestedName = string.IsNullOrEmpty(_levelName) ? "Level" : _levelName;
            string path = EditorUtility.SaveFilePanelInProject(
                "Sauvegarder le niveau",
                suggestedName,
                "asset",
                "Choisir l'emplacement du LevelSO",
                _defaultFolder);
 
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
 
            var asset = ScriptableObject.CreateInstance<LevelSO>();
            asset.SetLevel(ReadLevel());
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
 
            _target = asset;
            EditorGUIUtility.PingObject(asset);
            Debug.Log($"Nouveau niveau cree -> {path}", asset);
        }
 
        private SLevelData ReadLevel()
        {
            return LevelBakeUtility.ReadLevel(
                _floorTilemap, _obstacleTilemap, _sourceTilemap,
                _biomes, _levelName, _pointThreshold);
        }
    }
}