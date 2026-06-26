using DocuDiff.Services;

WordComparisonService wordComparer = new();

var result = wordComparer.CompareWords(
    "John likes pizza.",
    "John really likes pizza."
);

foreach (var word in result.Words)
{
    Console.WriteLine($"{word.ChangeType}: {word.Text}");
}