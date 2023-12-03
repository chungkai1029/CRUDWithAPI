using System.Text;
using System.Text.Json;
using System.Data.SqlClient;

namespace HouseFun.Models
{
    public class Northwind
    {
        // Model and list to handle all data read from dataTable column.
        List<Customers> customersList;
        Customers customers;

        SqlConnection sqlConnection;
        SqlCommand sqlCommand;

        // Connect string use to connect with Northwind.dbo.Customers.
        StringBuilder connectString = new StringBuilder("Data Source=(local);Initial Catalog=Northwind;Integrated Security=true");
        // Query string use to get or set data to the dataTable.
        StringBuilder queryString;

        // A return string to send back to calling place with a json format.
        string jsonString;

        public string? SelectCustomersData()
        {
            // Query string use to get data from the dataTable.
            queryString = new StringBuilder();
            queryString.Append("SELECT CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax ");
            queryString.Append("FROM Northwind.dbo.Customers");

            // Create and open the connection in a using block.
            // This ensures that all resources will be closed and disposed when the code exits.
            using (sqlConnection = new SqlConnection(connectString.ToString()))
            {
                sqlCommand = new(queryString.ToString(), sqlConnection);

                // Open the connection in a try/catch block.
                // Create and execute the DataReader, writing the result set to the console window.
                // Initalize the model and list of model to store data read from dataTable.
                try
                {
                    customersList = new List<Customers>();

                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        customers = new Customers
                        {
                            CustomerID = reader[0].ToString(),
                            CompanyName = reader[1].ToString(),
                            ContactName = reader[2].ToString(),
                            ContactTitle = reader[3].ToString(),
                            Address = reader[4].ToString(),
                            City = reader[5].ToString(),
                            Region = reader[6].ToString(),
                            PostalCode = reader[7].ToString(),
                            Country = reader[8].ToString(),
                            Phone = reader[9].ToString(),
                            Fax = reader[10].ToString(),
                        };

                        // After reading for a cycle, store them into the list.
                        customersList.Add(customers);
                    }

                    // Serialize the list get from dataTable as a json string.
                    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

                    jsonString = JsonSerializer.Serialize(customersList, options);
                    Console.WriteLine($"customersList={jsonString}");
                    return jsonString;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
        }

        public string? SelectCustomerDataById(string contactName)
        {
            // Query string use to get data from the dataTable with a condition.
            queryString = new StringBuilder();
            queryString.Append("SELECT CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax ");
            queryString.Append("FROM Northwind.dbo.Customers ");
            queryString.Append($"WHERE ContactName = \'{contactName}\'");

            using (sqlConnection = new SqlConnection(connectString.ToString()))
            {
                sqlCommand = new(queryString.ToString(), sqlConnection);

                try
                {
                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    // If the reader cannot find the target row then jump out.
                    if (!reader.Read())
                    {
                        return null;
                    }
                    
                    while (reader.Read())
                    {
                        customers = new Customers
                        {
                            CustomerID = reader[0].ToString(),
                            CompanyName = reader[1].ToString(),
                            ContactName = reader[2].ToString(),
                            ContactTitle = reader[3].ToString(),
                            Address = reader[4].ToString(),
                            City = reader[5].ToString(),
                            Region = reader[6].ToString(),
                            PostalCode = reader[7].ToString(),
                            Country = reader[8].ToString(),
                            Phone = reader[9].ToString(),
                            Fax = reader[10].ToString(),
                        };
                    }

                    // Serialize the model get from dataTable as a json string.
                    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

                    jsonString = JsonSerializer.Serialize(customers, options);
                    Console.WriteLine($"customers={jsonString}");

                    return jsonString;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
        }
    }
}
