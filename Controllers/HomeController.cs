using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MujHelpdesk.Data;
using MujHelpdesk.Models;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace MujHelpdesk.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly MujHelpdeskContext _context;
		public HomeController(ILogger<HomeController> logger, MujHelpdeskContext context)
		{
			_logger = logger;
			_context = context;
		}

        private static List<Request> reqList = new();

        public IActionResult List()
		{
			
			reqList = _context.Requests.Where(a => a.Status == 0).ToList();
			//reqList = _context.Requests.ToList();

			return View(reqList);
        }

		public IActionResult CloseReq(int id)
		{
			Request req = _context.Requests.Single(x => x.Id == id);
			req.Status = 1;
			_context.SaveChanges();

			return RedirectToAction(nameof(List));
		}
		

		public IActionResult CreateForm()
        {
			var newReq = new Request();
			return View(newReq);
			//return View();
		}

		public byte[] picToByte(IFormFile postedFile)
		{

			if (postedFile.Length > 0)
			{
				using (var ms = new MemoryStream())
				{
					postedFile.CopyTo(ms);
					return ms.ToArray();
					//string s = Convert.ToBase64String(fileBytes);
					// act on the Base64 data
				}
			}
			else
			{
				return null;
			}
		}

		[HttpPost]
		public IActionResult RequestCreate(Request req, List<IFormFile> imgFiles)
		{

			if (ModelState.IsValid)
			{

				foreach (IFormFile postedFile in imgFiles)
				{
					req.Img = picToByte(postedFile);
				}


				DateTime dt = DateTime.Now;

				req.Status = 0;
				req.OrigDate = dt;

				_context.Requests.Add(req);
				_context.SaveChanges();

				return RedirectToAction(nameof(List));
			}
			return View("CreateForm",  req);
			//return RedirectToAction(nameof(CreateForm));
		}

		public IActionResult CommentCreate(string UserName, string Text, int rId)
		{

			Comment com = new();
			com.UserName = UserName;
			com.Text = Text;
			com.ReqId= rId;

			DateTime dt = DateTime.Now;

			com.PostDate = dt;

			_context.Comments.Add(com);
			_context.SaveChanges();

			//return Item(rId);

			return RedirectToAction("Item", new { reqId = rId });
			//nameof(
		}


		public IActionResult Item(int reqId)
        {

			//Request req = _context.Requests.Where(x => x.Status == reqId).ToList();
			Request req = _context.Requests.Single(x => x.Id == reqId);
			List<Comment> comList = new();
			comList = _context.Comments.Where(a => a.ReqId == reqId ).ToList();

			ReqComms rc = new()
			{
				ComList = comList,
				Req = req
			};


			return View(rc);
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
	}
}