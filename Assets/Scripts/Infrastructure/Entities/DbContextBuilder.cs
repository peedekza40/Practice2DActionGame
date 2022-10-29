using Core.Configs;
using SqlCipher4Unity3D;
using UnityEngine;

namespace Infrastructure.Entities
{
    public class DbContextBuilder
    {
        public SQLiteConnection Connection;

        public DbContextBuilder(AppSettingsModel configure)
        {
            Connection = new SQLiteConnection(Application.streamingAssetsPath + configure.Database.Path, configure.Database.Password);
        }
    }
}

