using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
/*
1- Consumir uma lista de pokemons
2- Realizar consultas simultaneas
3- Fazer download de imagem de pokemon
*/
namespace PokemonAPIClient
{
    class PokeApi {
    private static readonly HttpClient client = new HttpClient();

    static async Task Main() {
        Console.WriteLine("\n   Welcome to PokeAPI!!!!!\n\n────────▄███████████▄────────|\n─────▄███▓▓▓▓▓▓▓▓▓▓▓███▄─────\n────███▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓███────\n───██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██───\n──██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██──\n─██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██─\n██▓▓▓▓▓▓▓▓▓███████▓▓▓▓▓▓▓▓▓██\n██▓▓▓▓▓▓▓▓██░░░░░██▓▓▓▓▓▓▓▓██\n██▓▓▓▓▓▓▓██░░███░░██▓▓▓▓▓▓▓██\n███████████░░███░░███████████\n██░░░░░░░██░░███░░██░░░░░░░██\n██░░░░░░░░██░░░░░██░░░░░░░░██\n██░░░░░░░░░███████░░░░░░░░░██\n─██░░░░░░░░░░░░░░░░░░░░░░░██─\n──██░░░░░░░░░░░░░░░░░░░░░██──\n───██░░░░░░░░░░░░░░░░░░░██───\n────███░░░░░░░░░░░░░░░███────\n─────▀███░░░░░░░░░░░███▀─────\n────────▀███████████▀────────");
        // string[] pokemonList = { "Charmander", "Squirtle", "Caterpie", "Weedle", "Pidgey", "Pidgeotto", "Rattata", "Spearow", "Fearow", "Arbok", "Pikachu", "Sandshrew"};
        string[] pokemonList = {"Ditto"};
        for (int i=0; i< pokemonList.Length; i++){
            Console.WriteLine("Retrieving {0} from PokeAPI..", pokemonList[i]);
            Task pokemonInfo = await GetPokemon(pokemonList[i]);
        }
        
    }

    private static async Task GetPokemon(string pokemon){
        Console.WriteLine("Requesting {0} from PokeAPI..", pokemon);
        var stringTask = client.GetStringAsync($"https://pokeapi.co/api/v2/pokemon/{pokemon.ToLower()}");

        await stringTask;
    }

    }
}