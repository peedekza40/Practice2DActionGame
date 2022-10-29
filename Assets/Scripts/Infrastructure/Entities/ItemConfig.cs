using SQLite.Attributes;

namespace Infrastructure.Entities
{
    [UnityEngine.Scripting.Preserve]
    public class ItemConfig
    {
        [PrimaryKey] 
        [AutoIncrement]
        public int Id { get; set; }
        [NotNull] 
        public string Name { get; set; }
        public string SpritePath { get; set; }
        public bool IsStackable { get; set; }
        public bool IsCanUse { get; set; }
        public bool IsWeapon { get; set; }
    }

}
