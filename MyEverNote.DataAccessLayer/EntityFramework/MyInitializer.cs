using FakeData;
using MyEverNote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.DataAccessLayer.EntityFramework
{
    public class MyInitializer:CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            //adding admin user
            EverNoteUser admin = new EverNoteUser()
            {
                Name = "Yasin",
                SurName = "Akdoğan",
                Email = "deneme@deneme.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                UserName = "YasinA",
                Password = "12345",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUserName = "YasinA",
                ProfilImageFileName="userimg.jfif"
            };
            //adding standart user
            EverNoteUser standartUser = new EverNoteUser()
            {
                Name = "Ahmet",
                SurName = "Korkmaz",
                Email = "ahmetkorkmaz@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                UserName = "AhmetK",
                Password = "54321",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUserName = "YasinA",
                ProfilImageFileName = "userimg.jfif"
            };
            context.EverNoteUsers.Add(admin);
            context.EverNoteUsers.Add(standartUser);

            //adding fake user
            for (int i = 0; i < 8; i++)
            {
                EverNoteUser fakeUser = new EverNoteUser()
                {
                    Name = NameData.GetFirstName(),
                    SurName = NameData.GetSurname(),
                    Email = NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = true,
                    UserName = $"user{i}",
                    Password = "123",
                    CreatedOn = DateTimeData.GetDatetime(DateTime.Now.AddYears(-2),DateTime.Now),
                    ModifiedOn = DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now),
                    ModifiedUserName = $"user{i}",
                    ProfilImageFileName = "userimg.jfif"
                };
             context.EverNoteUsers.Add(fakeUser);
            }
             context.SaveChanges();

            //User List For Using
            List<EverNoteUser> userList = context.EverNoteUsers.ToList();

            //adding fake Categories
            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title=PlaceData.GetStreetName(),
                    Description=PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn=DateTime.Now,
                    ModifiedUserName="YasinA"
                };
                    context.Categories.Add(cat);
                 //adding fake notes
                for (int k = 0; k < NumberData.GetNumber(5,9); k++)
                {
                    EverNoteUser owner = userList[NumberData.GetNumber(0, userList.Count - 1)];
                    Note note = new Note()
                    {
                        Title=TextData.GetAlphabetical(NumberData.GetNumber(5,25)),
                        Text= TextData.GetSentences(NumberData.GetNumber(1,3)),
                        Category=cat,
                        IsDraft=false,
                        LikeCount=NumberData.GetNumber(1,9),
                        Owner=owner,
                        CreatedOn = DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now),
                        ModifiedOn = DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now),
                        ModifiedUserName=owner.UserName
                    };
                    cat.Notes.Add(note);
                    //adding fake comment
                    for (int j = 0; j < NumberData.GetNumber(3,5); j++)
                    {
                        EverNoteUser comment_owner = userList[NumberData.GetNumber(0, userList.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text=TextData.GetSentence(),
                            Owner=comment_owner,
                            CreatedOn= DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now),
                            ModifiedOn= DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now),
                            ModifiedUserName=comment_owner.UserName,
                            //Note=note
                        };
                        note.Comments.Add(comment);
                        //adding fake Likes
                        for (int m = 0; m < note.LikeCount; m++)
                        {
                            Liked liked = new Liked()
                            {
                                LikedUser = userList[m]
                            };
                            note.Likes.Add(liked);
                        }
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
