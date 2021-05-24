<Query Kind="Program">
  <NuGetReference>Microsoft.TeamFoundationServer.Client</NuGetReference>
  <NuGetReference>Microsoft.VisualStudio.Services.Client</NuGetReference>
  <NuGetReference>Microsoft.VisualStudio.Services.InteractiveClient</NuGetReference>
  <Namespace>Microsoft.VisualStudio.Services.WebApi</Namespace>
  <Namespace>Microsoft.TeamFoundation.WorkItemTracking.WebApi</Namespace>
  <Namespace>Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models</Namespace>
  <Namespace>Microsoft.VisualStudio.Services.Common</Namespace>
</Query>

//// Define other methods and classes here
//// https://www.nuget.org/packages/Microsoft.TeamFoundationServer.Client/
//using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
//using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
//
//// https://www.nuget.org/packages/Microsoft.VisualStudio.Services.InteractiveClient/
//using Microsoft.VisualStudio.Services.Client;
//
//// https://www.nuget.org/packages/Microsoft.VisualStudio.Services.Client/
//using Microsoft.VisualStudio.Services.Common; 

/// <summary>
/// This sample creates a new work item query for New Bugs, stores it under 'MyQueries', runs the query, and then sends the results to the console.
/// </summary>
public static void Main()
{
	var uri = "https://Tfs-Url.net/Collection";
	var pat = "a27b7iby6ptwgh5fn7f5n7begr3ba6js66ek473vhvohg26dm7ha";
	var repo = "Repository";
	
	// Connection object could be created once per application and we will use it to get httpclient objects. 
	// Httpclients have been reused between callers and threads.
	// Their lifetime has been managed by connection (we don't have to dispose them).
	// This is more robust then newing up httpclient objects directly.  

	//new VssCredentials() <-- use this to be prompted for credentials
	//new VssBasicCredential(string.Empty, pat) <-- use for PAT
	
	// Be sure to send in the full collection uri, i.e. http://myserver:8080/tfs/defaultcollection
	// We are using default VssCredentials which uses NTLM against a Team Foundation Server.  See additional provided
	// examples for creating credentials for other types of authentication.
	VssConnection connection = new VssConnection(new Uri(uri), new VssBasicCredential(string.Empty, pat));

	// Create instance of WorkItemTrackingHttpClient using VssConnection
	WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();

	// Get 2 levels of query hierarchy items
	List<QueryHierarchyItem> queryHierarchyItems = witClient.GetQueriesAsync(repo, depth: 2).Result;

	// Search for 'My Queries' folder
	QueryHierarchyItem myQueriesFolder = queryHierarchyItems.FirstOrDefault(qhi => qhi.Name.Equals("My Queries"));
	if (myQueriesFolder != null)
	{
		string queryName = "REST Sample";

		// See if our 'REST Sample' query already exists under 'My Queries' folder.
		QueryHierarchyItem newBugsQuery = null;
		
		if (myQueriesFolder.Children != null)
		{
			newBugsQuery = myQueriesFolder.Children.FirstOrDefault(qhi => qhi.Name.Equals(queryName));
		}
		
		if (newBugsQuery == null)
		{
			// if the 'REST Sample' query does not exist, create it.
			newBugsQuery = new QueryHierarchyItem()
			{
				Name = queryName,
				Wiql = "SELECT [System.Id],[System.WorkItemType],[System.Title],[System.AssignedTo],[System.State],[System.Tags] FROM WorkItems WHERE [System.TeamProject] = @project AND [System.WorkItemType] = 'Bug' AND [System.State] = 'New'",
				IsFolder = false
			};
		
			newBugsQuery = witClient.CreateQueryAsync(newBugsQuery, repo, myQueriesFolder.Name).Result;
		}

		// run the 'REST Sample' query
		WorkItemQueryResult result = witClient.QueryByIdAsync(newBugsQuery.Id).Result;

		if (result.WorkItems.Any())
		{
			int skip = 0;
			const int batchSize = 100;
			IEnumerable<WorkItemReference> workItemRefs;
			do
			{
				workItemRefs = result.WorkItems.Skip(skip).Take(batchSize);
				if (workItemRefs.Any())
				{
					// get details for each work item in the batch
					List<WorkItem> workItems = witClient.GetWorkItemsAsync(workItemRefs.Select(wir => wir.Id)).Result;
					foreach (WorkItem workItem in workItems)
					{
						// write work item to console
						Console.WriteLine("{0} {1}", workItem.Id, workItem.Fields["System.Title"]);
					}
				}
				skip += batchSize;
			}
			while (workItemRefs.Count() == batchSize);
		}
		else
		{
			Console.WriteLine("No work items were returned from query.");
		}
	}
}