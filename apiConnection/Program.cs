using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace apiConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            const string BASE_URL = "https://swapi.co/api/";
            const string PLANETS = "planets/";
            string url = BASE_URL + PLANETS;
            //string url = "https://swapi.co/api/planets/";

            while (true)
            {
                JObject planetdata = CallRestMethod(url);

                JArray planets = (JArray)planetdata["results"];
                foreach (JObject planet in planets)
                {
                    Console.WriteLine(planet["name"]);
                    JArray films = (JArray)planet["films"];
                    foreach (JValue film in films)
                    {
                        Console.WriteLine(CallRestMethod(film.ToString())["title"]);
                    }
                    Console.WriteLine();
                }

                if (planetdata["next"].Type != JTokenType.Null)
                {
                    url = planetdata["next"].ToString();
                }
                else
                {
                    break;
                }
            }
            Console.ReadLine();
        }

        static JObject CallRestMethod(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader responseStream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                return JObject.Parse(responseStream.ReadToEnd());
            }
            catch (Exception e)
            {
                return JObject.Parse($"{{'Error':'{e.Message}'}}");
            }
        }
    }
}
