namespace DocuDiff.Models;

public class ParagraphDifference
{
    public List<WordDifference> Words { get; set; } = new();
}