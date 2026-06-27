namespace DocuDiff.Models;

public class DocumentContent
{
    public List<string> Paragraphs { get; set; } = new();

    public List<string> TableRows { get; set; } = new();
}