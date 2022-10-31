using Core.Configs;
using SqlCipher4Unity3D;
using UnityEngine;

namespace Infrastructure.Entities
{
    public class DbContextBuilder
    {
        public SQLiteConnection Connection;

        public DbContextBuilder(AppSettingsModel config)
        {
            Connection = new SQLiteConnection(Application.streamingAssetsPath + config.Database.Path, config.Database.Password);
        }
    }
}

