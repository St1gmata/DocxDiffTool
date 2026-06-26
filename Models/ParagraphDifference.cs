namespace DocuDiff.Models;

public class ParagraphDifference
{
    public int ParagraphNumber { get; set; }

    public List<WordDifference> Words { get; set; } = new();
}