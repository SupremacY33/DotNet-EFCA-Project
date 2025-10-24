using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface IAuthServices
    {
        Task<string?> AuthenticationAsync(LoginDTO loginDTO);
    }
}
