<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.AccountManagement.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.Protocols.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Namespace>System.DirectoryServices.AccountManagement</Namespace>
  <Namespace>System.DirectoryServices.ActiveDirectory</Namespace>
  <Namespace>System.DirectoryServices</Namespace>
</Query>

public static string SAVE_TO;

void Main()
{
	SAVE_TO = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "Output");

	UserInquiryByUsername();
	//UserInquiryByName();
	//GroupInquiry();
}

private void GroupInquiry()
{
	QueryGroup("Active directory group name", true);
}

private void UserInquiryByUsername()
{
	var lst = new List<string>()
	{
		"username1",
		"username2",
		"username3"
	};

	var contacts = QueryUsers(lst);

	//contacts.Dump();

	//string.Join(";", contacts.Select(x => x.Email)).Dump();

	var sb = new StringBuilder();

	foreach (var c in contacts)
	{
		sb.Append("{\"").Append(c.Username).Append("\", \"").Append(c.FirstName).Append(" ").Append(c.LastName).Append("\"},").AppendLine();
	}

	sb.ToString().Dump();
}

private void UserInquiryByName()
{
	var lst = new List<ContactInfo>()
	{
		new ContactInfo { FirstName = "John", LastName = "Smith" }
	};

	var lstAgg = new List<ContactInfo>();

	foreach (var element in lst)
	{
		lstAgg.AddRange(QueryUser(element));
	}

	lstAgg.Dump();
}

public List<ContactInfo> QueryUsers(List<string> threeFour)
{
	var lst = new List<ContactInfo>();

	foreach (var tf in threeFour)
	{
		try
		{
			lst.Add(QueryUser(tf, false));
		}
		catch(Exception ex)
		{
			Console.WriteLine($"Error on {tf}: {ex.Message}");			
		}
	}

	return lst;
}

public List<ContactInfo> QueryUser(ContactInfo info)
{
	return QueryUser(info.FirstName, info.LastName);
}

public List<ContactInfo> QueryUser(string firstName, string lastName)
{
	using (var pc = new PrincipalContext(ContextType.Domain, "ActiveDirectoryServerName", "DC=prefix,DC=adhost,DC=net"))
	{
		// define a "query-by-example" principal - here, we search for a UserPrincipal 
		// and with the first name (GivenName) and a last name (Surname) 
		var q = new UserPrincipal(pc);
		q.GivenName = firstName;
		q.Surname = lastName;
		//q.DisplayName = "Hayon Eli";

		// create your principal searcher passing in the QBE principal    
		var searcher = new PrincipalSearcher(q);

		var lst = searcher.FindAll();

		var lstReturn = lst.Select(x =>
		{
			var de = x.GetUnderlyingObject() as DirectoryEntry;
			
			//Sam Account Name in this case is the Three Four
			var c = GetContactInfo(x.SamAccountName, de);

			return c;
		}).ToList();

		Console.WriteLine($"Searching for {firstName} {lastName} yielded {lstReturn} results");

		return lstReturn;
	}
}

public ContactInfo QueryUser(string threeFour, bool saveToFile)
{
	using (var pc = new PrincipalContext(ContextType.Domain, "ActiveDirectoryServerName", "DC=prefix,DC=adhost,DC=net"))
	{
		//pc.Dump();

		// find the user by identity (or many other ways)
		using (var user = UserPrincipal.FindByIdentity(pc, IdentityType.Name, threeFour))
		{
			//user.Dump();
			var de = user.GetUnderlyingObject() as DirectoryEntry;
			//de.Properties.Dump();

			PrintAll(threeFour, de);

			var ci = GetContactInfo(threeFour, de);
			
			//ci.Dump();
			
		    return ci;
		}
	}
}

public ContactInfo QueryGroup(string groupName, bool saveToFile)
{
	using (var pc = new PrincipalContext(ContextType.Domain, "ActiveDirectoryServerName", "DC=prefix,DC=adhost,DC=net"))
	{
		//pc.Dump();

		// find the user by identity (or many other ways)
		using (var group = GroupPrincipal.FindByIdentity(pc, groupName))
		{
			group.Dump();
			
			var de = group.GetUnderlyingObject() as DirectoryEntry;
			//de.Properties.Dump();

			PrintAll(groupName, de);

			var ci = GetContactInfo(groupName, de);

			//ci.Dump();

			return ci;
		}
	}
}

public void PrintAll(string threeFour, DirectoryEntry de)
{
	var sb = new StringBuilder();

	foreach (PropertyValueCollection p in de.Properties)
	{
		sb.Append(p.PropertyName).Append("|").Append(p.Value).Append("|").Append(p.Count).AppendLine();
		
		if(p.Count < 2)
			continue;
		
		object[] arr = (object[])p.Value;

		for (int i = 0; i < p.Count; i++)
			sb.Append(p.PropertyName).Append("_").Append(i).Append("|").Append(arr[i]).Append("|").AppendLine();
	}
	
	//Console.WriteLine("{0}\t{1}\t{2}", p.PropertyName, p.Value, p.Count);

    File.WriteAllText(Path.Combine(SAVE_TO, threeFour + ".txt"), sb.ToString());
}

public ContactInfo GetContactInfo(string threeFour, DirectoryEntry de)
{
	List<PropertyValueCollection> lst = de.Properties.Cast<PropertyValueCollection>().ToList();
	
	var c = new ContactInfo();

	c.Username = threeFour;
	c.FirstName = GetStringValue(lst, "givenName");
	c.LastName = GetStringValue(lst, "sn");
	c.Organization = GetStringValue(lst, "extensionAttribute3"); //Alternatively you can use extensionAttribute1
	//c.TimeZone = GetStringValue(lst, "");
	c.Title = GetStringValue(lst, "title");
	c.Address1 = GetStringValue(lst, "streetAddress");
	c.City = GetStringValue(lst, "l");
	c.State = GetStringValue(lst, "st");
	c.Zip = GetStringValue(lst, "postalCode");
	c.Email = GetStringValue(lst, "mail");
	c.DeskPhone = GetStringValue(lst, "telephoneNumber");

	return c;
}

private string GetStringValue(List<PropertyValueCollection> lst, string propertyName)
{
	try
	{
		var str = Convert.ToString(lst.FirstOrDefault(x => x.PropertyName == propertyName).Value);
		
		return str;
	}
	catch(Exception ex)
	{
		return $"When trying to get the value for property \"{propertyName}\" this exception was thrown: {ex.Message}";
	}
}

public class ContactInfo
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Organization { get; set; }
	public string Title { get; set; }
	public string Username { get; set; }
	public string TimeZone { get; set; }
	public string Address1 { get; set; }
	public string Address2 { get; set; }
	public string City { get; set; }
	public string State { get; set; }
	public string Zip { get; set; }
	public string Email { get; set; }
	public string DeskPhone { get; set; }
	public string MobilePhone { get; set; }
}

//DirectoryEntry deUser = user.GetUnderlyingObject() as DirectoryEntry;
//deUser.Dump();
//DirectoryEntry deUserContainer = deUser.Parent;
//deUserContainer.Dump();
//deUserContainer.Properties["Organization"].Value.Dump();