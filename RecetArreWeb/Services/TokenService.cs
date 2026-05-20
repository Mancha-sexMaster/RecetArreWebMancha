using Microsoft.JSInterop;

namespace RecetArreWeb.Services
{
    public interface ITokenService
    {
        Task GuardarToken(string token, DateTime expiracion);
        Task<string?> ObtenerToken();
        Task<DateTime?> ObtenerExpiracionToken();
        Task<bool> EstaAutenticado();
        Task EliminarToken();
    }

    public class TokenService : ITokenService
    {
        private readonly IJSRuntime jSRuntime;
        private const string TOKEN_KEY = "authToken";
        private const string EXPIRACION_KEY = "tokenExpiracion";

        public TokenService(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime;
        }

        public async Task EliminarToken()
        {
            await jSRuntime.InvokeVoidAsync("localStorage.removeItem", TOKEN_KEY);
            await jSRuntime.InvokeVoidAsync("localStorage.removeItem", EXPIRACION_KEY);
        }

        public async Task<bool> EstaAutenticado()
        {
            var token  = await ObtenerToken();
            return !string.IsNullOrEmpty(token);
        }

        public async Task GuardarToken(string token, DateTime expiracion)
        {
            await jSRuntime.InvokeVoidAsync("localStorage.setItem", TOKEN_KEY, token);
            await jSRuntime.InvokeVoidAsync("localStorage.setItem", EXPIRACION_KEY, expiracion.ToString("o"));
        }

        public async Task<DateTime?> ObtenerExpiracionToken()
        {
            try
            {
                var expiracionStr = await jSRuntime.InvokeAsync<string>("localStorage.getItem", EXPIRACION_KEY);
                if (string.IsNullOrEmpty(expiracionStr))
                {
                    return null;
                }
                if (DateTime.TryParse(expiracionStr, out var expiracion))
                {
                    return expiracion;
                }
                return null;
            }
            catch
            {
                return null;
            }
            
        }

        public async Task<string?> ObtenerToken()
        {
            try
            {
                //1. Leer token de LocalStorage
                var token = await jSRuntime.InvokeAsync<string>("localStorage.getItem", TOKEN_KEY);

                //2. Si no existe el token, retornar null
                if (string.IsNullOrEmpty(token))
                {
                    return null;
                }

                //3. Verificar si el token ha expirado
                var expiracion = await ObtenerExpiracionToken();

                if (expiracion.HasValue && expiracion.Value < DateTime.UtcNow)
                {
                    //4. Si el token ha expirado, eliminarlo y retornar null
                    await EliminarToken();
                    return null;
                }
                return token;
            }
            catch
            {
                return null;
            }
        }
    }
}
