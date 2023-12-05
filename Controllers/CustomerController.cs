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
        // Response body use to response the request from apis.
        string? responseBody;

        [HttpGet]
        public ActionResult GetCustomersData()
        {
            northwind = new Northwind();
            responseBody = northwind.SelectCustomersData();

            return Ok(responseBody);
        }

        [HttpGet(template: "{contactName}")]
        public IActionResult GetCustomerDataById(string contactName)
        {
            northwind = new Northwind();
            responseBody = northwind.SelectCustomerDataById(contactName);

            if (responseBody == null)
            {
                return BadRequest(contactName);
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

        [HttpPut(template: "{contactName}")]
        public IActionResult PutCustomerData(string contactName, [FromBody] Customer putCustomer)
        {
            northwind = new Northwind();
            responseBody = northwind.PutCustomerData(contactName, putCustomer);

            if (responseBody == null)
            {
                return BadRequest(putCustomer);
            }

            return Ok(responseBody);
        }

        [HttpPatch(template: "{contactName}")]
        public void PatchCustomerData(string contactName, [FromBody] JsonPatchDocument<Customer> patchCustomer)
        {
            northwind = new Northwind();
            northwind.PatchCustomerData(contactName, patchCustomer);
        }

        [HttpDelete(template: "{contactName}")]
        public void DeleteCustomerData(string contactName)
        {
            northwind = new Northwind();
            northwind.DeleteCustomerData(contactName);
        }
    }
}
