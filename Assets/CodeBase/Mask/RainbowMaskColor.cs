using CodeBase.Fish;

namespace CodeBase.Mask
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