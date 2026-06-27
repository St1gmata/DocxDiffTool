using DocuDiff.Services;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;

namespace DocuDiff.UI;

public partial class MainWindow : Window
{
    private string? _originalPath;
    private string? _modifiedPath;
    private string _latestReportText = "";

    public MainWindow()
    {
        InitializeComponent();
    }

    private void BrowseOriginal_Click(object sender, RoutedEventArgs e)
    {
        _originalPath = PickDocxFile();

        if (_originalPath != null)
            OriginalPathText.Text = _originalPath;
    }

    private void BrowseModified_Click(object sender, RoutedEventArgs e)
    {
        _modifiedPath = PickDocxFile();

        if (_modifiedPath != null)
            ModifiedPathText.Text = _modifiedPath;
    }

    private void Compare_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_originalPath) ||
            string.IsNullOrWhiteSpace(_modifiedPath))
        {
            MessageBox.Show("Please select both DOCX files.");
            return;
        }

        DocumentReader reader = new();
        DocumentComparisonService comparer = new();

        var original = reader.Read(_originalPath);
        var modified = reader.Read(_modifiedPath);
        var comparison = comparer.Compare(original, modified);

        StringBuilder output = new();

        AppendTableDifferences(output, original.TableRows, modified.TableRows);

        foreach (var paragraph in comparison.Paragraphs)
        {
            var changedWords = paragraph.Words
                .Where(w => w.ChangeType != "Unchanged")
                .ToList();

            if (changedWords.Count == 0)
                continue;

            output.AppendLine($"Paragraph {paragraph.ParagraphNumber}:");

            foreach (var word in changedWords)
                output.AppendLine($"{word.ChangeType}: {word.Text}");

            output.AppendLine();
        }

        _latestReportText = output.Length == 0
            ? "No differences found."
            : output.ToString();

        ResultText.Text = _latestReportText;
    }

    private void ExportReport_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_latestReportText))
        {
            MessageBox.Show("Please compare two documents first.");
            return;
        }

        SaveFileDialog dialog = new()
        {
            Filter = "HTML Report (*.html)|*.html",
            FileName = "DocumentComparisonReport.html",
            Title = "Save Comparison Report"
        };

        if (dialog.ShowDialog() != true)
            return;

        string html = BuildHtmlReport(_latestReportText);

        File.WriteAllText(dialog.FileName, html, Encoding.UTF8);

        Process.Start(new ProcessStartInfo
        {
            FileName = dialog.FileName,
            UseShellExecute = true
        });
    }

    private static string BuildHtmlReport(string reportText)
    {
        string encoded = WebUtility.HtmlEncode(reportText);

        encoded = encoded
            .Replace("Missing row:", "<span class='deleted'>Missing row:</span>")
            .Replace("Deleted:", "<span class='deleted'>Deleted:</span>")
            .Replace("Added row:", "<span class='inserted'>Added row:</span>")
            .Replace("Inserted:", "<span class='inserted'>Inserted:</span>");

        return @$"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <title>Luai's Document Comparer Report</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            margin: 40px;
            background: #f5f5f5;
            color: #222;
        }}

        .container {{
            background: white;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0 4px 20px rgba(0,0,0,0.1);
        }}

        h1 {{
            margin-top: 0;
        }}

        pre {{
            white-space: pre-wrap;
            font-family: Consolas, monospace;
            line-height: 1.6;
            background: #fafafa;
            padding: 20px;
            border-radius: 8px;
            border: 1px solid #ddd;
        }}

        .deleted {{
            color: #b00020;
            font-weight: bold;
        }}

        .inserted {{
            color: #0b7a27;
            font-weight: bold;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <h1>Luai's Document Comparer Report</h1>
        <p>Generated comparison report for two Word documents.</p>
        <pre>{encoded}</pre>
    </div>
</body>
</html>";
    }

    private static void AppendTableDifferences(
        StringBuilder output,
        List<string> originalRows,
        List<string> modifiedRows)
    {
        var missingRows = originalRows
            .Where(row => !modifiedRows.Contains(row))
            .ToList();

        var addedRows = modifiedRows
            .Where(row => !originalRows.Contains(row))
            .ToList();

        if (missingRows.Count == 0 && addedRows.Count == 0)
            return;

        output.AppendLine("========== TABLE DIFFERENCES ==========");
        output.AppendLine();

        foreach (var row in missingRows)
            output.AppendLine($"Missing row: {row}");

        foreach (var row in addedRows)
            output.AppendLine($"Added row: {row}");

        output.AppendLine();
    }

    private static string? PickDocxFile()
    {
        OpenFileDialog dialog = new()
        {
            Filter = "Word Documents (*.docx)|*.docx",
            Title = "Select Word Document"
        };

        return dialog.ShowDialog() == true
            ? dialog.FileName
            : null;
    }
}