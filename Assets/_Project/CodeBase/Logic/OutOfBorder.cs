using _Project.CodeBase.Hero;
using UnityEngine;

namespace _Project.CodeBase.Logic
{
    public class OutOfBorder : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player")) 
                col.GetComponent<HeroHealth>().OutOfBorders();
        }
    }
}