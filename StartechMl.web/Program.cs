using StartechML.Core.Utils;

var builder = WebApplication.CreateBuilder(args);

// INICIALIZAR LOGGER (ANTES DE Build)
Logger.SetLogPath(Path.Combine(AppContext.BaseDirectory, "Log"));
Logger.SetLogLine(85);
Logger.Write("Inicio aplicación StartechML.WEB", "Y", "Y", Logger.Mode.Info.ToString());

builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});


var app = builder.Build();

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

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowAll");
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();
app.MapControllers();

// APP LEVANTADA
Logger.Write("Aplicación StartechML.WEB levantada correctamente", "Y", "Y", Logger.Mode.Info.ToString());

app.Run();

