namespace DocuDiff.Models;

public class WordDifference
{
    public string Text { get; set; } = string.Empty;
    public string ChangeType { get; set; } = "Unchanged";
}