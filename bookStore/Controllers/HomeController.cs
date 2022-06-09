using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using bookStore.Models;
using bookStore.Service;




namespace bookStore.Controllers {

    // universal routing
    //[Route("[controller]/[action]")]
    public class HomeController : Controller {

        // reads settings from appsettings.json 
        private readonly IConfiguration configuration;
        private readonly IUserService _userService;

        private readonly IEmailService _emailService;
        public HomeController(IConfiguration _configuration, IUserService userService,
        IEmailService emailService)
        {
            configuration = _configuration;
            _userService = userService;
            _emailService = emailService;
        }

        [ViewData]
        public string CustomProp { get; set; }
        [ViewData]
        public string Title { get; set; }
        [ViewData]
        public BookModel Book { get; set; }

        [Route("~/ ")]
        //[Route("[controller]/[action]")]
        public async Task<ViewResult> Index(){
            // ViewBag.Title = "usingizinayo";
            // // ViewBag.Data = new { Id=12313, Name="Tarza"};
            

            // dynamic data = new ExpandoObject();
            // data.Id = 3434;
            // data.Name = "nfjf";
            // ViewBag.Data = data;
            // ViewBag.Type = new BookModel() {
            //      Id=12313, Author="Teabag"
            // };

            // ViewData["prop1"] = "tracymac";
            // ViewData["book"] = new BookModel() { Author="ruru", Id=1};
            // view data attribute
            // ViewData["CustomProp"] = "value";
            // Book = new BookModel() {};
            // CustomProp = "some other value";
            // Title = "home";
            // var result = configuration["AppName"];
            // var k1 = configuration["infoObj:key1"];
            // var k2 = configuration["infoObj:key2"];
            // // var kkk = configuration.GetSection("infoObj");
            // // var ka = kkk.GetValue<bool>();
            // var k3 = configuration["infoObj:key3:k3obj1"];

            // var tres = configuration.GetValue<bool>("DisplayAlert");
            
            // getting the logged in user id
            var userId = _userService.GetUserId();
            // check if the user is authenticated
            var isLoggedIn = _userService.isAuthenticated();


            UserEmailModel options = new UserEmailModel{
                ToEmails = new List<string>() {"testing@gmail.com"},
                Placeholders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", "jii")
                }
            };
            await _emailService.SendTestEmail(options);
            return View();
        }

        // attribute routing
        // [Route("about-us/{?}/{name:alpha:minlength(5):regex()}/{?}")]
        
        public ViewResult AboutUs() {

            Title = "aboutus";
            return View();
        }

        public ViewResult ContactUs() {

            Title = "contactus";
            return View();
        }
    }
}