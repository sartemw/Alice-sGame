using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure.Factory;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.PooledObjects
{
    public class Pooler : MonoBehaviour
    {
        public PoolObjectsTypeId PooledObject;
        public int AmountToPool;
        public List<GameObject> _pooledObjects;
        private IGameFactory _factory;

        public void Construct(IGameFactory gameFactory) => 
            _factory = gameFactory;
        
        void Start() => 
            InstantiateRequiredObjects();

        public GameObject GetPooledObject()
        {
            if (_pooledObjects.Count == 0)
                return null;
            
            for(int i = 0; i < AmountToPool; i++)
            {
                if(!_pooledObjects[i].activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            }
            return null;
        }

        private async Task InstantiateRequiredObjects()
        {
            _pooledObjects = new List<GameObject>();
            GameObject tmp;
            for (int i = 0; i < AmountToPool; i++)
            {
                tmp = await _factory.CreatePoolObjects(PooledObject, transform);
                if (tmp == null)
                    return;
                
                tmp.SetActive(false);
                _pooledObjects.Add(tmp);
            }
        }
    }
}