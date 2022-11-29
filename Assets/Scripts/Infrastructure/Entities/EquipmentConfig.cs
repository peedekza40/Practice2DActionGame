using SQLite.Attributes;

namespace Infrastructure.Entities
{
    [UnityEngine.Scripting.Preserve]
    public class EquipmentConfig
    {
        [PrimaryKey] 
        [AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int TypeId { get; set; }

        [NotNull]
        public int ItemId { get; set; }

        public string HaveWeaponSpritePath { get; set; }

        public string NoWeaponSpritePath { get; set; }

        public float MaxDefense { get; set; }

        public float MaxDamage { get; set; }
    }

}