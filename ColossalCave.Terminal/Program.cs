Console.WriteLine("Welcome to Colossal Cave Adventure!");

var httpClient = new HttpClient();
//httpClient.BaseAddress = new Uri("http://localhost:5168/api/"); // http host
httpClient.BaseAddress = new Uri("http://localhost:39328/api/"); // iisexpress host

var response = await httpClient.GetAsync("heartbeat");
Console.WriteLine(await response.Content.ReadAsStringAsync());

while (true)
{
    Console.Write("> ");
    var command = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(command))
        break;

    var content = new StringContent(command);
    response = await httpClient.PostAsync("command", content);

    if (response.IsSuccessStatusCode)
    {
        var result = await response.Content.ReadAsStringAsync();
        Console.WriteLine(result);
    }
    else
    {
        Console.WriteLine("Error: Unable to process command.");
    }
}