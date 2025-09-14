using System;
using System.Threading.Tasks;
using SoapClient.ServiceReference1;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var client = new UserServiceClient(
            UserServiceClient.EndpointConfiguration.BasicHttpBinding_IUserService,
            "http://localhost:5095/UserService.asmx" 
        );

        var user = new User
        {
            FirstName = "Jan",
            LastName = "Kowalski",
            EmailAddress = "jan.kowalski@wsei.edu.pl",
            Age = 25,
            MarketingConsent = true
        };

        var result = await client.RegisterUserAsync(user);
        Console.WriteLine(result);

        try { await client.CloseAsync(); } catch { client.Abort(); }
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
