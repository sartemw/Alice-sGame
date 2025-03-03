using _Project.CodeBase.Fish;

namespace _Project.CodeBase.Mask
{
    public class RainbowMaskColor : Colored
    {
        public void StartRainbowColor()
        {
            StartCoroutine(RainbowColor());
        }

        private void OnDisable()
        {
            StopCoroutine(RainbowColor());
        }
    }
}