using ConaviWeb.Model.Response;
using System.Threading.Tasks;

namespace ConaviWeb.Tools
{
    public interface ISecurityTools
    {
        string GetSHA256(string str);
        Task<string> GetToken(UserResponse user);
        int GetUserFromAccessToken(string accessToken);
    }
}
