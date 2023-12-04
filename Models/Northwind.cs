using System.Text;
using System.Text.Json;
using System.Data.SqlClient;

namespace HouseFun.Models
{
    public class Northwind
    {
        // Model and list to handle all data read from DataTable column.
        List<Customer> customerList;
        Customer customer;

        SqlConnection sqlConnection;
        SqlCommand sqlCommand;

        //
        // Connect string use to connect with Northwind.dbo.Customers.
        StringBuilder connectString = new StringBuilder("Server=localhost;Database=master;Trusted_Connection=True;");
        // Query string use to get data from the DataTable, or insert/update data into DataTable.
        StringBuilder queryString;

        // A return string to send back to calling place with a json format.
        string jsonString;

        public string? SelectCustomersData()
        {
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
                // Initalize the model and list of model to store data read from DataTable.
                try
                {
                    customerList = new List<Customer>();

                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        customer = new Customer
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
                        customerList.Add(customer);
                    }

                    // Serialize the list get from DataTable as a json string.
                    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

                    jsonString = JsonSerializer.Serialize(customerList, options);
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
                        customer = new Customer
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

                    // Serialize the model get from DataTable as a json string.
                    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

                    jsonString = JsonSerializer.Serialize(customer, options);
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

        public string? InsertCustomerData(Customer insertCustomer)
        {
            queryString = new StringBuilder();
            queryString.Append("INSERT INTO Northwind.dbo.Customers(CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax) ");
            queryString.Append("VALUES(@CustomerID, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax)");

            using (sqlConnection = new SqlConnection(connectString.ToString()))
            {
                sqlCommand = new(queryString.ToString(), sqlConnection);

                try
                {
                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@CustomerID", insertCustomer.CustomerID);
                    sqlCommand.Parameters.AddWithValue("@CompanyName", insertCustomer.CompanyName);
                    sqlCommand.Parameters.AddWithValue("@ContactName", insertCustomer.ContactName);
                    sqlCommand.Parameters.AddWithValue("@ContactTitle", insertCustomer.ContactTitle);
                    sqlCommand.Parameters.AddWithValue("@Address", insertCustomer.Address);
                    sqlCommand.Parameters.AddWithValue("@City", insertCustomer.City);
                    sqlCommand.Parameters.AddWithValue("@Region", insertCustomer.Region);
                    sqlCommand.Parameters.AddWithValue("@PostalCode", insertCustomer.PostalCode);
                    sqlCommand.Parameters.AddWithValue("@Country", insertCustomer.Country);
                    sqlCommand.Parameters.AddWithValue("@Phone", insertCustomer.Phone);
                    sqlCommand.Parameters.AddWithValue("@Fax", insertCustomer.Fax);

                    int result = sqlCommand.ExecuteNonQuery();

                    if (result < 0)
                    {
                        Console.WriteLine("Error inserting data into DataTable.");
                        return null;
                    }

                    // Serialize the model get from DataTable as a json string.
                    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

                    jsonString = JsonSerializer.Serialize(insertCustomer, options);
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
