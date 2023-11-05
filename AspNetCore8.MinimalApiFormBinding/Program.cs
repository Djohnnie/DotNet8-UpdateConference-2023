using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAntiforgery();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", (HttpContext context, IAntiforgery antiforgery) =>
{
    var token = antiforgery.GetAndStoreTokens(context);
    var html = $"""
        <html>
            <body>
                <form action="/todo" method="POST" enctype="multipart/form-data">
                    <input name="{token.FormFieldName}" type="hidden" value="{token.RequestToken}" />
                    <input type="text" name="name" />
                    <input type="date" name="dueDate" />
                    <input type="checkbox" name="isCompleted" />
                    <input type="submit" />
                </form>
            </body>
        </html>
    """;

    return Results.Content(html, "text/html");
});

app.MapPost("/todo", async ([FromForm] Todo todo, HttpContext context, IAntiforgery antiforgery) =>
{
    await antiforgery.ValidateRequestAsync(context);
    return Results.Ok(todo);
});

app.UseAntiforgery();
app.Run();

class Todo
{
    public string Name { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public DateTime DueDate { get; set; } = DateTime.Now.Add(TimeSpan.FromDays(1));
}