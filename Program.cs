using DocuDiff.Services;

DocumentReader reader = new();
ComparisonService comparer = new();

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

var result = comparer.Compare(originalDocument, modifiedDocument);

Console.WriteLine();
Console.WriteLine("========== COMPARISON ==========");
Console.WriteLine();

Console.WriteLine($"Added paragraphs: {result.AddedParagraphs.Count}");
foreach (var paragraph in result.AddedParagraphs)
{
    Console.WriteLine($"+ {paragraph}");
}

Console.WriteLine();

Console.WriteLine($"Removed paragraphs: {result.RemovedParagraphs.Count}");
foreach (var paragraph in result.RemovedParagraphs)
{
    Console.WriteLine($"- {paragraph}");
}

Console.WriteLine();

Console.WriteLine($"Unchanged paragraphs: {result.UnchangedParagraphs.Count}");