using DocuDiff.Services;

Console.Write("Enter DOCX file path: ");

string? path = Console.ReadLine();

if (string.IsNullOrWhiteSpace(path))
{
    Console.WriteLine("No file selected.");
    return;
}

if (!File.Exists(path))
{
    Console.WriteLine("File not found.");
    return;
}

DocumentReader reader = new();

var document = reader.Read(path);

Console.WriteLine();
Console.WriteLine("Paragraphs found:");
Console.WriteLine("----------------------------");

foreach (string paragraph in document.Paragraphs)
{
    Console.WriteLine(paragraph);
}
