using ConsoleApp_Chat_Streaming.Helper;
using System.Reflection;
using System.Text.Json;
using System.Text;
var _vehicleId = "2356";

string[] asciiArt = new string[]
      {
            "   _____                                                _     _              _   ",
            "  / ____|                                 /\\           (_)   | |            | |  ",
            " | |     ___ _ __ ___ _ __   ___ ___     /  \\   ___ ___ _ ___| |_ __ _ _ __ | |_ ",
            " | |    / _ \\ '__/ _ \\ '_ \\ / __/ _ \\   / /\\ \\ / __/ __| / __| __/ _` | '_ \\| __|",
            " | |___|  __/ | |  __/ | | | (_|  __/  / ____ \\\\__ \\__ \\ \\__ \\ || (_| | | | | |_ ",
            "  \\_____\\___|_|  \\___|_| |_|\\___\\___| /_/    \\_\\___/___/_|___/\\__\\__,_|_| |_|\\__|",
            "                                                                                 ",
            "                                                                                 "
      };

var headerfooter = "############################################################################################\n";            
Console.WriteLine($"VehicleId: {_vehicleId}\n");
Console.WriteLine(headerfooter);
foreach (string line in asciiArt)
{
    Console.WriteLine(line);
}
Console.WriteLine(headerfooter);

//HttpClient httpClient = new HttpClient();
//string baseAddress = "http://localhost:5203/";
//// var chatHelper = new ChatEndpointHelper(httpClient, baseAddress);

using var client = new HttpClient();
string? url = "http://localhost:5203/Chat/ChatRDCNew"; // http://localhost:5203/Chat/Test2  test post endpoint
client.DefaultRequestHeaders.Add("vehicleId", "123456");

while (true)
{
    Console.Write("User: ");
    var key = Console.ReadKey(intercept: true);

    if (key.Key == ConsoleKey.Escape)
    {
        Console.WriteLine("\nEscape key pressed. Exiting...");
        break;
    }

    string? userMessage = Console.ReadLine();

    var payload = new { prompt = userMessage };
    var jsonContent = JsonSerializer.Serialize(payload);
    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

    // Call our endpoint here with userMessage
    try
    {
        using var response = await client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);
        Console.Write($"Assistant: ");

        char[] buffer = new char[2010]; // Increased buffer size for efficiency
        int charsRead;

        while ((charsRead = await reader.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            // Instead of writing the whole buffer at once, write character by character with a delay
            for (int i = 0; i < charsRead; i++)
            {
                Console.Write(buffer[i]);
                await Task.Delay(50); // Adjust the delay to control typing speed
            }
        }
        Console.WriteLine("\n");
    }
    catch (HttpRequestException e)
    {
        Console.WriteLine($"Error: {e.Message}");
    }
}