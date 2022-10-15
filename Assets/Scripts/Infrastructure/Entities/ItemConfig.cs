using SQLite.Attributes;

namespace Infrastructure.Entity
{
    [UnityEngine.Scripting.Preserve]
    public class ItemConfig
    {
        [PrimaryKey] 
        [AutoIncrement]
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string SpritePath { get; set; }
        public bool IsStackable { get; set; }
        public bool IsCanUse { get; set; }
    }

}
