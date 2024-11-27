using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebRencanaProduksi.Data;
using WebRencanaProduksi.Models;

namespace WebRencanaProduksi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebContext _webContext;

        public HomeController(ILogger<HomeController> logger, WebContext webContext)
        {
            _logger = logger;
            _webContext = webContext;
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

        //TASK I
        [HttpPost]
        public IActionResult RencanaProduksi([FromBody] List<int> list)
        {
            try
            {
                int total = list.Sum();
                int count = list.Count;

                int average = total / count;
                int remain = total % count;

                List<int> result = new List<int>(new int[count]);

                for (int i = 0; i < count; i++)
                {
                    result[i] = average;
                }

                for (int i = 0; i < remain; i++)
                {
                    int hariMax = list.IndexOf(list.Max());
                    result[hariMax]++;
                    list[hariMax] = -1;
                }

                return Json(new { status = true, Data = result });
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //TASK II
        public IActionResult Task2()
        {
            return View();
        }

        private string getDay(int Index)
        {
            string[] Days = { "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu", "Minggu" };
            return Days[Index];
        }

        [HttpPost]
        public IActionResult RencanaProduksiTask2([FromBody] List<int> list)
        {
            try
            {
                List<int> tempData = new List<int>(list);

                int total = 0;
                int countAll = list.Count;
                int countOn = 0;

                foreach(int item in list)
                {
                    if(item > 0)
                    {
                        total += item;
                        countOn++;
                    }
                }

                int average = total / countOn;
                int remain = total % countOn;

                List<int> result = new List<int>(new int[countAll]);

                int dayOFF = list.IndexOf((int)0);
                List<int> dayOffIndex = new List<int>();

                for (int i = 0; i < countAll; i++)
                {
                    if (list[i] == 0)
                    {
                        dayOffIndex.Add(i);
                    } else
                    {
                        result[i] = average;
                    }
                }

                for (int i = 0; i < remain; i++)
                {
                    int hariMax = tempData.IndexOf(tempData.Max());
                    result[hariMax]++;
                    tempData[hariMax] = -1;
                }

                //SAVE DATA TO DATABASE
                SaveToDatabase(list, result);

                return Json(new { status = true, Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void SaveToDatabase(List<int> plan, List<int> result)
        {

            var planRecords = new List<RencanaProduksi>();

            //save plan
            for(int i = 0;i < plan.Count;i++)
            {
                var PlanRecord = new RencanaProduksi
                {
                    Hari = getDay(i),
                    Produksi = plan[i],
                };
                _webContext.RencanaProduksi.Add(PlanRecord);
                planRecords.Add(PlanRecord);
            };

            _webContext.SaveChanges();

            //save log
            for(int i = 0; i < result.Count;i++)
            {
                var resultRecord = new LogEfisiensi
                {
                    IdRencanaProduksi = planRecords[i].Id,
                    Hari = getDay(i),
                    Produksi = result[i],
                };
                _webContext.LogEfisiensi.Add(resultRecord);
            }

            _webContext.SaveChanges();

        }
    }
}
