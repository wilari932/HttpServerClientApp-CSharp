using Flurl.Http;

try
{

	Console.WriteLine("Presione ENTER para eniviar una peticion");
	
	// URL del servidor simple
	string serverUrl = $"http://localhost:7082/{Console.ReadLine()??""}";

	// Realizar una solicitud GET al servidor y recibir la respuesta como texto
	var response = await serverUrl.GetAsync();

	// Imprimir la respuesta en la consola
	Console.WriteLine($"Respuesta del servidor: {await response.GetStringAsync()}");
	Console.WriteLine($"Respuesta del servidor: { string.Join(",",response.Cookies.Select(x=> x.Name))}");
}
catch (Exception ex)
{
	Console.WriteLine($"Error al comunicarse con el servidor: {ex.Message}");
}

Console.WriteLine("Presione ENTER para cerrar la aplicación.");
Console.ReadLine();