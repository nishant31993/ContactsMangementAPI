namespace ContactsMangementAPI.Handlers
{
    public interface IFileHandler
    {
        bool Exists(string path);
        string ReadAllText(string path);
        void WriteAllText(string path, string content);
    }

    public class FileHandler : IFileHandler
    {
        public bool Exists(string path)
        {
            return File.Exists(path); // Check if the file exists
        }

        public string ReadAllText(string path)
        {
            if (Exists(path))
            {
                return File.ReadAllText(path); // Read the content of the file
            }
            throw new FileNotFoundException($"The file at {path} was not found.");
        }

        public void WriteAllText(string path, string content)
        {
            File.WriteAllText(path, content); // Write content to the file
        }
    }

}