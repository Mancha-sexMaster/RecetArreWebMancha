using System.Net.Http.Json;
using RecetArreWeb.DTOs;

namespace RecetArreWeb.Services
{
    public interface IRatingService
    {
        Task<List<RatingDTO>> ObtenerPorReceta(int recetaId);
        Task<RatingDTO?> ObtenerPorUsuario(int recetaId, string usuarioId);
        Task<RatingDTO?> Crear(RatingCreacionDTO ratingDto);
        Task<RatingDTO?> Actualizar(RatingCreacionDTO ratingDto);
    }

    public class RatingService : IRatingService
    {
        private readonly HttpClient httpClient;
        private const string endpoint = "api/Rating";

        public RatingService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<RatingDTO>> ObtenerPorReceta(int recetaId)
        {
            try
            {
                var ratings = await httpClient.GetFromJsonAsync<List<RatingDTO>>($"{endpoint}/rating/{recetaId}");
                return ratings ?? new List<RatingDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener ratings: {ex.Message}");
                return new List<RatingDTO>();
            }
        }

        public async Task<RatingDTO?> ObtenerPorUsuario(int recetaId, string usuarioId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<RatingDTO>($"{endpoint}/rating/{recetaId}/usuario/{usuarioId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener rating del usuario: {ex.Message}");
                return null;
            }
        }

        public async Task<RatingDTO?> Crear(RatingCreacionDTO ratingDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(endpoint, ratingDto);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RatingDTO>();
                }

                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al crear rating: {error}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear rating: {ex.Message}");
                return null;
            }
        }

        public async Task<RatingDTO?> Actualizar(RatingCreacionDTO ratingDto)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Patch, endpoint)
                {
                    Content = JsonContent.Create(ratingDto)
                };

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RatingDTO>();
                }

                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al actualizar rating: {error}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar rating: {ex.Message}");
                return null;
            }
        }
    }
}
