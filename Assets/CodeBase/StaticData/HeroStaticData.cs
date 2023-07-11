using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "HeroData", menuName = "Static Data/Hero")]
    public class HeroStaticData : ScriptableObject
    {
        public HeroTypeId HeroTypeId;
        
        [Range(1,100)]
        public int Hp = 50;
    
        [Range(1,30)]
        public int Damage = 10;

        [Range(.5f,3)]
        public float EffectiveDistance = .5f;
    
        [Range(.5f,1)]
        public float Cleavage = .5f;

        [Range(0,100)]
        public int MoveSpeed = 50;
    }
}
