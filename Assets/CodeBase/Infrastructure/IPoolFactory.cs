using System.Threading.Tasks;
using CodeBase.Logic.PooledObjects;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public interface IPoolFactory
    {
        Task<GameObject> Create(PoolObjectsTypeId pooler, Transform parent);
    }
}