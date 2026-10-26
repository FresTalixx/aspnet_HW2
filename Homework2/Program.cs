using Homework2;
using Homework2.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

builder.Services.AddSingleton<MessageHelper>(sp =>
{
    var messagesFilePath = builder.Configuration.GetValue<string>("MessagesFilePath");
    if (string.IsNullOrEmpty(messagesFilePath))
    {
        throw new InvalidOperationException("MessagesFilePath is not configured in appsettings.json.");
    }
    return new MessageHelper(messagesFilePath);
});
// TODO налашутвання роботи з кодуванням JSON


var app = builder.Build();

var messages = new List<Message>();

messages.Add(new Message
{
    Address = "test1",
    Text = "Hello, this is a test message 1"
});

messages.Add(new Message
{
    Address = "test2",
    Text = "Hello, this is a test message 2"
});

messages.Add(new Message
{
    Address = "test3",
    Text = "Hello, this is a test message 3"
});

File.WriteAllText("messages.json", System.Text.Json.JsonSerializer.Serialize(messages));

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "My API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

var messagesFilePath = builder.Configuration.GetValue<string>("MessagesFilePath");

if (messagesFilePath == null)
{
    throw new InvalidOperationException("MessagesFilePath is not configured in appsettings.json.");
}



//Створити проєкт веб - апі

//реалізувати методи отримання усіх повідомлень, отримання одного повідомлення, повідомленнь від одного відправника (параметр адреса)
//видалення повідомленян по ід



//стоврити http файл з тестами методів щоб продемонтрувати як вони працюють

app.MapGet("/messages", async (MessageHelper messageHelper) =>
{
    var messages = await messageHelper.LoadMessagesAsync();
    return Results.Ok(messages);
});

app.MapGet("/messages/{id:int}", async (int id, MessageHelper messageHelper) =>
{
    var messages = await messageHelper.GetAllMessagesByIdAsync(id);
    if (messages.Count == 0)
    {
        return Results.NotFound();
    }
    return Results.Ok(messages);
});

app.MapGet("/messages/address/{address}", async (string address, MessageHelper messageHelper) =>
{
    var message = await messageHelper.GetMessagesByAddressAsync(address);
    if (message == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(message);
});

app.MapDelete("/messages/{id:int}", async (int id, MessageHelper messageHelper) =>
{
    await messageHelper.DeleteMessageAsync(id);
    return Results.NoContent();
});



app.Run();
