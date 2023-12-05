using System.Text;
using System.Text.Json;
using System.Data.SqlClient;
using Microsoft.AspNetCore.JsonPatch;
using System.Reflection;

namespace HouseFun.Models
{
    public class Northwind
    {
        // Model and list to handle all data read from DataTable column.
        List<Customer> customerList;
        Customer customer;

        SqlConnection sqlConnection;
        SqlCommand sqlCommand;

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

        public string? InsertCustomerData(Customer insertData)
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

                    // Assign value to the target column.
                    sqlCommand.Parameters.AddWithValue("@CustomerID", insertData.CustomerID);
                    sqlCommand.Parameters.AddWithValue("@CompanyName", insertData.CompanyName);
                    sqlCommand.Parameters.AddWithValue("@ContactName", insertData.ContactName);
                    sqlCommand.Parameters.AddWithValue("@ContactTitle", insertData.ContactTitle);
                    sqlCommand.Parameters.AddWithValue("@Address", insertData.Address);
                    sqlCommand.Parameters.AddWithValue("@City", insertData.City);
                    sqlCommand.Parameters.AddWithValue("@Region", insertData.Region);   // Null exception has not complete.
                    sqlCommand.Parameters.AddWithValue("@PostalCode", insertData.PostalCode);   // Null exception has not complete.
                    sqlCommand.Parameters.AddWithValue("@Country", insertData.Country);
                    sqlCommand.Parameters.AddWithValue("@Phone", insertData.Phone);
                    sqlCommand.Parameters.AddWithValue("@Fax", insertData.Fax); // Null exception has not complete.

                    int result = sqlCommand.ExecuteNonQuery();

                    if (result < 0)
                    {
                        Console.WriteLine("Error inserting data into DataTable.");
                        return null;
                    }

                    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

                    jsonString = JsonSerializer.Serialize(insertData, options);
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

        public string? PutCustomerData(string contactName, Customer putData)
        {
            queryString = new StringBuilder();
            queryString.Append("UPDATE Northwind.dbo.Customers ");
            queryString.Append("SET CustomerID = @CustomerID, CompanyName = @CompanyName, ContactName = @ContactName, ContactTitle = @ContactTitle, Address = @Address, City = @City, Region = @Region, PostalCode = @PostalCode, Country = @Country, Phone = @Phone, Fax = @Fax ");
            queryString.Append($"WHERE ContactName = \'{contactName}\'");

            using (sqlConnection = new SqlConnection(connectString.ToString()))
            {
                sqlCommand = new SqlCommand(queryString.ToString(), sqlConnection);

                try
                {
                    sqlConnection.Open();

                    // Update value to the target column.
                    sqlCommand.Parameters.AddWithValue("@CustomerID", putData.CustomerID);
                    sqlCommand.Parameters.AddWithValue("@CompanyName", putData.CompanyName);
                    sqlCommand.Parameters.AddWithValue("@ContactName", putData.ContactName);
                    sqlCommand.Parameters.AddWithValue("@ContactTitle", putData.ContactTitle);
                    sqlCommand.Parameters.AddWithValue("@Address", putData.Address);
                    sqlCommand.Parameters.AddWithValue("@City", putData.City);
                    sqlCommand.Parameters.AddWithValue("@Region", putData.Region);  // Null exception has not complete.
                    sqlCommand.Parameters.AddWithValue("@PostalCode", putData.PostalCode);  // Null exception has not complete.
                    sqlCommand.Parameters.AddWithValue("@Country", putData.Country);
                    sqlCommand.Parameters.AddWithValue("@Phone", putData.Phone);
                    sqlCommand.Parameters.AddWithValue("@Fax", putData.Fax);    // Null exception has not complete.

                    int result = sqlCommand.ExecuteNonQuery();

                    if (result < 0)
                    {
                        Console.WriteLine("Error updating data of DataTable.");
                        return null;
                    }

                    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

                    jsonString = JsonSerializer.Serialize(putData, options);
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

        public void PatchCustomerData(string contactName, JsonPatchDocument<Customer> patch)
        {
            queryString = new StringBuilder();
            queryString.Append("UPDATE Northwind.dbo.Customers ");
            queryString.Append("SET CustomerID = @CustomerID, CompanyName = @CompanyName, ContactName = @ContactName, ContactTitle = @ContactTitle, Address = @Address, City = @City, Region = @Region, PostalCode = @PostalCode, Country = @Country, Phone = @Phone, Fax = @Fax ");
            queryString.Append($"WHERE ContactName = \'{contactName}\'");
        }

        public void DeleteCustomerData(string contactName)
        {
            queryString = new StringBuilder();
            queryString.Append($"DELETE FROM Northwind.dbo.Customers WHERE ContactName = \'{contactName}\'");
        }
    }
}
