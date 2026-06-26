using DocuDiff.Models;

namespace DocuDiff.Services;

public class ComparisonService
{
    public ComparisonResult Compare(DocumentContent oldDocument, DocumentContent newDocument)
    {
        var result = new ComparisonResult();

        foreach (string paragraph in oldDocument.Paragraphs)
        {
            if (newDocument.Paragraphs.Contains(paragraph))
                result.UnchangedParagraphs.Add(paragraph);
            else
                result.RemovedParagraphs.Add(paragraph);
        }

        foreach (string paragraph in newDocument.Paragraphs)
        {
            if (!oldDocument.Paragraphs.Contains(paragraph))
                result.AddedParagraphs.Add(paragraph);
        }

        return result;
    }
}