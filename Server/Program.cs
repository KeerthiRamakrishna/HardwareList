using Radzen;
using HardwareManagement.Server.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024).AddInteractiveWebAssemblyComponents();
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddScoped<HardwareManagement.Server.FocusDBService>();
builder.Services.AddDbContext<HardwareManagement.Server.Data.FocusDBContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("FocusDBConnection"));
});
builder.Services.AddControllers().AddOData(opt =>
{
    var oDataBuilderFocusDB = new ODataConventionModelBuilder();
    oDataBuilderFocusDB.EntitySet<HardwareManagement.Server.Models.FocusDB.Architecture>("Architectures");
    oDataBuilderFocusDB.EntitySet<HardwareManagement.Server.Models.FocusDB.AutosarVersion>("AutosarVersions");
    oDataBuilderFocusDB.EntitySet<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus>("AvailabilityStatuses");
    oDataBuilderFocusDB.EntitySet<HardwareManagement.Server.Models.FocusDB.CompilerVendor>("CompilerVendors");
    oDataBuilderFocusDB.EntitySet<HardwareManagement.Server.Models.FocusDB.CompilerVersion>("CompilerVersions");
    oDataBuilderFocusDB.EntitySet<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative>("MicroControllerDerivatives");
    oDataBuilderFocusDB.EntitySet<HardwareManagement.Server.Models.FocusDB.MicroController>("MicroControllers");
    oDataBuilderFocusDB.EntitySet<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative>("MicroControllerSubDerivatives");
    oDataBuilderFocusDB.EntitySet<HardwareManagement.Server.Models.FocusDB.SiliconVendor>("SiliconVendors");
    oDataBuilderFocusDB.EntitySet<HardwareManagement.Server.Models.FocusDB.TresosAcg>("TresosAcgs");
    oDataBuilderFocusDB.EntitySet<HardwareManagement.Server.Models.FocusDB.TresosAutoCore>("TresosAutoCores");
    oDataBuilderFocusDB.EntitySet<HardwareManagement.Server.Models.FocusDB.TresosSafetyO>("TresosSafetyOs");
    opt.AddRouteComponents("odata/FocusDB", oDataBuilderFocusDB.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<HardwareManagement.Client.FocusDBService>();
var app = builder.Build();
app.MapDefaultEndpoints();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode().AddInteractiveWebAssemblyRenderMode().AddAdditionalAssemblies(typeof(HardwareManagement.Client._Imports).Assembly);
app.Run();