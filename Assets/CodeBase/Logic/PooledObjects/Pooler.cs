using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure;
using CodeBase.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.PooledObjects
{
    public class Pooler : MonoBehaviour
    {
        public PoolObjectsTypeId PoolObject;
        public int AmountToPool;
        public List<GameObject> PoolObjectsList;
        private IPoolFactory _factory;

        [Inject]
        public void Construct(IPoolFactory poolFactory)
        {
            _factory = poolFactory;
        }

        async Task Start() => 
            await InstantiateRequiredObjects();

        public GameObject GetPooledObject()
        {
            if (PoolObjectsList.Count == 0)
                return null;
            
            for(int i = 0; i < AmountToPool; i++)
            {
                if(!PoolObjectsList[i].activeInHierarchy)
                {
                    return PoolObjectsList[i];
                }
            }
            return null;
        }

        private async Task InstantiateRequiredObjects()
        {
            PoolObjectsList = new List<GameObject>();
            GameObject tmp;
            for (int i = 0; i < AmountToPool; i++)
            {
                tmp = await _factory.Create(PoolObject, transform);
                if (tmp == null)
                    return;
                
                tmp.SetActive(false);
                PoolObjectsList.Add(tmp);
            }
        }
    }
}