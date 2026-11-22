using System.Threading.Tasks;

namespace UniMarket.Logica
{
    public interface IAsistenteService
    {
        bool Configurado { get; }
        Task<string> PreguntarAsync(string pregunta);
    }
}