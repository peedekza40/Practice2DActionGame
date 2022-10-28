using System.Collections;
using UnityEngine;

namespace Character.Behaviors
{
    public class FlashEffect : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;
        public Material FlashMaterial;
        public Color Color;
        public float Duration;

        private Material OriginalMaterial;
        private Coroutine FlashRoutine;

        private void Awake() 
        {
            OriginalMaterial = SpriteRenderer.material;
        }

        public void Flash()
        {
            if(FlashRoutine != null)
            {
                StopCoroutine(FlashRoutine);
            }
            FlashRoutine = StartCoroutine(WaitFlash());
        }

        private IEnumerator WaitFlash()
        {
            FlashMaterial.color = Color;
            SpriteRenderer.material = FlashMaterial;
            yield return new WaitForSeconds(Duration);
            SpriteRenderer.material = OriginalMaterial;
            FlashRoutine = null;
        }
    }

}
