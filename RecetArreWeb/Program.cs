using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RecetArreWeb;
using RecetArreWeb.Auth;
using RecetArreWeb.Handlers;
using RecetArreWeb.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//Configurar HTTP Client con handler de Autorización
//TODO Descomentar
builder.Services.AddScoped<AuthorizationMessageHandler>();

builder.Services.AddScoped<HttpClient>(sp =>
    {
        var handler = sp.GetRequiredService<AuthorizationMessageHandler>();
        handler.InnerHandler = new HttpClientHandler();

        return new HttpClient(handler)
        {
            BaseAddress = new Uri("https://RecetMancha.somee.com/")
            //BaseAddress = new Uri("https://localhost:XXXX/")
            
        };
    }
);

//Registrar servicios
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IIngredienteService, IngredienteService>();
builder.Services.AddScoped<IRecetaService, RecetaService>();
builder.Services.AddScoped<IComentarioService, ComentarioService>();
builder.Services.AddScoped<IRatingService, RatingService>();
//TODO: Todos los demas servicios, por ejemplo ICategoriaService, IIngredienteService

//Configurar autenticación
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();


await builder.Build().RunAsync();
