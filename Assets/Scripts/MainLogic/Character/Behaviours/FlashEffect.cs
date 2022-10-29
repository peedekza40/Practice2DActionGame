using System.Collections;
using System.Linq;
using Character.Combat.States.Player;
using Constants;
using UnityEngine;

namespace Character.Behaviours
{
    public class FlashEffect : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;
        public Material FlashMaterial;
        public Color Color;
        public float Duration;

        private Material OriginalMaterial;
        private Coroutine FlashRoutine;
        private StateMachine CombatStateMachine;

        private void Awake() 
        {
            CombatStateMachine = GetComponents<StateMachine>().FirstOrDefault(x => x.Id == StateId.Combat);
            OriginalMaterial = SpriteRenderer.material;
        }

        public void Flash()
        {
            if(FlashRoutine != null)
            {
                StopCoroutine(FlashRoutine);
            }
            
            if(CombatStateMachine.IsCurrentState(typeof(BlockParryState)) == false)
            {
                FlashRoutine = StartCoroutine(WaitFlash());
            }
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
