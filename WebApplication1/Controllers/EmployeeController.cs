using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee  
        public ActionResult EmployeeInfo()
        {
            Employee employee = new Employee()
            {
                EmployeeId = 1001,
                EmployeeName = "Khaja Moiz",
                EmployeeLocation = "Hyderabad",
                EmailId = "test@test.com"
            };


            return View(employee);
        }
    }
}