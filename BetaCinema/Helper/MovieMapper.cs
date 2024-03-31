using BetaCinema.Entity;
using BetaCinema.EntityDTO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IO;
using System.Xml.Linq;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace BetaCinema.Helper
{
    public static class MovieMapper
    {
        public static List<MovieDTO> Mapper(List<Movie> movie)
        {
            var movieDTO = new List<MovieDTO>();
            foreach(var i in movie)
            {
                int x = (i.Schedule == null || i.Schedule.All(x => x.Tickets == null)) ? 0 : i.Schedule.Sum(x => x.Tickets.Count());
                MovieDTO y = new MovieDTO()
                {
                    MovieId = i.MovieId,
                    MovieDuration = i.MovieDuration,
                    Description = i.Description,
                    Director = i.Director,
                    MovieTypeId = i.MovieTypeId,
                    Name = i.Name,
                    RateId = i.RateId,
                    Trailer = i.Trailer,
                    View = x
                };
                movieDTO.Add(y);
            }
            return movieDTO;
        }
        public static List<SeatDTO> Mapper(List<Seat> seat)
        {
            var seatDTO = new List<SeatDTO>();
            foreach(var i in seat)
            {
                SeatDTO x = new SeatDTO()
                {
                    SeatId = i.SeatId,
                    Number = i.Number,
                    Line = i.Line,
                    SeatStatus = i.SeatStatus.NameStatus.ToString(),
                    SeatType = i.SeatType.NameType.ToString()
                };
                seatDTO.Add(x);
            }
            return seatDTO;
        }
        public static List<CinemaDTO> Mapper(List<Cinema> cinema)
        {
            var cinemaDTO = new List<CinemaDTO>();
            foreach(var i in cinema)
            {
                CinemaDTO x = new CinemaDTO()
                {
                    CinemaId = i.CinemaId,
                    Address = i.Address,
                    Descriptiom = i.Descriptiom,
                    Code = i.Code,
                    NameOfCinema = i.NameOfCinema,
                    IsActive = i.IsActive
                };
                cinemaDTO.Add(x);
            }
            return cinemaDTO;
        }
    }
}
