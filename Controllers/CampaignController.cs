using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYPProject.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using AutoMapper;
using System.Net;
using Stripe;
using Stripe.Checkout;
using Newtonsoft.Json;

namespace FYPProject.Controllers
{
    public class CampaignController : CommonController
    {


        private ApplicationDbContext db = new ApplicationDbContext();
        private Campaign campaign = new Campaign();




        // GET: Campaign
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NoStripeConnected()
        {
            return View();
        }

        // GET: Campaign/CreateCampaign
        [Authorize]
        public ActionResult CreateCampaign()
        {
            var userId = User.Identity.GetUserId();
            var usertoken = db.Users.Where(a => a.Id == userId).FirstOrDefault();
            string userstripetoken = usertoken.StripeToken;
            if (userstripetoken != null)
            {
                CreateCampaignViewModel model = new CreateCampaignViewModel
                {
                    OwnerId = userId,
                    OwnerUsername = this.HttpContext.User.Identity.Name,
                    StartDate = DateTime.Now,
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("NoStripeConnected", "Campaign");
            }
        }

        // GET: Campaign/CreateCampaign
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCampaign(CreateCampaignViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = campaign.Create(model);
                if (result != 0)
                {
                    return RedirectToAction("ViewCampaign", new { id = result });
                }
                else
                {
                    return View(model);
                }
            }
            return View(model);
        }



        public ActionResult ViewAllCampaign(string search,string filter)
        {
            var posts = campaign.RetrieveCampaigns(search, filter);
            return View(posts);
        }

        public ActionResult ViewClosedCampaign(string search, string filter)
        {
            var posts = campaign.RetrieveCampaigns(search, filter);
            return View(posts);
        }


        [HttpGet]
        public ActionResult ViewCampaign(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campaign campaign = db.Campaigns.Find(id);
            if (campaign == null)
            {
                return HttpNotFound();
            }

            if (TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }
            DetailsViewModel model = campaign.GetDetails(campaign.CampaignId);
            return View(model);
        }




        public ActionResult ConnectSuccess()
        {
            return View();
        }

        public ActionResult ConnectFailed()
        {
            return View();
        }

