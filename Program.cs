using DocuDiff.Services;

DocumentReader reader = new();
DocumentComparisonService comparer = new();

Console.Write("Enter ORIGINAL DOCX path: ");
string? originalPath = Console.ReadLine();

Console.Write("Enter MODIFIED DOCX path: ");
string? modifiedPath = Console.ReadLine();

if (string.IsNullOrWhiteSpace(originalPath) ||
    string.IsNullOrWhiteSpace(modifiedPath))
{
    Console.WriteLine("Both file paths are required.");
    return;
}

if (!File.Exists(originalPath) || !File.Exists(modifiedPath))
{
    Console.WriteLine("One or both files do not exist.");
    return;
}

var originalDocument = reader.Read(originalPath);
var modifiedDocument = reader.Read(modifiedPath);

var comparison = comparer.Compare(originalDocument, modifiedDocument);

Console.WriteLine();
Console.WriteLine("========== WORD-LEVEL COMPARISON ==========");
Console.WriteLine();

foreach (var paragraph in comparison.Paragraphs)
{
    Console.WriteLine($"Paragraph {paragraph.ParagraphNumber}:");

    foreach (var word in paragraph.Words)
    {
        Console.WriteLine($"{word.ChangeType}: {word.Text}");
    }

    Console.WriteLine();
}