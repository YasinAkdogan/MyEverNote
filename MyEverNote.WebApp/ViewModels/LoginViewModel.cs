using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEverNote.WebApp.ViewModels
{
    public class LoginViewModel
    {
        [DisplayName("Kullanıcı Adı"),Required(ErrorMessage ="{0} alanı boş geçilemez."),StringLength(25,ErrorMessage ="{0} max {1} karakter olmalı")]//0 alanından dediğimiz username.boş bıraktığımız zaman kullanıcı adı boş geçilemez yazacak.25 den fazla karakter giremezsin{1} ise 25 e denk geliyor.
        public string UserName { get; set; }
        [DisplayName("Şifre"),Required(ErrorMessage = "{0} alanı boş geçilemez."),DataType(DataType.Password),StringLength(25, ErrorMessage = "{0} max {1} karakter ] olmalı")]//datatype kullandık çünkü bunun bir password tipi olduğunu tanıtmak için.
        public string Password { get; set; }

    }
}