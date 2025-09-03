using KanbanBoardAPI.Data;
using KanbanBoardAPI.Models;
using Microsoft.EntityFrameworkCore;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS — list every frontend origin 
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins(
                "https://kanban-board-xtt1.onrender.com", // your existing Render frontend (if you still use it)
                "https://muditjha20.github.io",           // NEW: GitHub Pages origin
                "http://localhost:5173",                  // optional: Vite dev
                "http://127.0.0.1:5500"                   // optional: Live Server
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// ===== Pipeline (ORDER MATTERS) =====
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Public health & root endpoints (unauthenticated)
app.MapGet("/", () => Results.Text("Kanban API is running", "text/plain"));
app.MapGet("/health", () => Results.Ok(new { ok = true }));

// Auth middleware MUST run after CORS and before controllers
app.UseMiddleware<FirebaseAuthMiddleware>();

app.UseAuthorization();

app.MapControllers();

// ===== SEED DATABASE =====
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Ensure tables exist by running migrations
    db.Database.Migrate();

    // seed if no columns exist
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

// Firebase Admin init (Render env var must be set)
FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromJson(
        Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS_JSON"))
});

app.Run();
