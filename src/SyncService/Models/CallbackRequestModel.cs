
namespace SyncService.Models;

//{"room":"1","data":{"monaco":{"type":"Text","content":"asdasdasdasd"}}}
public class Data
{
    public Monaco Monaco { get; set; }
}

public class Monaco
{
    public string Type { get; set; }
    
    public string Content { get; set; }
}

public class CallbackRequestModel
{
    public string Room { get; set; }
    
    public Data Data { get; set; }
}