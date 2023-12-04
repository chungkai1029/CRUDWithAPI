using Microsoft.AspNetCore.Mvc;
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
        public IActionResult PostCustomersData([FromBody] Customer customers)
        {
            northwind = new Northwind();
            responseBody = northwind.InsertCustomerData(customers);

            if (responseBody == null)
            {
                return BadRequest(customers);
            }

            return Ok(responseBody);
        }

        [HttpPut(template: "{contactName}")]
        public void PutCustomerData(string contactName)
        {

        }

        [HttpPatch(template: "{contactName}")]
        public void PatchCustomerData(string contactName)
        {

        }

        [HttpDelete(template: "{contactName}")]
        public void DeleteCustomerData(string contactName)
        {

        }
    }
}
