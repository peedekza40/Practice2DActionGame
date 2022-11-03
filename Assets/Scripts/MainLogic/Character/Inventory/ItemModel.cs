using System;
using Core.Constants;
using Core.Repositories;
using Infrastructure.Entities;
using UnityEngine;
using Zenject;

namespace Character.Inventory
{
    [Serializable]
    public class ItemModel
    {
        public Guid InstanceId;
        public ItemType Type;
        public int Amount;
        public bool IsStackable { get; private set; }
        public bool IsCanUse { get; private set; }
        public bool IsWeapon { get; private set; }
        public Sprite Sprite { get; private set; }

        #region Dependencies
        private ItemAssets itemAssets;
        private IItemConfigRepository itemConfigRepository;
        #endregion

        [Inject]
        public void Init(
            ItemAssets itemAssets,
            IItemConfigRepository itemConfigRepository)
        {
            this.itemAssets = itemAssets;
            this.itemConfigRepository = itemConfigRepository;
        }

        public void Setup(ItemType type, int amount = 1)
        {
            InstanceId = Guid.NewGuid();
            Type = type;
            Amount = amount;

            ItemConfig itemConfig = itemConfigRepository.GetByType(Type);
            IsStackable = itemConfig.IsStackable;
            IsCanUse = itemConfig.IsCanUse;
            IsWeapon = itemConfig.IsWeapon;
            Sprite = GetSprite(itemConfig.SpritePath);
        }
        
        private Sprite GetSprite(string spritePath)
        {
            Sprite sprite = Resources.Load<Sprite>(spritePath);
            if(sprite == null)
            {
                sprite = itemAssets.DefaultSprite;
            }
            return sprite;
        }
    }

}
