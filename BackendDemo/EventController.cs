using BackendDemo;
using Newtonsoft.Json;
using System.Web.Http;


public class EventController : ApiController
{
    [HttpGet]
    public EventList List()
    {
        try
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MatchData.json");
            string json = File.ReadAllText(path);
            var eventList  = JsonConvert.DeserializeObject<EventList>(json)!;

            if (eventList.Events != null)
            {
                foreach (var ev in eventList.Events)
                {
                    if (Storage.Instance.SimulatedTime < ev.EndGuessTime)
                    {
                        ev.Winner = -1;  
                    }
                }
            }

            return eventList;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading or deserializing JSON file: " + ex.Message);
            return new EventList(); 
        }
    }
}
