using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;
using Core.Constants;

namespace Character.Animators
{
    public class BlockFlashAnimatorController : MonoBehaviour
    {
        private Animator Animator;
        private Transform EffectBlockPoint;

        // Start is called before the first frame update
        void Start()
        {
            Animator = GetComponent<Animator>();
            EffectBlockPoint = GameObject.Find("EffectBlockPoint").transform;
        }

        void destroyEvent()
        {
        }

        public void TriggerParryEffect()
        {
            transform.position = EffectBlockPoint.position;
            Animator.SetTrigger(AnimationParameter.Parry);
        }
    }
}

