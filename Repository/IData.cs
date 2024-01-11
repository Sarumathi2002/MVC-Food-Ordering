using Foodordering.Models;
using System.Security.Claims;

namespace Foodordering.Repository
{
    public interface IData
    {
       Task<ApplicationUser> GetUser(ClaimsPrincipal claims); 
    }
}