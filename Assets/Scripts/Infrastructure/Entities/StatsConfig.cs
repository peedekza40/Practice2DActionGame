using SQLite.Attributes;

namespace Infrastructure.Entity
{
    [UnityEngine.Scripting.Preserve]
    public class StatsConfig
    {
        [PrimaryKey] 
        [AutoIncrement]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MainIconPath { get; set; }
        public string SubIconPath { get; set; }
    }

}
