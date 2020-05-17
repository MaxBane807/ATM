using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ATM.Web.Models;

namespace ATM.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly ICalculator _Calculator;

        public HomeController(ILogger<HomeController> logger, ICalculator calculator)
        {
            _logger = logger;
            _Calculator = calculator;
        }

        public IActionResult Index()
        {
            var viewmodel = new AtmViewModel();
            viewmodel.BillsGivenOut = new List<Bill>()
            {
                new Bill {Value = 1000, Amount = 0 },
                new Bill {Value = 500, Amount = 0 },
                new Bill {Value = 100, Amount = 0}
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Index(AtmViewModel viewModel)
        {
            var tempBills = TempData["GivenOutBills"];
            TempData.Clear();
            List<Bill> billsGivenOut = null;

            if (tempBills != null)
            {
                billsGivenOut = JsonConvert.DeserializeObject<List<Bill>>(tempBills.ToString());
            }

            var BillsInAtm = _Calculator.UpdateBills(billsGivenOut);

            var emptylist = new List<Bill>()
            {
                new Bill { Value = 1000, Amount = 0},
                new Bill {Value = 500, Amount = 0},
                new Bill {Value = 100, Amount = 0}
            };

            billsGivenOut = (billsGivenOut == null ? emptylist : billsGivenOut);
            viewModel.BillsGivenOut = billsGivenOut;

            if (_Calculator.GetIfAtmISEmpty(BillsInAtm))
            {
                ModelState.AddModelError(string.Empty, "The Atm is empty");
                TempData["GivenOutBills"] = JsonConvert.SerializeObject(billsGivenOut);
                return View(viewModel);
            }
            else if (!_Calculator.GetIfValueExists(BillsInAtm, viewModel.ValueRequested))
            {
                ModelState.AddModelError(string.Empty, "The Atm dosn't contain that value");
                TempData["GivenOutBills"] = JsonConvert.SerializeObject(billsGivenOut);
                return View(viewModel);
            }
            else
            {
                var result = _Calculator.CalculateBillsToWithdraw(BillsInAtm, viewModel.ValueRequested);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "The Atm don't have bills that add up to that sum");
                    TempData["GivenOutBills"] = JsonConvert.SerializeObject(billsGivenOut);
                    return View(viewModel);
                }
                else
                {
                    var updatedList = _Calculator.UpdateWithdrawnBills(result, viewModel.BillsGivenOut);
                    viewModel.BillsGivenOut = updatedList;
                    TempData["GivenOutBills"] = JsonConvert.SerializeObject(updatedList);
                    return View(viewModel);
                }
            }
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
    }
}
