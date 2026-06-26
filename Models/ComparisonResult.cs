namespace DocuDiff.Models;

public class ComparisonResult
{
    public List<string> AddedParagraphs { get; set; } = new();
    public List<string> RemovedParagraphs { get; set; } = new();
    public List<string> UnchangedParagraphs { get; set; } = new();
}