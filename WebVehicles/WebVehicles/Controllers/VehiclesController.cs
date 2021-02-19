using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebVehicles.Models;

namespace WebVehicles.Controllers
{
    public class VehiclesController : Controller
    {
        // GET: VehiclesController
        string Baseurl = "https://localhost:44336/";
        public async Task<IActionResult> Index()
        {
            List<VehicleViewModel> list = new List<VehicleViewModel>();
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri(Baseurl);
                cliente.DefaultRequestHeaders.Clear();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cliente.GetAsync("api/vehicles/");
                if (res.IsSuccessStatusCode)
                {
                    var Response = res.Content.ReadAsStringAsync().Result;
                    list = JsonConvert.DeserializeObject<List<VehicleViewModel>>(Response);
                }
                return View(list);
            }
        }
        // GET: VehiclesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VehiclesController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: VehiclesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult AddVehicle() => View();

        [HttpPost]
        public async Task<IActionResult> AddVehicle(VehicleViewModel vehicle)
        {
            VehicleViewModel request = new VehicleViewModel();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(vehicle), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:44336/api/vehicles", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    request = JsonConvert.DeserializeObject<VehicleViewModel>(apiResponse);
                }
            }
            return View(request);
        }
        // GET: VehiclesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VehiclesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VehiclesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VehiclesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
