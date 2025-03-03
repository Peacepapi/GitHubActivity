using GitHubActivity.Models;
using System.Text.Json;
using System.Web;

var username = Console.ReadLine();
HttpClient client = new HttpClient();


var encodedUsername = HttpUtility.UrlEncode(username);
string url = $"https://api.github.com/users/{encodedUsername}/events";

client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

var response = await client.GetAsync(url);

if (response.IsSuccessStatusCode)
{
    var result = await response.Content.ReadAsStringAsync();
    var activities = JsonSerializer.Deserialize<List<Activity>>(result);

    displayActivities(activities);
}
else
{
    Console.WriteLine($"There was an error getting the activity : {response.ReasonPhrase}");
}

void displayActivities(List<Activity> activities)
{
    foreach(Activity activity in activities) 
    {
        switch(activity.Type)
        {
            case "PullRequestEvent":
                Console.WriteLine($"Pulled from {activity.Repo.Name} on {activity.CreatedAt}");
                break;
            case "PushEvent":
                Console.WriteLine($"Pushed {activity.Payload.Size} commits to {activity.Repo.Name} on {activity.CreatedAt}");
                break;
            case "IssueCommentEvent":
                Console.WriteLine($"Issue a comment on {activity.Repo.Name} on {activity.CreatedAt}");
                break;

            case "IssuesEvent":
                Console.WriteLine($"IssuesEvent");
                break;

            case "WatchEvent":
                Console.WriteLine($"WatchEvent");
                break;
            case "CreateEvent":
                Console.WriteLine($"Created a {activity.Payload.RefType} on {activity.Repo.Name} on {activity.CreatedAt}");
                break;
            case "DeleteEvent":
                Console.WriteLine($"Deleted a {activity.Payload.RefType} on {activity.Repo.Name} on {activity.CreatedAt}");
                break;
        }
    }
}