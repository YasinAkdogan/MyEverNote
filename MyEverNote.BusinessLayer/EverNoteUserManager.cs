using MyEverNote.Common.Helpers;
using MyEverNote.DataAccessLayer.EntityFramework;
using MyEverNote.Entities;
using MyEverNote.Entities.Messages;
using MyEverNote.Entities.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.BusinessLayer
{
    public class EverNoteUserManager
    {
        private Repository<EverNoteUser> repo_user = new Repository<EverNoteUser>();
        public BusinessLayerResult<EverNoteUser>RegisterUser(RegisterViewModel data)
        {
            EverNoteUser user = repo_user.Find(x => x.UserName == data.UserName || x.Email == data.Email);
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();
            if (user != null)
            {
                if (user.UserName==data.UserName)
                {
                    res.AddError(ErrorMessageCode.UserNameAlreadyExist, "Kullanıcı adı kayıtlı.");
                }
                if (user.Email==data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "E-Posta Kayıtlı");
                }

            }
            else
            {
                int dbResult = repo_user.Insert(new EverNoteUser()
                {
                    UserName=data.UserName,
                    Email=data.Email,
                    Password=data.Password,
                    ActivateGuid=Guid.NewGuid(),
                    IsActive=false,
                    IsAdmin=false,
                    ProfilImageFileName="userimg.jfif"
                    
                });
                if (dbResult>0)
                {
                    res.Result = repo_user.Find(x => x.Email == data.Email && x.UserName == data.UserName);
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.UserName};\n Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız.</a>";
                    MailHelper.SendMail(body, res.Result.Email, "MyEverNote Hesap Aktifleştirme");
                }
            }
            return res;
        }
 
        public BusinessLayerResult<EverNoteUser> LoginUser(LoginViewModel data)
        {
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();
            res.Result = repo_user.Find(x => x.UserName == data.UserName && x.Password == data.Password);
            if (res.Result!=null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen E-Posta adresinizi kontrol ediniz.");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserNameOrPassWrong, "Kullanıcı adı veya şifre uyuşmuyor.");
            }
            return res;
        }
        public BusinessLayerResult<EverNoteUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();
            res.Result = repo_user.Find(x => x.ActivateGuid == activateId);
            if (res.Result!=null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiş.");
                    return res;
                }
                res.Result.IsActive = true;
                repo_user.Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Aktivasyon kodu hatalı.");
            }
            return res;
        }

        public BusinessLayerResult<EverNoteUser> GetUserById(int id)
        {
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();
            res.Result = repo_user.Find(x => x.Id == id);
            if (res.Result==null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı.");
            }
            return res;
        }

        public BusinessLayerResult<EverNoteUser> UpdateProfile(EverNoteUser user)
        {
            EverNoteUser db_user = repo_user.Find(x => x.Id != user.Id && (x.UserName == user.UserName || x.Email == user.Email));
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();
            if (db_user!=null && db_user.Id!=user.Id)
            {
                if (db_user.UserName==user.UserName)
                {
                    res.AddError(ErrorMessageCode.UserNameAlreadyExist,"Bu kullanıcı daha önce oluşturulmuş.");
                }
                if (db_user.Email==user.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist,"Bu e-posta daha önce oluşturulmuş.");
                }
                return res;
            }
            res.Result = repo_user.Find(x => x.Id == user.Id);
            res.Result.Email = user.Email;
            res.Result.Name = user.Name;
            res.Result.SurName = user.SurName;
            res.Result.Password = user.Password;
            res.Result.UserName = user.UserName;
            if (string.IsNullOrEmpty(user.ProfilImageFileName)==false)
            {
                res.Result.ProfilImageFileName = user.ProfilImageFileName;
            }
            if (repo_user.Update(res.Result)==0)
            {
                res.AddError(ErrorMessageCode.ProfilCouldNotUpdate, "Profil Güncellenemedi.");
            }
            return res;
        }
    }
}
