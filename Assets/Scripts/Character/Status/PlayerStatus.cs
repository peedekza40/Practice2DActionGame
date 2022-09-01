using Constants;
using UnityEngine;

namespace Character
{
    public class PlayerStatus : CharacterStatus
    {
        private IPlayerCombat PlayerCombat;
        private BlockFlashAnimatorController BlockFlashAnimatorController;

        void Start()
        {
            PlayerCombat = GetComponent<IPlayerCombat>();
            BlockFlashAnimatorController = GameObject.Find(GameObjectName.EffectBlock).GetComponent<BlockFlashAnimatorController>();
            base.BaseStart();
        }

        void Update()
        {
            base.BaseUpdate();
        }

        public override void TakeDamage(float damage)
        {
            var reduceDamage = 0f;
            if(PlayerCombat.IsBlocking)
            {
                if(PlayerCombat.PressBlockThisFrame)
                {
                    reduceDamage = damage;
                    BlockFlashAnimatorController.TriggerParry();
                }
                else
                {
                    reduceDamage = PlayerCombat.GetReduceDamage();
                }

                damage -= reduceDamage;
            }
            
            base.TakeDamage(damage);
        }
    }
}