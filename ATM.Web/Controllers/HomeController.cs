using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ATM.Web.Models;
using ATM.Web.Services.Interfaces;
using ATM.Web.ViewModels;
using Newtonsoft.Json;

namespace ATM.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly ICalculator _Calculator;
        private readonly IBillRepository _BillRepository;

        public HomeController(ILogger<HomeController> logger, ICalculator calculator, IBillRepository billRepository)
        {
            _logger = logger;
            _Calculator = calculator;
            _BillRepository = billRepository;
        }

        public IActionResult Index()
        {
            var viewmodel = new AtmViewModel();
            viewmodel.BillsGivenOut = _BillRepository.GetGivenOutBills();
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Index(AtmViewModel viewModel)
        {

            viewModel.BillsGivenOut = _BillRepository.GetGivenOutBills();
            var BillsInAtm = _BillRepository.GetBillsInAtm();
            

            if (_Calculator.GetIfAtmISEmpty(BillsInAtm))
            {
                ModelState.AddModelError(string.Empty, "The Atm is empty");
                return View(viewModel);
            }
            else if (!_Calculator.GetIfValueExists(BillsInAtm, viewModel.ValueRequested))
            {
                ModelState.AddModelError(string.Empty, "The Atm dosn't contain that value");
                return View(viewModel);
            }
            else
            {
                var result = _Calculator.CalculateBillsToWithdraw(BillsInAtm, viewModel.ValueRequested);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "The Atm don't have bills that add up to that sum");
                    return View(viewModel);
                }
                else
                {
                    var savedbillsinatm = _BillRepository.GetBillsInAtm();
                    _BillRepository.WithdrawBills(result,savedbillsinatm);
                    viewModel.BillsGivenOut = _BillRepository.GetGivenOutBills();                 
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
