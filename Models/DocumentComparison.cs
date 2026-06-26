namespace DocuDiff.Models;

public class DocumentComparison
{
    public List<ParagraphDifference> Paragraphs { get; set; } = new();
}