using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Footballers.DataProcessor.ExportDto;
using Newtonsoft.Json;

namespace Footballers.DataProcessor
{
    using System;

    using Data;

    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Coaches");
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCoachDto[]), xmlRoot);

            using StringWriter sw = new StringWriter(sb);

            var coaches = context.Coaches
                .ToArray()
                .Where(c => c.Footballers.Any())
                .Select(c => new ExportCoachDto()
                {
                    FootballersCount = c.Footballers.Count(),
                    CoachName = c.Name,
                    Footballers = c.Footballers
                        .Select(f => new ExportCoachFootballerDto()
                        {
                            Name = f.Name,
                            Position = f.PositionType.ToString()
                        })
                        .OrderBy(f => f.Name)
                        .ToArray()
                })
                .OrderByDescending(c => c.Footballers.Length)
                .ThenBy(c => c.CoachName)
                .ToArray();

            xmlSerializer.Serialize(sw,coaches,namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teamsWithFootballers = context.Teams
                .ToArray()
                .Where(t => t.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                .Select(t => new
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                        .Where(tf => tf.Footballer.ContractStartDate >= date)
                        .Select(tf => tf.Footballer)
                        .OrderByDescending(f => f.ContractEndDate)
                        .ThenBy(f => f.Name)
                        .Select(f => new
                        {
                            FootballerName = f.Name,
                            ContractStartDate = f.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                            ContractEndDate = f.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                            BestSkillType = f.BestSkillType.ToString(),
                            PositionType = f.PositionType.ToString(),
                        })
                        
                        .ToArray()
                })
                .OrderByDescending(t => t.Footballers.Length)
                .ThenBy(t => t.Name)
                .Take(5)
                .ToArray();

            string json = JsonConvert.SerializeObject(teamsWithFootballers, Formatting.Indented);

            return json;
        }
    }
}
