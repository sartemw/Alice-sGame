using CodeBase.Fish;
using UnityEngine;

namespace CodeBase.Services.Repainting
{
    public class Repaintable: MonoBehaviour
    {
        public ColorType ColorType;

        public void Painting(Material material)
        {
            gameObject.GetComponent<Renderer>().material = material;
        }
    }
}