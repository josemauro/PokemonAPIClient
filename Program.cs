using System;
using System.Threading.Tasks;
using System.Net.Http;
using Func = System.Text.Json;

using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            string[] pokemonList = { "Charmander", "Squirtle", "Caterpie", "Weedle", "Pidgey", "Pidgeotto", "Rattata", "Spearow", "Fearow", "Arbok", "Pikachu", "Sandshrew"};
            using StreamWriter file = new("PokemonsInfoList.txt");

            file.Write('[');
            for (int i=0; i< pokemonList.Length; i++){
                if (i>0) {file.Write(",\n");}
                var pokemonInfo = "";
                pokemonInfo = await GetPokemonInfo(pokemonList[i]);
                file.Write(pokemonInfo);
            }
            file.Write(']');
            Console.WriteLine("Done!");

            
        }

        private static async Task<string> GetPokemonInfo(string pokemonName){
            Console.WriteLine($"Requesting {pokemonName} from PokeAPI (https://pokeapi.co/api/v2/pokemon/{pokemonName.ToLower()})");
            var stringTask = client.GetStringAsync($"https://pokeapi.co/api/v2/pokemon/{pokemonName.ToLower()}");
            // Get JSON with the information about the pokemon
            var pokemonInfo = JObject.Parse(await stringTask);
            Dictionary<string, JToken> pokemonInfoDict = new Dictionary<string, JToken>();
            pokemonInfoDict.Add("name", pokemonInfo["name"]);
            pokemonInfoDict.Add("weight", pokemonInfo["weight"]);
            pokemonInfoDict.Add("height", pokemonInfo["height"]);
            
            // Get types of the pokemon
            List<JToken> pokemonTypes = new List<JToken>();
            foreach (JToken pokemonType in pokemonInfo["types"]){
                pokemonTypes.Add(pokemonType["type"]["name"]);    
            }
            pokemonInfoDict.Add("type", JToken.FromObject(pokemonTypes));

            // Get the abilities of the pokemon
            List<JToken> pokemonAbilities = new List<JToken>();
            foreach (JToken pokemonAbility in pokemonInfo["abilities"]){
                pokemonAbilities.Add(pokemonAbility["ability"]["name"]);    
            }
            pokemonInfoDict.Add("abilities", JToken.FromObject(pokemonAbilities));


            // Get the front_default sprite of the pokemon
            pokemonInfoDict.Add("sprite", pokemonInfo["sprites"]["front_default"]);

            var pokemonInfoResult = JsonConvert.SerializeObject(pokemonInfoDict, Formatting.Indented);
            return pokemonInfoResult;
        }
    }
}