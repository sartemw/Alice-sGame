using System.Threading.Tasks;
using _Project.CodeBase.Infrastructure.AssetManagement;
using _Project.CodeBase.Services.StaticData;
using _Project.CodeBase.StaticData;
using UnityEngine;
using Zenject;

namespace _Project.CodeBase.Infrastructure
{
    public class PoolFactory : IPoolFactory
    {
        private IAssetProvider _assetProvider;
        private IStaticDataService _staticData;

        [Inject]
        public void Construct(IAssetProvider assetProvider, IStaticDataService staticData)
        {
            _assetProvider = assetProvider;
            _staticData = staticData;
        }
        
        public async Task<GameObject>Create(PoolObjectsTypeId poolObjectId, Transform parent)
        {
            PoolObjectStaticData poolObjectData = _staticData.ForPoolObjects(poolObjectId);
            
            GameObject prefab = await _assetProvider.Load<GameObject>(poolObjectData.PrefabReference);
            GameObject poolObject = InstantiateRegistered(prefab, parent);

            return poolObject;
        }
        
        private GameObject InstantiateRegistered(GameObject prefab, Transform parent)
        {
            GameObject gameObject = Object.Instantiate(prefab, parent);
            //RegisterProgressWatchers(gameObject);

            return gameObject;
        }
    }
}