using System.Collections.Generic;
using Infrastructure.Entity;

namespace Core.Repositories
{
    public interface IStatsConfigRepository
    {
        List<StatsConfig> Get();
    }
}