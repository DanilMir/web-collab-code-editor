namespace SyncService.Models;

//{"room":"1","data":{"monaco":{"type":"Text","content":"asdasdasdasd"}}}
public class CallbackRequestModel
{
    public string Room;
    public CallbackData Data;
}

public class CallbackData
{
    public Monaco Monaco;
}

public class Monaco
{
    public string Type;
    public byte[] Content;
}