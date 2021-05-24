<Query Kind="Program" />

void Main()
{
	MockParse();
	
	Console.WriteLine("Finished");
}

// Define other methods and classes here
private void MockParse()
{
	using (var reader = new StreamReader(@"J:\Dump\raw email message with attachment.txt"))
	{
		var HeaderContentType = "Content-Type: ";

		var line = reader.ReadLine();

		EmailParser parser = new EmailOnlyParser(null);

		while(line != null && line != ".")
		{
			if (line == string.Empty)
			{
				break;
			}
			
			if (line.StartsWith(HeaderContentType))
			{
				var contentType = line.Substring(HeaderContentType.Length);

				if (contentType == "multipart/mixed;")
				{
					parser = new BoundaryParser(reader);
				}
				else
				{
					parser = new EmailOnlyParser(reader);
				}
			}
			
			line = reader.ReadLine();
		}

		parser.ParseBody();

		parser.ParsedEmail.Dump();
	}
}

public abstract class EmailParser
{
	protected const string ContentTypeBodyValue = "text/html; charset=us-ascii";
	protected const string ContentFileTypeValue = "text/plain; name=";
	protected const string ContentTransferEncodingKey = "Content-Transfer-Encoding: ";
	protected const string ContentTransferEncodingBase64 = "base64";
	protected const string ContentTransferEncodingQuotedPrintable = "quoted-printable";
	protected const string ContentDispositionKey = "Content-Disposition: ";
	protected const string ContentDispositionValue = "attachment";
	protected const string ContentTypeKey = "Content-Type: ";
	
	public Email ParsedEmail { get; private set; } = new Email();
	protected readonly TextReader _reader;

	public EmailParser(TextReader reader)
	{
		_reader = reader;
	}

	protected string GetNextLine()
	{
		var line = _reader.ReadLine();

		while (line == string.Empty)
		{
			line = _reader.ReadLine();
		}

		return line;
	}

	protected abstract void ParseMessage();

	public abstract void ParseBody();
}

public class EmailOnlyParser
	: EmailParser
{
	public EmailOnlyParser(TextReader reader)
			: base(reader)
	{

	}

	protected override void ParseMessage()
	{
		var line = GetNextLine();

		var sb = new StringBuilder();

		while (line != null && line != ".")
		{
			if (line.StartsWith(ContentTransferEncodingKey))
			{
				//Save to Header Info
			}
			else
			{
				sb.Append(line);
			}

			line = GetNextLine();
		}
	
		ParsedEmail.Message = sb.ToString();
	}

	public override void ParseBody()
	{
		ParseMessage();
	}
}

public class BoundaryParser
	: EmailParser
{
	private string _boundaryValue;
	private string _boundaryOpen;
	private string _boundaryClose;
	private const string HeaderBoundary = " boundary=";
	private Regex _attachmentName;
	
	public BoundaryParser(TextReader reader)
		: base(reader)
	{
		var line = _reader.ReadLine();
		
		_boundaryValue = line.Substring(HeaderBoundary.Length);

		_boundaryOpen = "--" + _boundaryValue;
		
		_boundaryClose = _boundaryOpen + "--";
		
		_attachmentName = new Regex("Content-Type: .+; name=(.+)");
	}

	protected override void ParseMessage()
	{
		var line = GetNextLine();

		var sb = new StringBuilder();

		var boundaryCount = 0;

		while (line != null && boundaryCount < 2)
		{
			if (line == _boundaryOpen)
			{
				boundaryCount++;

				if(boundaryCount < 2)
				{
					line = GetNextLine();
				}

				continue;
			}
			else if (line.StartsWith(ContentTransferEncodingKey))
			{
				//Save to Header Info
			}
			else if (line.StartsWith(ContentTypeKey))
			{
				//Save to Header Info
			}
			else
			{
				sb.Append(line);
			}

			line = GetNextLine();
		}

		ParsedEmail.Message = sb.ToString();
	}
	
	protected void ParseAttachments()
	{
		var line = GetNextLine();

		var lst = new List<EmailAttachment>();
		
		var endReached = false;

		while (!endReached)
		{
			var a = new EmailAttachment();

			var sb = new StringBuilder();

			while (line != null)
			{
				if (line == _boundaryClose)
				{
					endReached = true;
					
					break;
				}
				else if (line == _boundaryOpen)
				{
					break;
				}
				else if (line.StartsWith(ContentTransferEncodingKey))
				{
					//Save to Header Info
				}
				else if (line.StartsWith(ContentTypeKey))
				{
					//Save attachment name
					var m = _attachmentName.Match(line);

					if (m.Success)
					{
						a.Name = m.Groups[1].Value;
					}
					else
					{
						a.Name = Path.GetRandomFileName();
					}
				}
				else if (line.StartsWith(ContentDispositionKey))
				{
					//Save to Header Info? Maybe?
				}
				else
				{
					sb.Append(line);
				}

				line = GetNextLine();
			}

			a.AttachmentBase64 = sb.ToString();

			lst.Add(a);
		
			line = GetNextLine();
		}
		
		ParsedEmail.Attachments = lst;
	}

	public override void ParseBody()
	{
		ParseMessage();
		
		ParseAttachments();
	}
}

public class Email
{
	public long EmailId { get; set; }

	public string From { get; set; }

	public string To { get; set; }

	public string Subject { get; set; }

	public string Message { get; set; }

	public string HeaderJson { get; set; }

	public DateTime CreatedOnUtc { get; set; }

	public List<EmailAttachment> Attachments { get; set; }
}

public class EmailAttachment
{
	public string Name { get; set; }
	
	public string AttachmentBase64 { get; set; }
}