using System;
using System.Threading.Tasks;
using System.Net.Http;
using Func = System.Text.Json;

using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PokemonAPIClient
{
    class PokeApi {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] argv) {
            Console.WriteLine("\n   Welcome to PokeAPI!!!!!\n\n────────▄███████████▄────────|\n─────▄███▓▓▓▓▓▓▓▓▓▓▓███▄─────\n────███▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓███────\n───██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██───\n──██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██──\n─██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██─\n██▓▓▓▓▓▓▓▓▓███████▓▓▓▓▓▓▓▓▓██\n██▓▓▓▓▓▓▓▓██░░░░░██▓▓▓▓▓▓▓▓██\n██▓▓▓▓▓▓▓██░░███░░██▓▓▓▓▓▓▓██\n███████████░░███░░███████████\n██░░░░░░░██░░███░░██░░░░░░░██\n██░░░░░░░░██░░░░░██░░░░░░░░██\n██░░░░░░░░░███████░░░░░░░░░██\n─██░░░░░░░░░░░░░░░░░░░░░░░██─\n──██░░░░░░░░░░░░░░░░░░░░░██──\n───██░░░░░░░░░░░░░░░░░░░██───\n────███░░░░░░░░░░░░░░░███────\n─────▀███░░░░░░░░░░░███▀─────\n────────▀███████████▀────────");
            string[] pokemonList = { "Charmander", "Squirtle", "Caterpie", "Weedle", "Pidgey", "Pidgeotto", "Rattata", "Spearow", "Fearow", "Arbok", "Pikachu", "Sandshrew"};
            using StreamWriter file = new("PokemonsInfoList.txt");
            Boolean parallel = false;
            try{ 
                if (argv[0].Equals("--parallel")) {
                    parallel= true;
                }
            }catch (System.IndexOutOfRangeException){
                parallel= false;
            }

            file.Write('[');

            if (parallel==false){
                Console.WriteLine("\nExecuting requests...\n\n");
                for (int i=0; i< pokemonList.Length; i++){
                    if (i>0) {file.Write(",\n");}
                    var pokemonInfo = await GetPokemonInfo(pokemonList[i]);
                    file.Write(pokemonInfo);
                }
            } else{
                Console.WriteLine("\nExecuting requests in parallel...\n\n");
                // batchSize defines how much requests will be made in parallel using Task
                int batchSize = 5;
                int numberOfBatches = (int)Math.Ceiling((double)pokemonList.Length / batchSize);
                var pokemons = new List<String>();
                    for(int i = 0; i < numberOfBatches; i++){
                        var currentIds = pokemonList.Skip(i * batchSize).Take(batchSize);
                        var tasks = currentIds.Select(id =>  GetPokemonInfo(id));
                        pokemons.AddRange(await Task.WhenAll(tasks));
                    }

                // write the pokemons in output file
                int cont = 0;
                foreach (string pokemon in pokemons){
                    if (cont > 0) {file.Write(",\n");} 
                    cont ++;
                    file.Write(pokemon);
                }
            }
            file.Write(']');
            Console.WriteLine("Done!!!!");

            
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