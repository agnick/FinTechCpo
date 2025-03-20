namespace FinTechApp.Importers;

public abstract class DataImporter<T>
{
    public void ImportData(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Неверно указан путь к файлу", nameof(filePath));

        try
        {
            string fileContent = File.ReadAllText(filePath);
            T data = ParseData(fileContent);
            ProcessData(data);
        }
        catch (Exception ex)
        {
            throw new Exception("Ошибка при импорте данных: " + ex.Message, ex);
        }
    }

    protected abstract T ParseData(string fileContent);
    protected abstract void ProcessData(T data);
}