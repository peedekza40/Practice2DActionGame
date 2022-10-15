using System.Data.Common;
using System.IO;
using SqlCipher4Unity3D;
using UnityEngine;

namespace Infrastructure.Entity
{
    public class DbContextBuilder
    {
        public SQLiteConnection Connection;

        private string PasswordDb = "Asdf+1234";
        private string ConnectionString = $"{Application.streamingAssetsPath}/Database/PlatformRPG.db";

        public DbContextBuilder()
        {
            Connection = new SQLiteConnection(ConnectionString, PasswordDb);
        }
    }
}

