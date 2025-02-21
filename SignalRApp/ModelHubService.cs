namespace SignalRApp;

public class ModelHubService
{
    public List<ModelHub> ModelStartedHubs { get; set; } = new();

    public void StopActions(ModelHub sender)
    {
        List<ModelHub> removeList = new();
        foreach (ModelHub hub in ModelStartedHubs)
        {
            if (sender.Context.ConnectionId == hub.Context.ConnectionId)
            {
                hub.CancelTheToken();
                removeList.Add(hub);
            }
        }
        foreach (ModelHub hub in removeList) ModelStartedHubs.Remove(hub);
    }
}
