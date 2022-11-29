using System.Collections;
using System.Collections.Generic;
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
        private List<SpriteRenderer> ChildSpriteRenders = new List<SpriteRenderer>();
        private Dictionary<string, Material> ChildOriginalMaterial = new Dictionary<string, Material>();
        private Coroutine FlashRoutine;
        private StateMachine CombatStateMachine;

        private void Awake() 
        {
            CombatStateMachine = GetComponents<StateMachine>().FirstOrDefault(x => x.Id == StateId.Combat);
            OriginalMaterial = SpriteRenderer.material;
            ChildSpriteRenders = SpriteRenderer.GetComponentsInChildren<SpriteRenderer>().ToList();
            foreach(var item in ChildSpriteRenders)
            {
                ChildOriginalMaterial.Add(item.name, item.material);
            }
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
            foreach(var item in ChildSpriteRenders)
            {
                item.material = FlashMaterial;
            }

            yield return new WaitForSeconds(Duration);
            
            SpriteRenderer.material = OriginalMaterial;
            foreach(var item in ChildSpriteRenders)
            {
                item.material = ChildOriginalMaterial[item.name];
            }
            FlashRoutine = null;
        }
    }

}
