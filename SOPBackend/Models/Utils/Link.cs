namespace SOPBackend.Utils;

public class Link
{
    public String Href { get; set; }
    public String Rel { get; set; }
    public String Method { get; set; }

    public Link(String href, String rel, String method)
    {
        Href = href;
        Rel = rel;
        Method = method;
    }
}