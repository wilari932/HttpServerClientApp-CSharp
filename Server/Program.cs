using System.Net;
using System.Text;

void ReturnHtml(HttpListenerContext context)
{

	byte[] responseBytes = File.ReadAllBytes("www/index.html");

	// Establecer el tipo de contenido y la longitud del contenido en la respuesta
	context.Response.ContentType = "text/html; charset=utf-8";
	context.Response.ContentLength64 = responseBytes.Length;
	context.Response.StatusCode = (int)HttpStatusCode.OK;
	context.Response.Cookies.Add(new Cookie("wilmar", "whats up"));
	// Escribir la respuesta en el flujo de salida y cerrar la conexión
	Stream outputStream = context.Response.OutputStream;
	outputStream.Write(responseBytes, 0, responseBytes.Length);
	outputStream.Close();
}

if (!HttpListener.IsSupported)
{
	Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
	return;
}

// Crear un HttpListener y configurar las direcciones a escuchar
HttpListener listener = new HttpListener();
listener.Prefixes.Add("http://localhost:7082/");


// Iniciar el HttpListener
listener.Start();

Console.WriteLine("Servidor web en ejecución en http://localhost:7082/. Presione ENTER para salir.");

CancellationTokenSource cts = new CancellationTokenSource();

// Ejecutar el bucle principal del servidor en un hilo separado
Task.Run(() =>
{
	try
	{
		while (!cts.Token.IsCancellationRequested)
		{
			// Esperar una solicitud entrante
			HttpListenerContext context = listener.GetContext();

			Console.WriteLine(context.Request.Url.AbsolutePath);
			if (context.Request.Url.AbsolutePath.ToLower().Equals("/index.html"))
			{
				ReturnHtml(context);
				continue;
			}

			// Procesar la solicitud y generar una respuesta
			string responseText = "Hola, mundo";
			byte[] responseBytes = Encoding.UTF8.GetBytes(responseText);

			// Establecer el tipo de contenido y la longitud del contenido en la respuesta
			context.Response.ContentType = "text/plain";
			context.Response.ContentLength64 = responseBytes.Length;
			context.Response.StatusCode = (int)HttpStatusCode.OK;
			// Escribir la respuesta en el flujo de salida y cerrar la conexión
			Stream outputStream = context.Response.OutputStream;
			outputStream.Write(responseBytes, 0, responseBytes.Length);
			outputStream.Close();
		}
	}
	catch
	{
		// ignored
	}
}, cts.Token);


// Esperar a que el usuario presione ENTER y detener el HttpListener
Console.ReadLine();
cts.Cancel();
listener.Stop();
 // Abortar el hilo del servidor
