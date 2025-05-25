using KanbanBoardAPI.Data;
using KanbanBoardAPI.Models;
using Microsoft.EntityFrameworkCore;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;


var builder = WebApplication.CreateBuilder(args);

// SERVICES
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (Only Once)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://kanban-board-xtt1.onrender.com") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// MIDDLEWARE
app.UseCors(); // Enable global CORS

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<FirebaseAuthMiddleware>();

app.UseAuthorization();

app.MapControllers();

// SEED DATABASE
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Columns.Any())
    {
        db.Columns.AddRange(
            new Column { Title = "Active Tasks" },
            new Column { Title = "In Progress" },
            new Column { Title = "Completed Tasks" }
        );
        db.SaveChanges();
    }

    // Debug print
    var tasks = db.Tasks.ToList();
    Console.WriteLine("==== TASKS ====");
    foreach (var t in tasks)
    {
        Console.WriteLine($"ID: {t.Id}, Title: {t.Title}, ColumnId: {t.ColumnId}");
    }
}

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("firebase-service-account.json")
});


app.Run();
