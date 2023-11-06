var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(opt =>
{
    opt.WithOrigins("https://localhost:4200").AllowAnyMethod().SetIsOriginAllowed(_ => true);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
