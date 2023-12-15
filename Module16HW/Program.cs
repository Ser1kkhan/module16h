using System;
using System.IO;

class Program
{
    private static string currentDirectory;
    private static string logFileName = "file_manager_log.txt";
    private static string logFilePath = Path.Combine(Environment.CurrentDirectory, logFileName);


    static void Main(string[] args)
    {
        Console.WriteLine("Simple File Manager");
        Console.WriteLine("===================");

        while (true)
        {
            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. View Directory Contents");
            Console.WriteLine("2. Create File/Directory");
            Console.WriteLine("3. Delete File/Directory");
            Console.WriteLine("4. Copy File/Directory");
            Console.WriteLine("5. Move File/Directory");
            Console.WriteLine("6. Read/Write to File");
            Console.WriteLine("7. Show Log");
            Console.WriteLine("8. Exit");

            int choice = GetIntegerInput("Enter your choice: ");

            switch (choice)
            {
                case 1:
                    ViewDirectoryContents();
                    break;
                case 2:
                    CreateFileOrDirectory();
                    break;
                case 3:
                    DeleteFileOrDirectory();
                    break;
                case 4:
                    CopyFileOrDirectory();
                    break;
                case 5:
                    MoveFileOrDirectory();
                    break;
                case 6:
                    ReadWriteToFile();
                    break;
                case 7:
                    ShowLog();
                    break;
                case 8:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void ViewDirectoryContents()
    {
        currentDirectory = GetStringInput("Enter the directory path: ");

        if (!Directory.Exists(currentDirectory))
        {
            Console.WriteLine("Directory does not exist.");
            return;
        }

        Console.WriteLine($"Contents of {currentDirectory}:");
        string[] directories = Directory.GetDirectories(currentDirectory);
        string[] files = Directory.GetFiles(currentDirectory);

        foreach (var directory in directories)
        {
            Console.WriteLine($"[Dir] {Path.GetFileName(directory)}");
        }

        foreach (var file in files)
        {
            Console.WriteLine($"[File] {Path.GetFileName(file)}");
        }
    }

    private static void CreateFileOrDirectory()
    {
        string path = GetStringInput("Enter the path for the new file/directory: ");
        if (File.Exists(path) || Directory.Exists(path))
        {
            Console.WriteLine("File/directory already exists.");
            return;
        }

        if (GetBooleanInput("Create a file? (Y/N): "))
        {
            File.Create(path).Close();
            Console.WriteLine("File created successfully.");
        }
        else
        {
            Directory.CreateDirectory(path);
            Console.WriteLine("Directory created successfully.");
        }
    }

    private static void DeleteFileOrDirectory()
    {
        string path = GetStringInput("Enter the path to delete: ");

        if (File.Exists(path))
        {
            File.Delete(path);
            Console.WriteLine("File deleted successfully.");
        }
        else if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
            Console.WriteLine("Directory deleted successfully.");
        }
        else
        {
            Console.WriteLine("File/directory does not exist.");
        }
    }

    private static void CopyFileOrDirectory()
    {
        string sourcePath = GetStringInput("Enter the source path: ");
        string destinationPath = GetStringInput("Enter the destination path: ");

        if (File.Exists(sourcePath))
        {
            File.Copy(sourcePath, destinationPath, true);
            Console.WriteLine("File copied successfully.");
        }
        else if (Directory.Exists(sourcePath))
        {
            CopyDirectory(sourcePath, destinationPath);
            Console.WriteLine("Directory copied successfully.");
        }
        else
        {
            Console.WriteLine("Source file/directory does not exist.");
        }
    }

    private static void CopyDirectory(string sourceDir, string destDir)
    {
        if (!Directory.Exists(destDir))
        {
            Directory.CreateDirectory(destDir);
        }

        string[] files = Directory.GetFiles(sourceDir);
        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            string destFile = Path.Combine(destDir, fileName);
            File.Copy(file, destFile, true);
        }

        string[] dirs = Directory.GetDirectories(sourceDir);
        foreach (string dir in dirs)
        {
            string dirName = Path.GetFileName(dir);
            string destDirPath = Path.Combine(destDir, dirName);
            CopyDirectory(dir, destDirPath);
        }
    }

    private static void MoveFileOrDirectory()
    {
        string sourcePath = GetStringInput("Enter the source path: ");
        string destinationPath = GetStringInput("Enter the destination path: ");

        if (File.Exists(sourcePath))
        {
            File.Move(sourcePath, destinationPath);
            Console.WriteLine("File moved successfully.");
        }
        else if (Directory.Exists(sourcePath))
        {
            Directory.Move(sourcePath, destinationPath);
            Console.WriteLine("Directory moved successfully.");
        }
        else
        {
            Console.WriteLine("Source file/directory does not exist.");
        }
    }

    private static void ReadWriteToFile()
    {
        string filePath = GetStringInput("Enter the file path: ");
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File does not exist.");
            return;
        }

        string fileContents = File.ReadAllText(filePath);
        Console.WriteLine($"Current contents of {filePath}:\n{fileContents}");

        if (GetBooleanInput("Do you want to overwrite the file? (Y/N): "))
        {
            string newContents = GetStringInput("Enter new file contents: ");
            File.WriteAllText(filePath, newContents);
            Console.WriteLine("File updated successfully.");
        }
    }

    private static void ShowLog()
    {
        if (File.Exists(logFilePath))
        {
            string logContents = File.ReadAllText(logFilePath);
            Console.WriteLine($"Log File Contents:\n{logContents}");
        }
        else
        {
            Console.WriteLine("Log file does not exist.");
        }
    }

    private static string GetStringInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    private static int GetIntegerInput(string prompt)
    {
        Console.Write(prompt);
        int value;
        while (!int.TryParse(Console.ReadLine(), out value))
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
            Console.Write(prompt);
        }
        return value;
    }

    private static bool GetBooleanInput(string prompt)
    {
        Console.Write(prompt);
        string input = Console.ReadLine();
        return input.Equals("Y", StringComparison.OrdinalIgnoreCase);
    }
}
