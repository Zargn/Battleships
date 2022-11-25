using Battleships.objects.networking;
using BattleshipsFwServer;
using ForwardingServer;

public class Server
{
    public static async Task Main()
    {
        var server = new Server();
        await server.RunServer();

        Console.WriteLine("Server closed.");
        Console.ReadLine();
    }

    private async Task RunServer()
    {
        var identification = new OnlineUserIdentification("ForwardingServer");
        var identificationPackage = new OnlineUserIdentificationPackage(identification);
        var jsonSerializer = new JsonSerializerAdapter();
        var logger = new Logger();
        var serverClient = new FwServer<OnlineUserIdentification>(25564, logger, jsonSerializer, identificationPackage);

        var serverTask = serverClient.Run();

        while (true)
        {
            Console.WriteLine("Write Q to quit.");
            var result = Console.ReadLine();
            if (result.ToLower() != "q") continue;
            Console.WriteLine("Quitting...");
            await serverClient.Stop();
            return;
        }
    }
}