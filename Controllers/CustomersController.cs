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
        public ActionResult GetCustomerDataById(string contactName)
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
        public void PostCustomersData()
        {

        }

        [HttpPut(template: "{id}")]
        public void PutCustomerData(int id)
        {

        }

        [HttpPatch(template: "{id}")]
        public void PatchCustomerData(int id)
        {

        }

        [HttpDelete(template: "{id}")]
        public void DeleteCustomerData(int id)
        {

        }
    }
}