        public ActionResult ConnectStripe(string code)
        {
            StripeConfiguration.ApiKey = "sk_test_51IBDZaCi79uOWe3Su0pY4cxDOy7fm58gQnAoRXGfZAoDRUW2x2ozcsvnmulkY4w0IpXeRgeYSggnGccxGflhhxxC00g16sAP52";


            if (code != null)
            {
                var options = new OAuthTokenCreateOptions
                {
                    GrantType = "authorization_code",
                    Code = code,
                };

                var service = new OAuthTokenService();
                var response = service.Create(options);

                // Access the connected account id in the response
                var userid = User.Identity.GetUserId();
                //store this one
                string connected_account_id = response.StripeUserId;

                var AcctUpdateOptions = new AccountUpdateOptions
                {
                    Settings = new AccountSettingsOptions
                    {
                        Payouts = new AccountSettingsPayoutsOptions
                        {
                            Schedule = new AccountSettingsPayoutsScheduleOptions
                            {
                                Interval = "manual",
                            },
                        },
                    },
                };
                var AcctService = new AccountService();
                var account = AcctService.Update(connected_account_id, AcctUpdateOptions);


                var temptoken = db.ApplicationUsers.Any(x => x.StripeToken.Equals(connected_account_id));
                var check = db.ApplicationUsers.Where(x => x.Id == userid && x.StripeToken == null).FirstOrDefault();

                if (temptoken)
                {
                    return RedirectToAction("ConnectFailed", "Campaign");
                }
                else
                {
                    if (check != null)
                    {
                        check.StripeToken = connected_account_id;
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("ConnectSuccess", "Campaign");
        }

        [HttpGet]
        [Authorize]
        public ActionResult DonateCampaign (int? id)
        {
            Campaign campaign = db.Campaigns.Find(id);
            return View(campaign);
        }

        public class CreateCheckoutSessionRequest
        {
            [JsonProperty("quantity")]
            public string DonationAmount { get; set; }
            public string Title { get; set; }
            public int CampaignId { get; set; }
            public string OrganiserId { get; set; }
        }

        [HttpPost]
        public ActionResult CreateCheckoutSession([Microsoft.AspNetCore.Mvc.FromBody]CreateCheckoutSessionRequest req)
        {
            var request = Request.Url.ToString();
            var ss = "/Campaign/DonationComplete";
            var request2 = Request.Url.Authority.ToString();
            var userid = User.Identity.GetUserId();
            var fullname = db.Users.Where(x => x.Id == userid).FirstOrDefault();
            var campaign = db.Campaigns.Where(a => a.CampaignId == req.CampaignId).FirstOrDefault();
            var usertoken = db.Users.Where(a => a.Id == campaign.OwnerId).FirstOrDefault();
            //curl -X POST -is "https://localhost:44342/Campaign/CreateCheckoutSession" -d ""
            StripeConfiguration.ApiKey = "sk_test_51IBDZaCi79uOWe3Su0pY4cxDOy7fm58gQnAoRXGfZAoDRUW2x2ozcsvnmulkY4w0IpXeRgeYSggnGccxGflhhxxC00g16sAP52";
            string stripetoken = usertoken.StripeToken;
            var options = new SessionCreateOptions
            {
                SubmitType = "donate",
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>
            {
                    new SessionLineItemOptions
                    {
                        Name = req.Title,
                        Amount = int.Parse(req.DonationAmount) * 100,
                        Currency = "sgd",
                        Quantity = 1,
                        Description = "Donating to " + "Organizer name",
                        //Images = new List<string> {"https://i.redd.it/97kohvqgcxc01.jpg" },
 
                    },
                    
                },
                Metadata = new Dictionary<string, string> {
                { "CampaignId", req.CampaignId.ToString()},
                { "UserId", userid},
                { "OrganiserId", req.OrganiserId},
                { "Amount", req.DonationAmount},
                { "FullName", fullname.FullName},
                { "Anonymous", "False" }
                },
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    ApplicationFeeAmount = Convert.ToInt64(((int.Parse(req.DonationAmount) * 100) * 0.05)),
                    TransferData = new SessionPaymentIntentDataTransferDataOptions
                    {
                        Destination = stripetoken,
                    },
                },
                //SuccessUrl = "https://example.com/success?session_id={CHECKOUT_SESSION_ID}",
                SuccessUrl = "https://" + request2 + ss + "?checkoutID={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://example.com/failure",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Json(new { id = session.Id });
        }

        public ActionResult DonationComplete(string checkoutID)
        {
            StripeConfiguration.ApiKey = "sk_test_51IBDZaCi79uOWe3Su0pY4cxDOy7fm58gQnAoRXGfZAoDRUW2x2ozcsvnmulkY4w0IpXeRgeYSggnGccxGflhhxxC00g16sAP52";

            try
            {
                //correct checkout id
                //string testCheckoutID = "cs_test_a1x8kBByAQxHd8cjT4QKX18pd8GDkDTI7T3mVu89zo9aHh5ldo2R8IBnd8";
                //fake checkout id
                //string testCheckoutID = "cs_test_a1x8kBByAQxHd8cjT4QKX15558GDkDTI7T3mVu89zo9aHh5ldo2R8IBnd8";
                var sessService = new SessionService();
                var serviceCheckout = sessService.Get(checkoutID);
                var paymentIntent = serviceCheckout.PaymentIntentId;
                var currentuserid = User.Identity.GetUserId();

                var piService = new PaymentIntentService();
                var servicePaymentIntent = piService.Get(paymentIntent);
                var stripe_user_token = servicePaymentIntent.TransferData.DestinationId;

                string campaignid = serviceCheckout.Metadata["CampaignId"];
                string userid = serviceCheckout.Metadata["UserId"];
                string organiserid = serviceCheckout.Metadata["OrganiserId"];
                string amount = serviceCheckout.Metadata["Amount"];
                string fullname = serviceCheckout.Metadata["FullName"];
                string anonymous = serviceCheckout.Metadata["Anonymous"];
                string checkoutToken = checkoutID;
                int campaignid2 = int.Parse(campaignid);
                var validatetoken = db.Checkouts.Any(x => x.CheckoutToken.Equals(checkoutToken));               

                if (!validatetoken)
                {
                    Checkout checkout = new Checkout
                    {
                        CampaignId = int.Parse(campaignid),
                        UserId = userid,
                        OrganiserId = organiserid,
                        Amount = int.Parse(amount),
                        FullName = fullname,
                        Anonymous = anonymous,
                        CheckoutToken = checkoutToken
                    };
                    db.Checkouts.Add(checkout);
                    var addcountcampaign = db.Campaigns.Where(x => x.CampaignId == campaignid2).FirstOrDefault();
                    var validateduplicate = db.Checkouts.Any(x => x.CampaignId == campaignid2 && x.UserId == currentuserid);
                    System.Diagnostics.Debug.WriteLine(validateduplicate);
                    if (validateduplicate == false)
                    {
                        addcountcampaign.Donors += 1;
                        db.SaveChanges();
                    }
                    db.SaveChanges();
                    var sumofdonation = db.Checkouts.GroupBy(a => a.CampaignId).Select(a => new { amount = a.Sum(b => b.Amount), CampaignId = a.Key }).ToList();
                    foreach (var item in sumofdonation)
                    {
                        var passcampaignid = db.Campaigns.Where(x => x.CampaignId == item.CampaignId).FirstOrDefault();
                        passcampaignid.CurrentDonation = item.amount;                       
                    }
                    db.SaveChanges();                   
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            catch (StripeException e)
            {
                // Unknown Error Type (redirect to error page occured)
                System.Diagnostics.Debug.WriteLine(e);
                return RedirectToAction("Error", "Shared");

            }

            return View();
        }

        public ActionResult Payout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payout(CreateCampaignViewModel model)
        {
            try
            {

                var campaign = db.Campaigns.Where(x => x.CampaignId == model.CampaignId).FirstOrDefault();
                StripeConfiguration.ApiKey = "sk_test_51IBDZaCi79uOWe3Su0pY4cxDOy7fm58gQnAoRXGfZAoDRUW2x2ozcsvnmulkY4w0IpXeRgeYSggnGccxGflhhxxC00g16sAP52";
                var user = db.Users.Where(x => x.Id == campaign.OwnerId).FirstOrDefault();

                var options = new PayoutCreateOptions
                {
                    Amount = Convert.ToInt32((campaign.CurrentDonation * 0.95) * 100),
                    Currency = "sgd",
                };

                var requestOptions = new RequestOptions();
                requestOptions.StripeAccount = user.StripeToken;

                var service = new PayoutService();
                var payout = service.Create(options, requestOptions);

                var payoutID = payout.Id;
                var temppayout = db.Campaigns.Any(x => x.PayoutID.Equals(payoutID));

                if (temppayout)
                {
                    return RedirectToAction("ConnectFailed", "Campaign");
                }
                else
                {
                    if (payoutID != null)
                    {
                        campaign.PayoutID = payoutID;
                        db.SaveChanges();
                    }
                }



            }
            catch (StripeException e)
            {
                // Unknown Error Type (redirect to error page occured)
                System.Diagnostics.Debug.WriteLine(e);
                return RedirectToAction("Error", "Shared");

            }
            return View();
        }

    }
}
