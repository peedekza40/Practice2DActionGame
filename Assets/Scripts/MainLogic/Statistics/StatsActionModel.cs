using UnityEngine.Events;

public class StatsActionModel
{
    public UnityAction<StatsRow> IncreaseStats;
    public UnityAction<StatsRow> DecreaseStats;
    public UnityAction GetCurrentStatsValue;
    public UnityAction<StatsRow> UpStats;

    public StatsActionModel(
        UnityAction<StatsRow> increaseStats, 
        UnityAction<StatsRow> decreaseStats, 
        UnityAction getCurrentStatsValue,
        UnityAction<StatsRow> upStats)
    {
        IncreaseStats = increaseStats;
        DecreaseStats = decreaseStats;
        GetCurrentStatsValue = getCurrentStatsValue;
        UpStats = upStats;
    }
}
