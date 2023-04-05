using Mag.Models;

namespace Mag.Interfaces
{
    public interface IUserService
    {
        public string? GetUserName();
        public string? GetEmail();
        public bool IsAuthorized();
        public Task<AspNetUser?> CurrentUser();
        public Task<int> BasketItems();
    }
}
