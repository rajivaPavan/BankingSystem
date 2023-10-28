using BankingSystem.DBContext;
using BankingSystem.Models;
using MySqlConnector;

namespace BankingSystem.DbOperations;

public List<Transaction> GetAccountTransactions(string accountNumber)
{
    List<Transaction> transactions = new List<Transaction>();

    
    string connectionString = "connectionString";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        string query = "SELECT * FROM Transactions WHERE AccountNumber = @AccountNumber";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@AccountNumber", accountNumber);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Transaction transaction = new Transaction
                    {
                        // Assuming the Transaction properties match the column names in the Transactions table
                        Id = (int)reader["Id"],
                        AccountNumber = reader["AccountNumber"].ToString(),
                        // Populate other properties as needed
                    };

                    transactions.Add(transaction);
                }
            }
        }
    }

    return transactions;
}