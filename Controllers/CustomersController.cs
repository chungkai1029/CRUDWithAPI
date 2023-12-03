using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HouseFun.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        [HttpGet]
        public void GetCustomersData()
        {
            
        }

        [HttpGet(template: "{id}")]
        public void GetCustomerDataById(int id)
        {

        }

        [HttpPost]
        public void AddCustomersData()
        {

        }

        [HttpPut(template: "{id}")]
        public void UpdateAllOfCustomerData(int id)
        {

        }

        [HttpPatch(template: "{id}")]
        public void UpdatePartOfCustomerData(int id)
        {

        }

        [HttpDelete(template: "{id}")]
        public void DeleteCustomerData(int id)
        {

        }
    }
}
