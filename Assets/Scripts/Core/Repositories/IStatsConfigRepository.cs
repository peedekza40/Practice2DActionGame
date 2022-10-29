using System.Collections.Generic;
using Infrastructure.Entities;

namespace Core.Repositories
{
    public interface IStatsConfigRepository
    {
        List<StatsConfig> Get();
    }
}