using SoapService.DataContract;

namespace SoapService.ServiceContract;

public class UserService : IUserService
{
    public string RegisterUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user?.EmailAddress)) return "Cannot register user.";
        return $"User {user.EmailAddress} registered!";
    }
}
