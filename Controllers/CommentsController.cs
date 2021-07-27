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
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Comments comment = new Comments();
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        public ActionResult PostComment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostComment(CommentViewModel model)
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
            var result = comment.PostComment(model);
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