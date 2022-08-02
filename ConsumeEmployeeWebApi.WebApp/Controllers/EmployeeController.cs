using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConsumeEmployeeWebApi.WebApp.Controllers
{
    public class EmployeeController : Controller
    {
        Uri baseUri = new Uri("https://localhost:7112/api");

        HttpClient client = new HttpClient();

        List<EmployeeViewModel> employeeList = new List<EmployeeViewModel>();


        public IActionResult Index()
        {
            client.BaseAddress = baseUri;
            HttpResponseMessage response = client.GetAsync(baseUri + "/employee").Result;
            if (response.IsSuccessStatusCode)
            {
                string EmployeeData = response.Content.ReadAsStringAsync().Result;
                employeeList = JsonConvert.DeserializeObject<List<EmployeeViewModel>>(EmployeeData); ;

            }

            return View(employeeList);
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult CreateNewEmployee(EmployeeViewModel employee)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7112/api/employee");
                var post = client.PostAsJsonAsync<EmployeeViewModel>("employee", employee);
                post.Wait();
                var result = post.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");

                }
                ModelState.AddModelError(string.Empty, "Server Error");
                return View(employee);
            }
        }

        public ActionResult Update(int id)
        {
            client.BaseAddress = baseUri;
            HttpResponseMessage response = client.GetAsync(baseUri + "/Employee").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            employeeList = JsonConvert.DeserializeObject<List<EmployeeViewModel>>(data);
            var emp = employeeList.Where(e => e.id == id).FirstOrDefault();
            return View(emp);
        }
        [HttpPost]
        public IActionResult Save(EmployeeViewModel employee)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7112/api/employee");
                var put = client.PutAsJsonAsync($"Employee?empId={employee.id}", employee);
                put.Wait();
                var result = put.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

            }
            ModelState.AddModelError(string.Empty, "server error");
            return View();

        }
        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7112/api/employee");
                var put = client.DeleteAsync($"Employee?empId={id}");
                put.Wait();
                var result = put.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

            }
            ModelState.AddModelError(string.Empty, "server error");
            return View();

        }

    }
    
}
