using Core.Configs;
using SqlCipher4Unity3D;
using UnityEngine;

namespace Infrastructure.Entities
{
    public class DbContextBuilder
    {
        public SQLiteConnection Connection;

        public DbContextBuilder(IAppSettingsContext appSettingsContext)
        {
            Connection = new SQLiteConnection(Application.streamingAssetsPath + appSettingsContext.Config.Database.Path, appSettingsContext.Config.Database.Password);
        }
        
    }
}

