using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IntrestRatesProvider.Models;
using ViliborService1;
using System.ServiceModel;
using Microsoft.Data.Sqlite;


namespace IntrestRatesProvider.Controllers
{

    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public double GetRate(double BaseRate, double Margin)
        {
            return BaseRate + Margin;
        }

        private List<Clients> Client { get; set; }
        private List<Agreements> Agreements;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculate(string ClientID, string NewRate)
        {
            VilibidViliborSoapClient service = new VilibidViliborSoapClient(new BasicHttpsBinding(BasicHttpsSecurityMode.Transport), new EndpointAddress("https://www.lb.lt/webservices/VilibidVilibor/VilibidVilibor.asmx?WSDL"));
            var msg = service.getLatestVilibRateAsync(NewRate);
            const string filename = @"C:\IISProjects\IntrestRatesProvider\web.db";
            var conn = new SqliteConnection("Data Source=" + filename);
            try
            {
                conn.Open();
                var listofclients = new List<Clients>();
                string query = "select * from Customer where PersonalID =" + ClientID;
                SqliteCommand sqlite_cmd = new SqliteCommand(query, conn);
                SqliteDataReader dr = sqlite_cmd.ExecuteReader();
                if (!dr.HasRows)
                {
                    dr.Close();
                    conn.Close();
                    return Error();
                }
                    while (dr.Read())
                    {
                        listofclients.Add(new Clients
                        {
                            Name = dr["Name"].ToString(),
                            Surname = dr["Surname"].ToString(),
                            PersonalID = dr["PersonalID"].ToString()
                        });
                    }
                    dr.Close();
                    Client = listofclients;

                    var listofagreements = new List<Agreements>();
                    query = "select * from Agreement a, Customer c where a.CustomerID = c.ID and c.PersonalID =" + ClientID;
                    sqlite_cmd = new SqliteCommand(query, conn);
                    dr = sqlite_cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string BaseRateCodeOld = dr["Base"].ToString();
                        double MarginCurrent = Convert.ToDouble(dr["Margin"]);
                        var msg2 = service.getLatestVilibRateAsync(BaseRateCodeOld);
                        double CurrentInterest = GetRate(Convert.ToDouble(msg2.Result), MarginCurrent);
                        double NewInterest = GetRate(Convert.ToDouble(msg.Result), MarginCurrent);
                        double Diff = CurrentInterest - NewInterest;
                        listofagreements.Add(new Agreements
                        {
                            Amount = Convert.ToInt32(dr["Amount"]),
                            BaseRateCode = BaseRateCodeOld,
                            Duration = Convert.ToInt32(dr["Duration"]),
                            Margin = MarginCurrent,
                            CurrentInterestRate = CurrentInterest,
                            NewInterestRate = NewInterest,
                            Difference = Math.Round(Diff, 2)
                        });

                    }
                    Agreements = listofagreements;
                    dr.Close();
                    conn.Close();
            }
            catch (Exception)
            {
                return Error();
            }
            ViewModel mymodel = new ViewModel();
            mymodel.Clients = Client;
            mymodel.Agreemenat = Agreements;
            return View(mymodel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Application helps to evaluate how the change of base rate will impact the interest rate. User submits person ID and new base rate code and application calculates current and new interest rate based on specified base rate";
            return View();
        }

        public IActionResult Error()
        {
            TempData["testmsg"] = "Incorrect person ID";
            return RedirectToAction("Index");
        }
    }
}
