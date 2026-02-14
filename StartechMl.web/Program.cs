using StartechML.Core.Utils;

var builder = WebApplication.CreateBuilder(args);

// INICIALIZAR LOGGER (ANTES DE Build)
Logger.SetLogPath(Path.Combine(AppContext.BaseDirectory, "Log"));
Logger.SetLogLine(85);
Logger.Write("Inicio aplicación StartechML.WEB", "Y", "Y", Logger.Mode.Info.ToString());

builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Habilitamos CORS para permitir llamadas desde React
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});



var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors("ReactPolicy");


// LOG DE AMBIENTE
if (!app.Environment.IsDevelopment())
{
    Logger.Write("Ambiente PRODUCCIÓN", "Y", "Y", Logger.Mode.Info.ToString());
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    Logger.Write("Ambiente DESARROLLO", "Y", "Y", Logger.Mode.Info.ToString());
}

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();
app.MapControllers();

// APP LEVANTADA
Logger.Write("Aplicación StartechML.WEB levantada correctamente", "Y", "Y", Logger.Mode.Info.ToString());

app.Run();

