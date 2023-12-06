using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using HouseFun.Models;

namespace HouseFun.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        Northwind northwind;
        Customer customer;
        // Response body use to response the request from apis.
        string? responseBody;

        [HttpGet]
        public ActionResult GetCustomersData()
        {
            northwind = new Northwind();
            responseBody = northwind.SelectCustomersData();

            return Ok(responseBody);
        }

        [HttpGet(template: "{customerID}")]
        public IActionResult GetCustomerDataById(string customerID)
        {
            northwind = new Northwind();
            responseBody = northwind.SelectCustomerDataById(customerID);

            if (responseBody == null)
            {
                return BadRequest(customerID);
            }

            return Ok(responseBody);
        }

        [HttpPost]
        public IActionResult PostCustomersData([FromBody] Customer postCustomer)
        {
            northwind = new Northwind();
            responseBody = northwind.InsertCustomerData(postCustomer);

            if (responseBody == null)
            {
                return BadRequest(postCustomer);
            }

            return Ok(responseBody);
        }

        [HttpPut(template: "{customerID}")]
        public IActionResult PutCustomerData(string customerID, [FromBody] Customer putCustomer)
        {
            northwind = new Northwind();
            responseBody = northwind.PutCustomerDataById(customerID, putCustomer);

            if (responseBody == null)
            {
                return BadRequest(putCustomer);
            }

            return Ok(responseBody);
        }

        [HttpPatch(template: "{customerID}")]
        public IActionResult PatchCustomerData(string customerID, [FromBody] JsonPatchDocument<Customer> patchCustomer)
        {
            if (patchCustomer == null)
            {
                return BadRequest(ModelState);
            }

            northwind = new Northwind();
            customer = new Customer();

            patchCustomer.ApplyTo(customer, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            responseBody = northwind.PatchCustomerDataById(customerID, customer);

            if (responseBody == null)
            {
                return BadRequest(customer);
            }

            return new ObjectResult(responseBody);
        }

        [HttpDelete(template: "{customerID}")]
        public IActionResult DeleteCustomerData(string customerID)
        {
            northwind = new Northwind();
            responseBody = northwind.DeleteCustomerDataById(customerID);

            if(responseBody == null)
            {
                return BadRequest(customerID);
            }

            return Ok(responseBody);
        }
    }
}
