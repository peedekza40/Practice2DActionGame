using SQLite.Attributes;

namespace Infrastructure.Entity
{
    [UnityEngine.Scripting.Preserve]
    public class EnemyConfig
    {
        [PrimaryKey] 
        [AutoIncrement]
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string PrefabPath { get; set; }
    }

}