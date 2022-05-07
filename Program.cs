using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
/*
1- Consumir uma lista de pokemons
    salvar: Name, Types, Abilities, Weight, Height, Sprites
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
                await GetPokemon(pokemonList[i]);
            }
            
        }

        private static async Task GetPokemon(string pokemonName){
            Console.WriteLine($"Requesting {pokemonName} from PokeAPI (https://pokeapi.co/api/v2/pokemon/{pokemonName.ToLower()})");
            var streamTask = client.GetStreamAsync($"https://pokeapi.co/api/v2/pokemon/{pokemonName.ToLower()}");
            // var pokemonInfo = await stringTask;
            Console.WriteLine("Pokemon info:");
            // Console.Write(pokemonInfo);
            var pokemon = await JsonSerializer.DeserializeAsync<Pokemon>(await streamTask);
        }
    }

    public class Pokemon{
        public string name {get; set;}
        string[] types;
        string[] abilities;
        public float weight {get; set;}
        public float height {get; set;}
        string[] sprites;
    }
}