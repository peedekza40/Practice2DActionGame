using SQLite.Attributes;

namespace Infrastructure.Entities
{
    [UnityEngine.Scripting.Preserve]
    public class EnemyConfig
    {
        [PrimaryKey] 
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string PrefabPath { get; set; }
    }

}