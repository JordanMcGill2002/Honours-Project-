namespace Films.Models
{
    public interface IFilmsDatabaseSettings
    {
        string FilmsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

    public class FilmsDatabaseSettings : IFilmsDatabaseSettings
    {
        public string FilmsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
