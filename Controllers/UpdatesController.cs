using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using FYPProject.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using Microsoft.Ajax.Utilities;
namespace FYPProject.Controllers
{
    public class UpdatesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Updates update = new Updates();
        // GET: Updates
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PostUpdates()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostUpdates(Updates model)
        {
            if (!ModelState.IsValid)
            {
                var campaign = db.Campaigns.Find(model.CampaignId);
                if (campaign.CampaignId != 0)
                {
                    return RedirectToAction("ViewCampaign", "Campaign", new { id = campaign.CampaignId });
                }
                return RedirectToAction("ViewCampaign", "Campaign", new { id = model.CampaignId });
            }

            model.OwnerId = User.Identity.GetUserId();
            var result = update.PostUpdates(model);
            if (result != 0)
            {
                Campaign campaign = db.Campaigns.Find(result);
                if (campaign.CampaignId == 0)
                {
                    return RedirectToAction("ViewCampaign", "Campaign", new { id = result });
                }
                else
                {
                    return RedirectToAction("ViewCampaign", "Campaign", new { id = campaign.CampaignId });
                }
            }
            return RedirectToAction("ViewCampaign", "Campaign", new { id = model.CampaignId });
        }
    }
}