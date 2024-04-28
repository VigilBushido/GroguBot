using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GroguBot.config
{
    public class JSONReader
    {
        public string Token { get; set; } = string.Empty;
        public string Prefix { get; set; } = string.Empty;

        public async Task ReadJSON()
        {
            using (StreamReader sr = new StreamReader("config/config.json"))
            {
                string json = await sr.ReadToEndAsync();
                JSONStructure data = JsonConvert.DeserializeObject<JSONStructure>(json) ?? null!;

                this.Token = data.token;
                this.Prefix = data.prefix;
            }
        }
    }

    internal sealed class JSONStructure
    {
        public string token { get; set; } = string.Empty;
        public string prefix { get; set; } = string.Empty;
    }
}