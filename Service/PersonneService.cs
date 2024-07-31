using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using WPFModernVerticalMenu.Model;

namespace Test_api.Service
{
    public class PersonneService
    {
        private readonly string _apiBaseUrl;

        public PersonneService()
        {
            ///l'url de configuration
            _apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
        }
        /// <summary>
        /// Méthode de l'api qui permet de lister
        /// </summary>
        /// <returns></returns>
        public async Task<List<Personne>> ObtenirToutesLesPersonnes()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"{_apiBaseUrl}list.php");
                var responseString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<Personne>>(responseString);
            }
        }
        /// <summary>
        /// Méthode de l'api qui permet d'ajouter une personne
        /// </summary>
        /// <param name="personne">objet personne</param>
        /// <returns></returns>
        public async Task<bool> AjouterPersonne(Personne personne)
        {
            using (HttpClient client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "nom", personne.Nom },
                    { "prenom", personne.Prenom },
                    { "age", personne.Age.ToString() }
                };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync($"{_apiBaseUrl}create.php", content);
                var responseString = await response.Content.ReadAsStringAsync();

                dynamic result = JsonConvert.DeserializeObject(responseString);
                return result.success;
            }
        }
        /// <summary>
        /// Méthode de l'api qui permet de mettre à jour
        /// </summary>
        /// <param name="personne"></param>
        /// <returns></returns>
        public async Task<bool> MettreAJourPersonne(Personne personne)
        {
            using (HttpClient client = new HttpClient())
            {
                var values = new Dictionary<string, string>
        {
            { "id", personne.Id.ToString() },
            { "nom", personne.Nom },
            { "prenom", personne.Prenom },
            { "age", personne.Age.ToString() }
        };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync($"{_apiBaseUrl}update.php", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}, Content: {errorContent}");
                    return false;
                }

                var responseString = await response.Content.ReadAsStringAsync();

                try
                {
                    dynamic result = JsonConvert.DeserializeObject(responseString);
                    return result.success;
                }
                catch (JsonReaderException ex)
                {
                    Console.WriteLine($"JSON parse error: {ex.Message}");
                    Console.WriteLine($"Response content: {responseString}");
                    return false;
                }
            }
        }

        /// <summary>
        /// Méthode de l'api qui permet de supprimer
        /// </summary>
        /// <param name="id">l'objet passer en paramétre</param>
        /// <returns></returns>
        public async Task<bool> SupprimerPersonne(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var values = new Dictionary<string, string>
        {
            { "id", id.ToString() }
        };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync($"{_apiBaseUrl}delete.php", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}, Content: {errorContent}");
                    return false;
                }

                var responseString = await response.Content.ReadAsStringAsync();

                try
                {
                    dynamic result = JsonConvert.DeserializeObject(responseString);
                    return result.success;
                }
                catch (JsonReaderException ex)
                {
                    Console.WriteLine($"JSON parse error: {ex.Message}");
                    Console.WriteLine($"Response content: {responseString}");
                    return false;
                }
            }
        }

        /// <summary>
        /// Méthode de l'api qui permet d'obtenir la personne par son id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Personne> ObtenirPersonneParId(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"{_apiBaseUrl}details.php?id={id}");
                var responseString = await response.Content.ReadAsStringAsync();

                dynamic result = JsonConvert.DeserializeObject(responseString);
                return JsonConvert.DeserializeObject<Personne>(result.resultat.ToString());
            }
        }
    }
}
