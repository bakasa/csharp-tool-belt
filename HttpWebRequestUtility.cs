public enum HttpMethodType
{
	GET = 0
	, PUT
	, POST
}   

public class HttpWebRequestUtility
{
	private WebRequest request;
	private Stream dataStream;

	private string status;

	public String Status
	{
		get
		{
			return status;
		}
		set
		{
			status = value;
		}
	}

	public HttpWebRequestUtility(string url)
	{
		// Create a request using a URL that can receive a post.

		request = WebRequest.Create(url);
	}

	public HttpWebRequestUtility(string url, HttpMethodType method)
		: this(url)
	{
		// Set the Method property of the request to POST.
		request.Method =  Enum.GetName(typeof(HttpMethodType), method);                
	}

	public HttpWebRequestUtility(string url, HttpMethodType method, string data)
		: this(url, method)
	{

		// Create POST data and convert it to a byte array.
		string postData = data;
		byte[] byteArray = Encoding.UTF8.GetBytes(postData);

		// Set the ContentType property of the WebRequest.
		request.ContentType = "application/json";

		// Set the ContentLength property of the WebRequest.
		request.ContentLength = byteArray.Length;

		// Get the request stream.
		dataStream = request.GetRequestStream();

		// Write the data to the request stream.
		dataStream.Write(byteArray, 0, byteArray.Length);

		// Close the Stream object.
		dataStream.Close();

	}

	public string GetResponse()
	{
		// Get the original response.
		WebResponse response = request.GetResponse();

		this.Status = ((HttpWebResponse)response).StatusDescription;

		// Get the stream containing all content returned by the requested server.
		dataStream = response.GetResponseStream();

		// Open the stream using a StreamReader for easy access.
		StreamReader reader = new StreamReader(dataStream);

		// Read the content fully up to the end.
		string responseFromServer = reader.ReadToEnd();

		// Clean up the streams.
		reader.Close();
		dataStream.Close();
		response.Close();

		return responseFromServer;
	}

}