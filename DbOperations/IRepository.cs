using System.Reflection;
using MySqlConnector;

namespace BankingSystem.DbOperations;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}

public abstract class Repository
{
    private static T ReadSingle<T>(MySqlDataReader reader) where T : new()
    {
        T obj = new T();

        // Get the type of the object T
        Type type = typeof(T);
        
        // Loop through the reader's columns
        for (int i = 0; i < reader.FieldCount; i++)
        {
            // Get the column name and value from the reader
            string columnName = reader.GetName(i);
            // CHANGE columnName from snake_case to PascalCase
            columnName = string.Concat(columnName.Split('_').Select(s => char.ToUpper(s[0]) + s.Substring(1)));
            object columnValue = reader[i];

            // Find a property in the object with the same name as the column
            PropertyInfo? property = type.GetProperty(columnName,
                BindingFlags.IgnoreCase | BindingFlags.Public 
                                        | BindingFlags.Instance);

            if (property != null && property.CanWrite)
            {
                // Convert the column value to the property's type
                object convertedValue = Convert.ChangeType(columnValue, property.PropertyType);
            
                // Set the property value of the object
                property.SetValue(obj, convertedValue);
            }
        }

        return obj;
    }
    
    public static async Task<IEnumerable<T>> Read<T>(MySqlDataReader reader) where T : new()
    {
        var list = new List<T>();

        using (reader)
        {
            while (await reader.ReadAsync())
            {
                list.Add(ReadSingle<T>(reader));
            }
        }

        return list;
    }
}
