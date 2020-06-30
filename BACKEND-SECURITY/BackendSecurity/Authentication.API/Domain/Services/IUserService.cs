using Authentication.API.Domain.Models;
using Authentication.API.Domain.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.API.Domain.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest body);
        IEnumerable<User> GetAll();

    }
}
