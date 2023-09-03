using SQLite.Attributes;

namespace Infrastructure.Entities
{
    [UnityEngine.Scripting.Preserve]
    public class StatsConfig
    {
        [PrimaryKey] 
        [AutoIncrement]
        public int Id { get; set; }
        
        [NotNull]
        [Unique]
        public string Code { get; set; }
        public string Name { get; set; }
        public float IncreaseValue { get; set; }
        public string MainIconPath { get; set; }
        public string SubIconPath { get; set; }
    }

}
