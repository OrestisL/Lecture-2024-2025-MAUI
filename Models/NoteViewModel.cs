using Newtonsoft.Json;
namespace NTUA_Notes.Models;
public class NoteViewModel
{
    public string Header { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Date { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
    public Guid Id { get; set; } = Guid.Empty;
    [JsonIgnore]
    public bool IsDirty;
    [JsonIgnore]
    public bool ToDelete;

    public void UpdateValues(string header = "", string body = "", string date = "")
    {
        Header = header;
        Body = body;
        Date = date;
        IsDirty = true;
    }
}
