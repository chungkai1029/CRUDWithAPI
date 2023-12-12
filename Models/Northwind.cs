using System.Text;
using System.Text.Json;
using System.Data.SqlClient;
using System.Reflection;

namespace SqlServerCRUD.Models
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
            queryString.Append("FROM Northwind.dbo.Customers;");

            // Create and open the connection in a using block.
            // This ensures that all resources will be closed and disposed when the code exits.
            using (sqlConnection = new SqlConnection(connectString.ToString()))
            {
                sqlCommand = new(queryString.ToString(), sqlConnection);

                // Open the connection in a try/catch block.
                // Create and execute the DataReader, writing the result set to the model.
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
                            Fax = reader[10].ToString()
                        };

                        // After reading for a cycle, store them into the list.
                        customerList.Add(customer);
                    }

                    // Serialize the list get from DataTable as a json string.
                    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

                    jsonString = JsonSerializer.Serialize(customerList, options);
                    return jsonString;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
        }

        public string? SelectCustomerDataById(string customerID)
        {
            queryString = new StringBuilder();
            queryString.Append("SELECT CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax ");
            queryString.Append("FROM Northwind.dbo.Customers ");
            queryString.Append($"WHERE CustomerID = \'{customerID}\';");

            using (sqlConnection = new SqlConnection(connectString.ToString()))
            {
                sqlCommand = new(queryString.ToString(), sqlConnection);

                try
                {
                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    // If the reader cannot find the target row then jump out, but it will be failed.
                    //if (!reader.Read())
                    //{
                    //    return null;
                    //}

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
                            Fax = reader[10].ToString()
                        };
                    }

                    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

                    jsonString = JsonSerializer.Serialize(customer, options);
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
            queryString.Append("VALUES(@CustomerID, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax);");

            using (sqlConnection = new SqlConnection(connectString.ToString()))
            {
                sqlCommand = new(queryString.ToString(), sqlConnection);

                try
                {
                    sqlConnection.Open();

                    // Assign value to the target column and handle null exception.
                    int insertResult  = ParametersAddWithValue(sqlCommand, insertData);

                    if (insertResult < 0)
                    {
                        Console.WriteLine("Insert data into DataTable error.");
                        return null;
                    }

                    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

                    jsonString = JsonSerializer.Serialize(insertData, options);
                    return jsonString;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
        }

        public string? PutCustomerDataById(string customerID, Customer putData)
        {
            queryString = new StringBuilder();
            queryString.Append("UPDATE Northwind.dbo.Customers ");
            queryString.Append("SET CustomerID = @CustomerID, CompanyName = @CompanyName, ContactName = @ContactName, ContactTitle = @ContactTitle, Address = @Address, City = @City, Region = @Region, PostalCode = @PostalCode, Country = @Country, Phone = @Phone, Fax = @Fax ");
            queryString.Append($"WHERE CustomerID = \'{customerID}\';");

            using (sqlConnection = new SqlConnection(connectString.ToString()))
            {
                sqlCommand = new SqlCommand(queryString.ToString(), sqlConnection);

                try
                {
                    sqlConnection.Open();

                    // Update all of value to the target column and handle null exception.
                    int putResult = ParametersAddWithValue(sqlCommand, putData);

                    if (putResult < 0)
                    {
                        Console.WriteLine("Update data of DataTable error.");
                        return null;
                    }

                    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

                    jsonString = JsonSerializer.Serialize(putData, options);
                    return jsonString;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
        }

        public string? PatchCustomerDataById(string customerID, Customer patchData)
        {
            queryString = new StringBuilder();
            queryString.Append("UPDATE Northwind.dbo.Customers ");

            PropertyInfo[] properties = typeof(Customer).GetProperties();
            int propertyCount = 0;

            foreach (PropertyInfo property in properties)
            {
                // Set the value that get from request to the property and check whether value is null.
                var value = property.GetValue(patchData);

                if (value == null)
                {
                    continue;
                }

                if (propertyCount == 0)
                {
                    queryString.Append($"SET {property.Name} = \'{value}\'");
                    propertyCount++;
                    continue;
                }

                queryString.Append($", {property.Name} = \'{value}\' ");
                propertyCount++;
            }

            queryString.Append($"WHERE CustomerID = \'{customerID}\'");

            using (sqlConnection = new SqlConnection(connectString.ToString()))
            {
                sqlCommand = new SqlCommand(queryString.ToString(), sqlConnection);

                try
                {
                    sqlConnection.Open();

                    int patchResult = sqlCommand.ExecuteNonQuery();

                    if (patchResult < 0)
                    {
                        Console.WriteLine("Update data of DataTable error.");
                        return null;
                    }

                    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

                    jsonString = JsonSerializer.Serialize(patchData, options);
                    return jsonString;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
        }

        public string? DeleteCustomerDataById(string customerID)
        {
            queryString = new StringBuilder();
            queryString.Append($"DELETE FROM Northwind.dbo.Customers WHERE CustomerID = \'{customerID}\';");

            using (SqlConnection sqlConnection = new SqlConnection(connectString.ToString()))
            {
                sqlCommand = new SqlCommand(queryString.ToString(), sqlConnection);

                try
                {
                    sqlConnection.Open();
                    int result = sqlCommand.ExecuteNonQuery();

                    if (result < 0)
                    {
                        Console.WriteLine("Delete data from DataTable error.");
                        return null;
                    }

                    Console.WriteLine("Delete data from DataTable success.");
                    return "Success";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
        }

        int ParametersAddWithValue(SqlCommand command, Customer addCustomer)
        {
            command.Parameters.AddWithValue("@CustomerID", addCustomer.CustomerID);
            command.Parameters.AddWithValue("@CompanyName", addCustomer.CompanyName);
            command.Parameters.AddWithValue("@ContactName", addCustomer.ContactName == null ? DBNull.Value : addCustomer.ContactName);
            command.Parameters.AddWithValue("@ContactTitle", addCustomer.ContactTitle == null ? DBNull.Value : addCustomer.ContactTitle);
            command.Parameters.AddWithValue("@Address", addCustomer.Address == null ? DBNull.Value : addCustomer.Address);
            command.Parameters.AddWithValue("@City", addCustomer.City == null ? DBNull.Value : addCustomer.City);
            command.Parameters.AddWithValue("@Region", addCustomer.Region == null ? DBNull.Value : addCustomer.Region);
            command.Parameters.AddWithValue("@PostalCode", addCustomer.PostalCode == null ? DBNull.Value : addCustomer.PostalCode);
            command.Parameters.AddWithValue("@Country", addCustomer.Country == null ? DBNull.Value : addCustomer.Country);
            command.Parameters.AddWithValue("@Phone", addCustomer.Phone == null ? DBNull.Value : addCustomer.Phone);
            command.Parameters.AddWithValue("@Fax", addCustomer.Fax == null ? DBNull.Value : addCustomer.Fax);

            return command.ExecuteNonQuery();
        }
    }
}
