using IOSU.Data;
using IOSU.Models;
using IOSU.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using Xceed.Words.NET;

namespace IOSU.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Запросы
        public async Task<IActionResult> LastAdmission()
        {
            string query = "SELECT * from dbo.Product where dbo.Product.EmDate >" +
                " DATEADD(DAY, -10, CAST( GETDATE() AS Date ));";

            var products = await _context.Product.Where(x => x.EmDate >= DateTime.
            Now.AddDays(-10)).ToListAsync();
            return View("../Products/Index", products);
        }

        public async Task<IActionResult> RatingClient()
        {
            var clients = await _context.Client.Include(x => x.Contracts).
                OrderByDescending(x => x.Contracts.Count).ToListAsync();
            return View("../Clients/Index", clients);
        }

        [HttpGet]
        public IActionResult ClientsWithProduct()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ClientsWithProduct([Bind("Id")] Product product)
        {
            var contracts = await _context.Contract.Include(x => x.Client).
                Where(x => x.ProductId == product.Id).ToListAsync();
            Client[] clients = new Client[contracts.Count];

            for (int i = 0; i < contracts.Count; i++)
            {
                clients[i] = contracts[i].Client;
            }

            return View("../Clients/Index", clients);
        }

        public IActionResult Dynamic()
        {
            SqlConnection con = new SqlConnection("Server=DESKTOP-6O4SV1L\\SQLEXPRESS;Database=IOSU;Trusted_Connection=true;");


            con.Open();
            SqlCommand cmd = new SqlCommand("select Product.Name," +
                "(select sum(AmountOfProduct) from Contract where ProductId = Product.Id and FORMAT(Date, 'MM-dd') between '01-01' and '01-31') as Январь," +
                "(select sum(AmountOfProduct) from Contract where ProductId = Product.Id and FORMAT(Date, 'MM-dd') between '02-01' and '02-28') as Февраль," +
                "(select sum(AmountOfProduct) from Contract where ProductId = Product.Id and FORMAT(Date, 'MM-dd') between '03-01' and '03-31') as Март," +
                "(select sum(AmountOfProduct) from Contract where ProductId = Product.Id and FORMAT(Date, 'MM-dd') between '04-01' and '04-30') as Апрель," +
                "(select sum(AmountOfProduct) from Contract where ProductId = Product.Id and FORMAT(Date, 'MM-dd') between '05-01' and '05-31') as Май," +
                "(select sum(AmountOfProduct) from Contract where ProductId = Product.Id and FORMAT(Date, 'MM-dd') between '06-01' and '06-30') as Июнь," +
                "(select sum(AmountOfProduct) from Contract where ProductId = Product.Id and FORMAT(Date, 'MM-dd') between '07-01' and '07-31') as Июль," +
                "(select sum(AmountOfProduct) from Contract where ProductId = Product.Id and FORMAT(Date, 'MM-dd') between '08-01' and '08-31') as Август," +
                "(select sum(AmountOfProduct) from Contract where ProductId = Product.Id and FORMAT(Date, 'MM-dd') between '09-01' and '09-30') as Сентябрь," +
                "(select sum(AmountOfProduct) from Contract where ProductId = Product.Id and FORMAT(Date, 'MM-dd') between '10-01' and '10-31') as Октябрь," +
                "(select sum(AmountOfProduct) from Contract where ProductId = Product.Id and FORMAT(Date, 'MM-dd') between '11-01' and '11-30') as Ноябрь," +
                "(select sum(AmountOfProduct) from Contract where ProductId = Product.Id and FORMAT(Date, 'MM-dd') between '12-01' and '12-31') as Декабрь," +
                "(select sum(AmountOfProduct) from Contract where ProductId = Product.Id) as Всего " +
                "from Product left join Contract on Product.Id = Contract.ProductId;", con);
            SqlDataReader srd = cmd.ExecuteReader();

            List<DynamicViewModel> model = new List<DynamicViewModel>();

            while (srd.Read())
            {
                var temp = new DynamicViewModel();
                if (!model.Any(x => x.Name == (string)srd.GetValue(0)))
                {
                    temp.Name = (string)srd.GetValue(0);
                    temp.January = Convert.IsDBNull(srd.GetValue(1)) ? 0 : (int?)srd.GetValue(1);
                    temp.February = Convert.IsDBNull(srd.GetValue(2)) ? 0 : (int?)srd.GetValue(2);
                    temp.March = Convert.IsDBNull(srd.GetValue(3)) ? 0 : (int?)srd.GetValue(3);
                    temp.April = Convert.IsDBNull(srd.GetValue(4)) ? 0 : (int?)srd.GetValue(4);
                    temp.May = Convert.IsDBNull(srd.GetValue(5)) ? 0 : (int?)srd.GetValue(5);
                    temp.June = Convert.IsDBNull(srd.GetValue(6)) ? 0 : (int?)srd.GetValue(6);
                    temp.July = Convert.IsDBNull(srd.GetValue(7)) ? 0 : (int?)srd.GetValue(7);
                    temp.August = Convert.IsDBNull(srd.GetValue(8)) ? 0 : (int?)srd.GetValue(8);
                    temp.September = Convert.IsDBNull(srd.GetValue(9)) ? 0 : (int?)srd.GetValue(9);
                    temp.October = Convert.IsDBNull(srd.GetValue(10)) ? 0 : (int?)srd.GetValue(10);
                    temp.November = Convert.IsDBNull(srd.GetValue(11)) ? 0 : (int?)srd.GetValue(11);
                    temp.December = Convert.IsDBNull(srd.GetValue(12)) ? 0 : (int?)srd.GetValue(12);
                    temp.All = Convert.IsDBNull(srd.GetValue(13)) ? null : (int?)srd.GetValue(13);
                    model.Add(temp);
                }
            }
            con.Close();
            return View(model);
        }

        public IActionResult ManufacturersClients()
        {
            List<string> model = new List<string>();
            ViewBag.MC = new List<string>();

            SqlConnection con = new SqlConnection("Server=DESKTOP-6O4SV1L\\SQLEXPRESS;Database=IOSU;Trusted_Connection=true;");

            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Name, 'Заказчик' as Принадлежность FROM dbo.Client " +
                "UNION SELECT Name, 'Производитель' as Принадлежность FROM dbo.Manufacturer ", con);
            SqlDataReader srd = cmd.ExecuteReader();

            while (srd.Read())
            {
                model.Add((string)srd.GetValue(0));
                ViewBag.Mc.Add((string)srd.GetValue(1));
            }
            con.Close();

            return View(model);
        }

        public async Task<IActionResult> ConractDoc(int contractId)
        {

            var contract = await _context.Contract.Include(x => x.Manager).Include(x => x.Client).Include(x => x.Product).
                FirstOrDefaultAsync(x => x.Id == contractId);

            string fileName = "wwwroot/contracts/" + contract.Date.ToString("dd.MM.yyyy") + $"_{contract.Id}_IdContract";

            var doc = DocX.Load("wwwroot/Contract_sample.doc");
            var doc2 = doc.Copy();

            doc2.ReplaceText("<ORG>", contract.Client.Name);
            doc2.ReplaceText("<PRODUCT>", contract.Product.Name);
            doc2.ReplaceText("<COUNT>", contract.AmountOfProduct.ToString());
            doc2.ReplaceText("<MANAGER_NAME>", contract.Manager.FullName);
            doc2.ReplaceText("<DATE>", contract.Date.ToString("dd.MM.yyyy"));

            doc.Save();
            doc2.SaveAs(fileName);

            return View("../Contracts/Index", await _context.Contract.
                Include(c => c.Client).Include(c => c.Manager).Include(c => c.Product).ToListAsync());
        }
    }
}