using DocuDiff.Models;

namespace DocuDiff.Services;

public class WordComparisonService
{
    public ParagraphDifference CompareWords(string originalText, string modifiedText)
    {
        var result = new ParagraphDifference();

        string[] originalWords = originalText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string[] modifiedWords = modifiedText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        int[,] table = BuildLcsTable(originalWords, modifiedWords);

        int i = 0;
        int j = 0;

        while (i < originalWords.Length && j < modifiedWords.Length)
        {
            if (originalWords[i] == modifiedWords[j])
            {
                result.Words.Add(new WordDifference
                {
                    Text = originalWords[i],
                    ChangeType = "Unchanged"
                });

                i++;
                j++;
            }
            else if (table[i + 1, j] >= table[i, j + 1])
            {
                result.Words.Add(new WordDifference
                {
                    Text = originalWords[i],
                    ChangeType = "Deleted"
                });

                i++;
            }
            else
            {
                result.Words.Add(new WordDifference
                {
                    Text = modifiedWords[j],
                    ChangeType = "Inserted"
                });

                j++;
            }
        }

        while (i < originalWords.Length)
        {
            result.Words.Add(new WordDifference
            {
                Text = originalWords[i],
                ChangeType = "Deleted"
            });

            i++;
        }

        while (j < modifiedWords.Length)
        {
            result.Words.Add(new WordDifference
            {
                Text = modifiedWords[j],
                ChangeType = "Inserted"
            });

            j++;
        }

        return result;
    }

    private int[,] BuildLcsTable(string[] originalWords, string[] modifiedWords)
    {
        int[,] table = new int[originalWords.Length + 1, modifiedWords.Length + 1];

        for (int i = originalWords.Length - 1; i >= 0; i--)
        {
            for (int j = modifiedWords.Length - 1; j >= 0; j--)
            {
                if (originalWords[i] == modifiedWords[j])
                    table[i, j] = table[i + 1, j + 1] + 1;
                else
                    table[i, j] = Math.Max(table[i + 1, j], table[i, j + 1]);
            }
        }

        return table;
    }
}