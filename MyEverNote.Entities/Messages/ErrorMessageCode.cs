using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.Entities.Messages
{
    public enum ErrorMessageCode
    {
        UserNameAlreadyExist=101,
        EmailAlreadyExist=102,
        UserIsNotActive=151,
        UserNameOrPassWrong=152,
        CheckYourEmail=153,
        UserAlreadyActive = 154,
        ActivateIdDoesNotExist = 155,
        UserNotFound = 156,
        ProfilCouldNotUpdate = 157
    }
}
