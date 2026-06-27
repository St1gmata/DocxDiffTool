using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocuDiff.Models;

namespace DocuDiff.Services;

public class DocumentReader
{
    public DocumentContent Read(string filePath)
    {
        var document = new DocumentContent();

        using WordprocessingDocument wordDocument =
            WordprocessingDocument.Open(filePath, false);

        var body = wordDocument.MainDocumentPart?.Document.Body;

        if (body == null)
            return document;

        foreach (Paragraph paragraph in body.Descendants<Paragraph>())
        {
            string text = paragraph.InnerText.Trim();

            if (!string.IsNullOrWhiteSpace(text))
                document.Paragraphs.Add(text);
        }

        foreach (Table table in body.Descendants<Table>())
        {
            foreach (TableRow row in table.Descendants<TableRow>())
            {
                var cells = row.Descendants<TableCell>()
                    .Select(cell => cell.InnerText.Trim())
                    .Where(text => !string.IsNullOrWhiteSpace(text))
                    .ToList();

                if (cells.Count > 0)
                    document.TableRows.Add(string.Join(" | ", cells));
            }
        }

        return document;
    }
}