using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Logic
{
    public class Poller : MonoBehaviour
    {
        public GameObject PooledObject;
        public int AmountToPool;
        private List<GameObject> _pooledObjects;
        

        void Start()
        {
            _pooledObjects = new List<GameObject>();
            GameObject tmp;
            for(int i = 0; i < AmountToPool; i++)
            {
                tmp = Instantiate(PooledObject, transform);
                tmp.SetActive(false);
                _pooledObjects.Add(tmp);
            }
        }
        
        public GameObject GetPooledObject()
        {
            for(int i = 0; i < AmountToPool; i++)
            {
                if(!_pooledObjects[i].activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            }
            return null;
        }
    }
}