using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoCompleteTextBox.Demo.Model;
using AutoCompleteTextBox.Editors;
using Newtonsoft.Json;

namespace AutoCompleteTextBox.Demo.Providers
{
    public class StateSuggestionProvider : IComboSuggestionProvider
    {
        public IEnumerable<State> ListOfStates { get; set; }

        private static HttpClient _httpClient = new HttpClient();

        public State GetExactSuggestion(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return null;
            return
                ListOfStates
                    .FirstOrDefault(state => string.Equals(state.Name, filter, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<IEnumerable<State>> GetSuggestions(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return null;
            var result = await (await _httpClient.GetAsync("https://cat-fact.herokuapp.com/facts/random?animal_type=cat&amount=200")).Content.ReadAsStringAsync();
            var states = JsonConvert.DeserializeObject<List<State>>(result) ?? new List<State>();


            return
                states
                    .Where(state => state.Name.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) > -1)
                    .ToList();
        }

       async Task<IEnumerable> IComboSuggestionProvider.GetSuggestions(string filter)
        {
            return await GetSuggestions(filter);
        }

       Task<IEnumerable> IComboSuggestionProvider.GetFullCollection()
        {
            return Task.FromResult<IEnumerable>(ListOfStates.ToList());
        }

        public StateSuggestionProvider()
        {
            var states = StateFactory.CreateStateList();
            ListOfStates = states;
        }



    }
}