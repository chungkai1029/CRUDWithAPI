using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http.Extensions;
using SqlServerCRUD.Models;

namespace SqlServerCRUD.Controllers
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
            if(postCustomer == null)
            {
                return BadRequest(postCustomer);
            }

            northwind = new Northwind();
            responseBody = northwind.InsertCustomerData(postCustomer);

            if (responseBody == null)
            {
                return BadRequest(postCustomer);
            }

            return Created(Request.GetDisplayUrl(), responseBody);
            //return Ok(responseBody);
        }

        [HttpPut(template: "{customerID}")]
        public IActionResult PutCustomerData(string customerID, [FromBody] Customer putCustomer)
        {
            if(putCustomer == null)
            {
                return BadRequest(putCustomer);
            }

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

            return Ok(responseBody);
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

            return NoContent();
        }
    }
}
