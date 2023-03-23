using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "PoolObjectData", menuName = "Static Data/PoolObject")]
    public class PoolObjectStaticData : ScriptableObject
    {
        public PoolObjectsTypeId PoolObjectTypeId;
        public AssetReferenceGameObject PrefabReference;
    }
}