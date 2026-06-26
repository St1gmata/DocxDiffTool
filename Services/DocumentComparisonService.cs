using DocuDiff.Models;

namespace DocuDiff.Services;

public class DocumentComparisonService
{
    private readonly WordComparisonService _wordComparer = new();

    public DocumentComparison Compare(DocumentContent original,
                                      DocumentContent modified)
    {
        var comparison = new DocumentComparison();

        int maxParagraphs = Math.Max(original.Paragraphs.Count,
                                     modified.Paragraphs.Count);

        for (int i = 0; i < maxParagraphs; i++)
        {
            string oldParagraph =
                i < original.Paragraphs.Count
                ? original.Paragraphs[i]
                : "";

            string newParagraph =
                i < modified.Paragraphs.Count
                ? modified.Paragraphs[i]
                : "";

            var paragraphDifference =
                _wordComparer.CompareWords(oldParagraph, newParagraph);

            paragraphDifference.ParagraphNumber = i + 1;

            comparison.Paragraphs.Add(paragraphDifference);
        }

        return comparison;
    }
}