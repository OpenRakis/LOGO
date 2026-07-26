using System.Globalization;
using System.Text.RegularExpressions;

namespace Hnm0Encoder;

internal static class FrameDiscovery
{
    private static readonly Regex SplitRegex = new("(\\d+)", RegexOptions.Compiled);

    public static IReadOnlyList<string> DiscoverPngFrames(string directory)
    {
        string[] files = Directory.GetFiles(directory, "*.png", SearchOption.TopDirectoryOnly);
        if (files.Length == 0)
        {
            throw new InvalidDataException($"No .png files found in input directory: {directory}");
        }

        Array.Sort(files, NaturalPathComparer.Instance);

        if (HasDuplicateFileNamesCaseInsensitive(files))
        {
            throw new InvalidDataException("Input frame list contains case-insensitive duplicate file names, which is not supported.");
        }

        return files;
    }

    public static IReadOnlyList<string> SelectFrameWindow(
        IReadOnlyList<string> orderedFrames,
        int startIndex,
        int? maxFrames)
    {
        if (startIndex >= orderedFrames.Count)
        {
            throw new InvalidDataException($"--start-index {startIndex} is outside the discovered frame count ({orderedFrames.Count}).");
        }

        int count = maxFrames ?? (orderedFrames.Count - startIndex);
        if (count <= 0)
        {
            throw new InvalidDataException("Selected frame count must be > 0.");
        }

        if (startIndex + count > orderedFrames.Count)
        {
            throw new InvalidDataException(
                $"Requested frame window start={startIndex} count={count} exceeds discovered frame count {orderedFrames.Count}.");
        }

        string[] selected = new string[count];
        for (int i = 0; i < count; i++)
        {
            selected[i] = orderedFrames[startIndex + i];
        }

        return selected;
    }

    private static bool HasDuplicateFileNamesCaseInsensitive(IEnumerable<string> files)
    {
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (string file in files)
        {
            if (!names.Add(Path.GetFileName(file)))
            {
                return true;
            }
        }

        return false;
    }

    private sealed class NaturalPathComparer : IComparer<string>
    {
        public static NaturalPathComparer Instance { get; } = new();

        public int Compare(string? x, string? y)
        {
            if (x is null || y is null)
            {
                throw new InvalidOperationException("Natural path comparison received a null value.");
            }

            string nx = Path.GetFileName(x);
            string ny = Path.GetFileName(y);
            return CompareNatural(nx, ny);
        }

        private static int CompareNatural(string a, string b)
        {
            string[] ax = SplitRegex.Split(a);
            string[] bx = SplitRegex.Split(b);

            int count = Math.Min(ax.Length, bx.Length);
            for (int i = 0; i < count; i++)
            {
                string pa = ax[i];
                string pb = bx[i];

                bool na = IsDigits(pa);
                bool nb = IsDigits(pb);
                int cmp;

                if (na && nb)
                {
                    int ia = int.Parse(pa, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    int ib = int.Parse(pb, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    cmp = ia.CompareTo(ib);
                }
                else
                {
                    cmp = string.Compare(pa, pb, StringComparison.OrdinalIgnoreCase);
                }

                if (cmp != 0)
                {
                    return cmp;
                }
            }

            return ax.Length.CompareTo(bx.Length);
        }

        private static bool IsDigits(string text)
        {
            if (text.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < text.Length; i++)
            {
                if (!char.IsDigit(text[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
