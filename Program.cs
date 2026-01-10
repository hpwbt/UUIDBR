using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace UUIDBR;

internal static class Program
{
    private const string ModeRandom = "random";
    private const string ModeSequential = "sequential";

    [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
    private static extern int StrCmpLogicalW(string x, string y);

    private static int Main(string[] args)
    {
        if (args.Length < 2)
            return 1;

        string mode = args[0].ToLower();
        string directory = args[1];

        if (!Directory.Exists(directory))
            return 1;

        string[] entries = GetVisibleEntries(directory);

        if (entries.Length == 0)
            return 0;

        switch (mode)
        {
            case ModeRandom:
                RenameRandom(directory, entries);
                return 0;
            case ModeSequential:
                RenameSequential(directory, entries);
                return 0;
            default:
                return 1;
        }
    }

    private static string[] GetVisibleEntries(string directory)
    {
        return Directory.GetFileSystemEntries(directory)
            .Where(e => !IsHiddenOrSystem(e))
            .ToArray();
    }

    private static bool IsHiddenOrSystem(string path)
    {
        try
        {
            FileAttributes attributes = File.GetAttributes(path);
            return (attributes & (FileAttributes.Hidden | FileAttributes.System)) != 0;
        }
        catch
        {
            return false;
        }
    }

    private static void RenameRandom(string directory, string[] entries)
    {
        foreach (string entry in entries)
        {
            string uuid = Guid.NewGuid().ToString().ToLower();
            MoveEntry(directory, entry, uuid);
        }
    }

    private static void RenameSequential(string directory, string[] entries)
    {
        string uuid = Guid.NewGuid().ToString().ToLower();
        Array.Sort(entries, StrCmpLogicalW);

        for (int i = 0; i < entries.Length; i++)
            MoveEntry(directory, entries[i], $"{uuid} {i + 1}");
    }

    private static void MoveEntry(string directory, string entry, string newName)
    {
        try
        {
            if (Directory.Exists(entry))
            {
                Directory.Move(entry, Path.Combine(directory, newName));
            }
            else if (File.Exists(entry))
            {
                string extension = Path.GetExtension(entry);
                File.Move(entry, Path.Combine(directory, newName + extension));
            }
        }
        // Silently ignores locked or missing entries.
        catch { }
    }
}