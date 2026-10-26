using Homework2.Models;

namespace Homework2;

public class MessageHelper
{
    private readonly string dataFilePath;
    public MessageHelper(string dataFilePath)
    {
        this.dataFilePath = dataFilePath;
    }

    public async Task<List<Message>> LoadMessagesAsync()
    {
        try
        {
            if (File.Exists(dataFilePath))
            {
                var jsonData = await File.ReadAllTextAsync(dataFilePath);
                return System.Text.Json.JsonSerializer.Deserialize<List<Message>>(jsonData) ?? [];
            }
            else
            {
                return [];
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading products: {ex.Message}");
            return [];
        }
    }
    public async Task SaveProductsAsync(List<Message> products)
    {
        try
        {
            var jsonData = System.Text.Json.JsonSerializer.Serialize(products);
            await File.WriteAllTextAsync(dataFilePath, jsonData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving products: {ex.Message}");
        }
    }

    public async Task<List<Message>> GetAllMessagesByIdAsync(int id)
    {
        var messages = await LoadMessagesAsync();
        return messages.Where(m => m.Id == id).ToList();
    }

    public async Task<Message?> GetMessagesByAddressAsync(string address)
    {
        var messages = await LoadMessagesAsync();
        return messages.FirstOrDefault(m => m.Address == address);
    }

    public async Task<Message?> GetLatestMessageByUserAsync(int id)
    {
        var messages = await LoadMessagesAsync();
        return messages.FirstOrDefault(m => m.Id == id);
    }

    public async Task SendMessageAsync(string from, string text)
    {
        var messages = await LoadMessagesAsync();
        var newMessage = new Message
        {
            Id = messages.Count > 0 ? messages.Max(m => m.Id) + 1 : 1,
            Address = from,
            Text = text,
            Date = DateTime.Now
        };
        messages.Add(newMessage);
        await SaveProductsAsync(messages);
    }

    public async Task DeleteMessageAsync(int id)
    {
        var messages = await LoadMessagesAsync();
        var messageToDelete = messages.FirstOrDefault(m => m.Id == id);
        if (messageToDelete != null)
        {
            messages.Remove(messageToDelete);
            await SaveProductsAsync(messages);
        }
    }

}
