using System.Data;

namespace Infrastructure.Extensions
{
    public static class IDataReaderExtension
    {
        public static string GetNullableString(this IDataReader reader, int index)
        {
            return reader.IsDBNull(index) ? null : reader.GetString(index);
        }
    }
}