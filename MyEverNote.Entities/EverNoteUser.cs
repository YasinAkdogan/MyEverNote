using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.Entities
{
    [Table("EverNoteUsers")]
    public class EverNoteUser:BaseEntity
    {
        [StringLength(25),DisplayName("Ad")]
        public String Name { get; set; }
        [StringLength(25), DisplayName("Soyad")]
        public String SurName { get; set; }
        [StringLength(25),Required, DisplayName("KullanıcıAdı")]
        public String UserName { get; set; }
        [StringLength(150),Required, DisplayName("E-Posta")]
        public String Email { get; set; }
        [DisplayName("Şifre")]
        public string Password { get; set; }
        [StringLength(30)]//user12.jpg
        public string ProfilImageFileName { get; set; }
        [DisplayName("Aktif mi ?")]
        public bool IsActive { get; set; }
        [DisplayName("Yönetici mi ?")]
        public bool IsAdmin { get; set; }
        [DisplayName("Benzersiz Id")]
        public Guid ActivateGuid { get; set; }
        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }

    }
}
