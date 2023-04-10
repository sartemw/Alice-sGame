using CodeBase.Fish;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;

namespace CodeBase.StaticData
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