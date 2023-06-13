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
        public int? MinGold { get; set; }
        public int? MaxGold { get; set; }
    }

}