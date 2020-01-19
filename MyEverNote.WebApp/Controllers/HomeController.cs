using MyEverNote.BusinessLayer;
using MyEverNote.Entities;
using MyEverNote.Entities.Messages;
using MyEverNote.Entities.ValueObject;
using MyEverNote.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEverNote.WebApp.Controllers
{
    public class HomeController : Controller
    {
        NotManager nm = new NotManager();
        // GET: Home
        public ActionResult Index()
        {
           
            return View(nm.GetAllNote().OrderByDescending(x => x.ModifiedOn).ToList());
        }
        public ActionResult ByCategory(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryManager cm = new CategoryManager();
            Category cat = cm.GetCategoryById(id.Value);
            if (cat==null)
            {
                return HttpNotFound();
            }
            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }
        public ActionResult MostLiked()
        {
            return View("Index", nm.GetAllNote().OrderByDescending(x => x.LikeCount).ToList());
        }
        public ActionResult About()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                EverNoteUserManager eum = new EverNoteUserManager();
                BusinessLayerResult<EverNoteUser> res = eum.LoginUser(model);
                if (res.Errors.Count>0)
                {
                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/1234-4567-789080";
                    }
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                Session["Login"] = res.Result;
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Register()
        {
           return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                EverNoteUserManager eum = new EverNoteUserManager();
                BusinessLayerResult<EverNoteUser> res = eum.RegisterUser(model);
                if (res.Errors.Count>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
               
                OkViewModel notifyObjOk = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login"
                };
                notifyObjOk.Items.Add(" Lütfen E-Posta hesabına gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz. Hesabınızı aktive etmeden not ekleyemez ve beğeni yapamazsınız.!!!");
                return View("Ok", notifyObjOk);

                
            }

            return View(model);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult RegisterOk()
        {
            return View();
        }
        public ActionResult UserActivate(Guid id)
        {
            EverNoteUserManager eum = new EverNoteUserManager();
            BusinessLayerResult<EverNoteUser> res = eum.ActivateUser(id);
            if (res.Errors.Count>0)
            {
                TempData["errors"] = res.Errors;
                return RedirectToAction("UserActivateCancel");
            }
            return RedirectToAction("UserActivateOk");
        }
        public ActionResult UserActivateOk()
        {
            return View();
        }
        public ActionResult UserActivateCancel()
        {
            List<ErrorMessageObj> errors = null;
            if (TempData["errors"]!=null)
            {
                errors = TempData["errors"] as List<ErrorMessageObj>;
            }
            return View(errors);
        }
        public ActionResult ShowProfile()
        {
            EverNoteUser currentUser = Session["login"] as EverNoteUser;
            EverNoteUserManager eum = new EverNoteUserManager();
            BusinessLayerResult<EverNoteUser> res = eum.GetUserById(currentUser.Id);
            if (res.Errors.Count>0)
            {
                //kullanıcıyı hata ekranına gönder.
            }
            return View(res.Result);
        }
        public ActionResult EditProfile()//bununla sayfa oluşuyor.
        {
            EverNoteUser currentUser = Session["login"] as EverNoteUser;//loginin içinden bütün bilgileri evernoteuser'a çevirerek currentuser'a atacak.
            EverNoteUserManager eum = new EverNoteUserManager();
            BusinessLayerResult<EverNoteUser> res = eum.GetUserById(currentUser.Id);
            if (res.Errors.Count>0)
            {
                
            }
            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditProfile(EverNoteUser user,HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                if (ProfileImage!=null && (ProfileImage.ContentType=="image/jpeg" || ProfileImage.ContentType=="image/jpg" ||ProfileImage.ContentType=="image/png"))
                {
                    string fileName = $"user_{user.Id}.{ProfileImage.ContentType.Split('/')[1]}"; 
                    ProfileImage.SaveAs(Server.MapPath($"~/Images/{fileName}"));
                    user.ProfilImageFileName = fileName;
                }
                EverNoteUserManager eum = new EverNoteUserManager();
                BusinessLayerResult<EverNoteUser> res = eum.UpdateProfile(user);
                if (res.Errors.Count>0)
                {
                    
                }
                Session["login"] = res.Result;
                return RedirectToAction("ShowProfile");
            }
            return View(user);
        }
        public ActionResult RemoveProfile()
        {
            return View();
        }
    }
}