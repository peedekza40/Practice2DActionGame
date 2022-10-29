using SQLite.Attributes;

namespace Infrastructure.Entities
{
    [UnityEngine.Scripting.Preserve]
    public class WeaponConfig
    {
        [PrimaryKey] 
        [AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int ItemId { get; set; }
        public string SpritePath { get; set; }
        public int MaxDamage { get; set; }
    }

}