using System.ServiceModel;
using SoapService.DataContract;

namespace SoapService.ServiceContract;

[ServiceContract]
public interface IUserService
{
    [OperationContract]
    string RegisterUser(User user);
}
