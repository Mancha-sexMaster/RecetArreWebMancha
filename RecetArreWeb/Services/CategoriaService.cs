using RecetArreWeb.DTOs;
using System.Data;
using System.Net.Http.Json;

namespace RecetArreWeb.Services
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDto>> ObtenerTodas();
        Task<CategoriaDto?> ObtenerporId(int id);
        Task<CategoriaDto?> Crear(CategoriaCreacionDto categoria);
        Task<bool> Actualizar(int id, CategoriaModificacionDto categoria);
        Task<bool> Eliminar(int id);
    }
    public class CategoriaService : ICategoriaService
    {
        private readonly HttpClient httpClient;
        private const string endpoint = "api/Categorias";

        public CategoriaService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<CategoriaDto>> ObtenerTodas()
        {
            try
            {
                var categorias = await httpClient.GetFromJsonAsync<List<CategoriaDto>>(endpoint);
                return categorias ?? new List<CategoriaDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener categorias: {ex.Message}");
                return new List<CategoriaDto>();
            }
        }

        public async Task<CategoriaDto?> ObtenerporId(int id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<CategoriaDto>($"{endpoint}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener categoria {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<CategoriaDto?> Crear(CategoriaCreacionDto categoriaDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(endpoint, categoriaDto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CategoriaDto>();
                }
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al crear categorias: {error}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear categorias: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> Actualizar(int id, CategoriaModificacionDto categoria)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"{endpoint}/{id}", categoria);
                if(response.IsSuccessStatusCode)return true;
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erroto al actualizar categoria {id}: {error}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar categoria {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"{endpoint}/{id}");
                if (response.IsSuccessStatusCode) return true;
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erroto al actualizar categoria {id}: {error}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar categoria {id}: {ex.Message}");
                return false;
            }
        }
    }
}
