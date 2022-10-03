using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Theatre.Data.Models;
using Theatre.Data.Models.Enums;
using Theatre.DataProcessor.ImportDto;

namespace Theatre.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Theatre.Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Plays");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPlayDto[]), xmlRoot);

            using StringReader sr = new StringReader(xmlString);

            ImportPlayDto[] playDtos = (ImportPlayDto[])xmlSerializer.Deserialize(sr);
            HashSet<Play> plays = new HashSet<Play>();

            foreach (var playDto in playDtos)
            {
                if (!IsValid(playDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isEnumValid = Enum.TryParse<Genre>(playDto.Genre, out Genre genre);
                if (!isEnumValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isDurationValid = TimeSpan.TryParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture,
                    TimeSpanStyles.None, out TimeSpan duration);
                

                if (duration.Hours<1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }//possible error

                Play play = new Play()
                {
                    Title = playDto.Title,
                    Duration = duration,
                    Rating = float.Parse(playDto.Rating),
                    Genre = Enum.Parse<Genre>(playDto.Genre),
                    Description = playDto.Description,
                    Screenwriter = playDto.Screenwriter
                };
                plays.Add(play);
                sb.AppendLine(String.Format(SuccessfulImportPlay, play.Title, play.Genre, play.Rating));
            }
            context.Plays.AddRange(plays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }//genres

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Casts");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCastDto[]), xmlRoot);

            using StringReader sr = new StringReader(xmlString);

            ImportCastDto[] castDtos = (ImportCastDto[])xmlSerializer.Deserialize(sr);
            HashSet<Cast> casts = new HashSet<Cast>();

            foreach (var castDto in castDtos)
            {
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Cast cast = new Cast()
                {
                    FullName = castDto.FullName,
                    IsMainCharacter = bool.Parse(castDto.IsMainCharacter),
                    PhoneNumber = castDto.PhoneNumber,
                    PlayId = castDto.PlayId
                };
                string charac = "";
                if (cast.IsMainCharacter==true)
                {
                    charac = "main";
                }
                else
                {
                    charac = "lesser";
                }
                casts.Add(cast);
                sb.AppendLine(String.Format(SuccessfulImportActor, cast.FullName, charac));
            }

            context.Casts.AddRange(casts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportTheatreDto[] theatreDtos = JsonConvert.DeserializeObject<ImportTheatreDto[]>(jsonString);

            HashSet<Data.Models.Theatre> theatres = new HashSet<Data.Models.Theatre>();

            foreach (var theatreDto in theatreDtos)
            {
                if (!IsValid(theatreDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Data.Models.Theatre theatre=new Data.Models.Theatre()
                {
                    Name = theatreDto.Name,
                    NumberOfHalls = theatreDto.NumberOfHalls,
                    Director = theatreDto.Director
                };
                HashSet<Ticket> tickets=new HashSet<Ticket>();
                foreach (var dtoTicket in theatreDto.Tickets)
                {
                    if (!IsValid(dtoTicket))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }//possible error
                    
                    Ticket ticket=new Ticket()
                    {
                        Price = dtoTicket.Price,
                        RowNumber = dtoTicket.RowNumber,
                        PlayId = dtoTicket.PlayId
                    };
                    tickets.Add(ticket);
                }

                theatre.Tickets = tickets;
                sb.AppendLine(String.Format(SuccessfulImportTheatre, theatre.Name, tickets.Count));
            }
            context.Theatres.AddRange(theatres);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
