<Query Kind="Program">
  <Namespace>System.Net.Mail</Namespace>
</Query>

void Main()
{
	Console.WriteLine("Start");
	SmtpExample("email.name@domain.com"); //Works just fine
	//SmtpExample(string.Empty); //Fails
	//SmtpExample("email.name@domain.com;email.name2@domain.com"); //fails
	//SmtpExample("email.name@domain.com,email.name2@domain.com"); //Works
	//SmtpExample("email.name@domain.com, email.name2@domain.com"); //Works
	//SmtpExample("email.name@domain.com "); //Works
	//SmtpExample("email.name@domain.com\r\n"); //Works
	//SmtpExample("email.name@domain.com\r\n"); //Works
	
	//ValidateEmailList();
	
	Console.WriteLine("End");
}

public class EmailUnit
{
	public string EmailAddress { get; set; }
	public bool IsValid { get; set; }
}

public void ValidateEmailList()
{
	List<string> lst = GetStringList(@"C:\EmailList.txt");
	
	var lstRes = new List<EmailUnit>();
	
	foreach(string email in lst)
		lstRes.Add(new EmailUnit() { EmailAddress = email, IsValid = IsValidEmailAddress(email) });

	List<EmailUnit> lstInvalid = lstRes.FindAll(x => !x.IsValid);
	int invalidCount = lstRes.Count(x => !x.IsValid);
	
	Console.WriteLine("Emails: {0}, Valid: {1}, Invalid {2}", lstRes.Count, lstRes.Count(x => x.IsValid), invalidCount);
	
	if(invalidCount > 0)
	{
		Console.WriteLine("Invalid Emails\n===================");
		
		foreach(EmailUnit eu in lstRes.FindAll(x => !x.IsValid))
			Console.WriteLine("[" + eu.EmailAddress + "]");
	}
}

public bool IsValidEmailAddress(string emailAddress)
{
	try
	{
		new MailAddress(emailAddress);
	
		return true;
	}
	catch(Exception ex)
	{
		ex.Dump();
		return false;
	}
}

// Define other methods and classes here
public void SmtpExample(string addressTo)
{
	using(var client = new SmtpClient())
	{
		using(var mail = new MailMessage("linqPadTest@local.com", addressTo))
		{
			client.Port = 25;
			client.DeliveryMethod = SmtpDeliveryMethod.Network;
			client.UseDefaultCredentials = false;
			client.Host = "smtp-host.net";
			//client.Host = "local.machine.fqdn";
			mail.Subject = "this is a test email.";
			mail.Body = "this is my test email body";
			client.Send(mail);
		}
	}
}

public List<string> GetStringList(string targetFullFilePath)
{
	var lst = new List<string>();
	
	using(var sr = new StreamReader(targetFullFilePath))
	{
		string strLine = null;
	
		while(!sr.EndOfStream)
		{
			strLine = sr.ReadLine();
			
			if(string.IsNullOrWhiteSpace(strLine))
				continue;
				
			lst.Add(strLine);
		}
	}
	
	return lst;
}