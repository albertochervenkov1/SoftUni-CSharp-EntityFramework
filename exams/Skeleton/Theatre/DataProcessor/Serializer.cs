using System.Linq;
using Newtonsoft.Json;

namespace Theatre.DataProcessor
{
    using System;
    using Theatre.Data;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var bestTheaters = context.Theatres
                .ToArray()
                //.Where(t => t.NumberOfHalls >= numbersOfHalls)
                .Select(t => new
                {
                    Name = t.Name
                })
                .ToArray();

            string json = JsonConvert.SerializeObject(bestTheaters, Formatting.Indented);
            return json;
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            throw new NotImplementedException();
        }
    }
}
