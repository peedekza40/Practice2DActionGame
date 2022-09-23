using Constants;
using UnityEngine;

namespace Character
{
    public class PlayerStatus : CharacterStatus, IDataPersistence
    {
        [Header("Assets")]
        public Gold Gold;
        public Inventory Inventory;

        private IPlayerCombat PlayerCombat;
        private BlockFlashAnimatorController BlockFlashAnimatorController;

        void Awake()
        {
            PlayerCombat = GetComponent<IPlayerCombat>();
            BlockFlashAnimatorController = GameObject.Find(GameObjectName.EffectBlock).GetComponent<BlockFlashAnimatorController>();
            base.BaseAwake();
        }

        void Update()
        {
            base.BaseUpdate();
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            ItemWorld itemWorld = other.GetComponent<ItemWorld>();
            if(itemWorld != null)
            {
                Inventory.AddItem(itemWorld.GetItem());
                itemWorld.DestroySelf();
            }
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
            SetCurrentHP(data.PlayerHP);
        }

        public void SaveData(ref GameData data)
        {
            data.PlayerHP = this.CurrentHP;
        }
    }
}
