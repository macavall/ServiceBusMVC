using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceBusMVC.Models;
using System.Threading.Tasks;

namespace ServiceBusMVC.Controllers
{
    public class MsgCounts
    {
        public string? MessageCount { get; set; }
        public string? ScheduledMessageCount { get; set; }
    }

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
                MessageCount = await _sbService.GetQueueMsgCount("sbqueueonlybatchnopart")
            };

            //return View();
            return View(MsgModel);
        }

        [HttpGet]
        public async Task PopulateSB(string id)
        {
            await _sbService.PopulateSB(id);
        }

        public async Task<string> GetMessageCounts(string id)
        {
            var msgCounts = new MsgCounts()
            {
                MessageCount = (await _sbService.GetQueueMsgCount(id)).ToString(),
                ScheduledMessageCount = (await _sbService.GetQueueSchMsgCount(id)).ToString()
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(msgCounts);
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
