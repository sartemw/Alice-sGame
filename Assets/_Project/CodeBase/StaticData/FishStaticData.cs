using _Project.CodeBase.Fish;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "FishData", menuName = "Static Data/Fish")]
    public class FishStaticData : ScriptableObject
    {
        public const string SpriteAtlasPath = "Fishs/Fishs";
        
        public FishBehaviourEnum FishBehaviour;
        [Range(0, 10)]
        public float MovementSpeed;

        public AssetReferenceGameObject PrefabReference;
    }
}