using Constants;
using UnityEngine;

namespace Character
{
    public class PlayerStatus : CharacterStatus, IDataPersistence
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

        public override void TakeDamage(float damage, GameObject attackerHitBox)
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
            
            base.TakeDamage(damage, attackerHitBox);
        }

        public override void Die()
        {
            GetComponent<PlayerCombat>().enabled = false;
            GetComponent<PlayerController>().enabled = false;
            base.Die();
        }

        public void LoadData(GameData data)
        {
            this.CurrentHP = data.PlayerHP;
            HealthBar.SetHealth(CurrentHP);
        }

        public void SaveData(ref GameData data)
        {
            data.PlayerHP = this.CurrentHP;
        }
    }
}
