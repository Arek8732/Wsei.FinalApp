using System.Runtime.Serialization;

namespace SoapService.DataContract;

[DataContract]
public class User
{
    [DataMember] public string EmailAddress { get; set; } = default!;
    [DataMember] public string FirstName { get; set; } = default!;
    [DataMember] public string LastName  { get; set; } = default!;
    [DataMember] public int Age          { get; set; }
    [DataMember] public bool MarketingConsent { get; set; }
}
