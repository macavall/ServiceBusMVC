using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceBusMVC.Models;
using System.Threading.Tasks;

namespace ServiceBusMVC.Controllers
{
    public class MsgCounts
    {
        public string? MsgCount { get; set; }
        public string? SchMsgCount { get; set; }

    public class TestController : Controller
    {
        private readonly ISbService _sbService;
        public TestController(ISbService sbService)
        {
            _sbService = sbService;
        }

        // GET: TestController
        public async Task<ActionResult> Index()
        {
            var MsgModel = new SbModel()
            {
                MessageCount = await _sbService.GetQueueMsgCount("myqueue")
            };

            return View(MsgModel);
        }

        public async Task<string> GetMessageCounts()
        {


            return "";
        }

        // GET: TestController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TestController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: TestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TestController/Edit/5
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

        // GET: TestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TestController/Delete/5
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
