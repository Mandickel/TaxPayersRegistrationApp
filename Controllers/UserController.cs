using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using TaxPayersRegistration.Models;
using System.Text;
using System.Net.Http.Headers;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TaxPayersRegistration.Controllers
{
    public class UserController : Controller
    {
        Uri baseAddress = new Uri("https://www.mra.mw/sandbox");
        HttpClient client;

        public UserController()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("candidateid", "mandickel@gmail.com");
            client.DefaultRequestHeaders.Add("apikey", "3fdb48c5-336b-47f9-87e4-ae73b8036a1c");
            client.BaseAddress = baseAddress;
        }
        public ActionResult Index()
        {

            return View();
        }    
        [HttpPost]
        public ActionResult Index(UserLoginModel model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/programming/challenge/webservice/auth/login", content).Result;
            if (response.IsSuccessStatusCode)
            {
              
                    string res = "";
                    using (HttpContent content1 = response.Content)
                    {
                        // ... Read the string.
                        Task<string> result = content1.ReadAsStringAsync();
                        res = result.Result;
                    var jo = JObject.Parse(res);
                    var Authorize = jo["Authenticated"].ToString();
                    //creating a cookie
                    
                    if (Authorize == "True")
                    {
                        HttpCookie mycookie = new HttpCookie("mycookie");

                        return RedirectToAction("Clients");
                    }
                    else
                    {

                        return View();
                    }
                    }

                
                //return RedirectToAction("Clients");
            } 

            return View();
        }
        public ActionResult Clients()
        {


            List<UserViewModel> modelList = new List<UserViewModel>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/programming/challenge/webservice/Taxpayers/getAll").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<UserViewModel>>(data);


            }
            return View(modelList);
        }
        //Creating new client 
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(UserViewModel model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/programming/challenge/webservice/Taxpayers/add", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Clients");
            }
            return View();
        }
        //Creating an edit function
        public ActionResult Edit(int id)
        {
            UserViewModel model = new UserViewModel();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/programming/challenge/webservice/Taxpayers/getAll" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<UserViewModel>(data);

            }
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Edit(UserViewModel model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PutAsync(client.BaseAddress + "/programming/challenge/webservice/Taxpayers/add/" + model.id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Edit", model);
        }
        public ActionResult Logout(LogoutModel model1)
        {
            string data = JsonConvert.SerializeObject(model1);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/programming/challenge/webservice/auth/logout", content).Result;
            if (response.IsSuccessStatusCode)
            {

                string res = "";
                using (HttpContent content1 = response.Content)
                {
                    // ... Read the string.
                    Task<string> result = content1.ReadAsStringAsync();
                    res = result.Result;
                    var jo = JObject.Parse(res);
                    var Logout = jo["Remark"].ToString();
                    if (Logout == "Log out Successful")
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View();
                    }
                }
               
            }

            return View();
            
        }

    }
} 