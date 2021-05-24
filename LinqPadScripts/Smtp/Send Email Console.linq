<Query Kind="Program">
  <Namespace>System.Net.Mail</Namespace>
</Query>

void Main(string[] args)
{
	//From, To, Host, Port, Subject, Body
	Console.WriteLine("Start");
	
	SmtpExample("email@email.com"); //Works just fine
	
	Console.WriteLine("End");
}

public void SmtpExample(string addressTo)
{
	using (var client = new SmtpClient())
	{
		using (var mail = new MailMessage("linqPadTest@local.com", addressTo))
		{
			client.Port = 25;
			client.DeliveryMethod = SmtpDeliveryMethod.Network;
			client.UseDefaultCredentials = false;
			//client.Host = "smtp-host.net";
			client.Host = "local.machine.fqdn";
			mail.Subject = "this is a test email.";
			mail.Body = "this is my test email body";
			client.Send(mail);
		}
	}
}