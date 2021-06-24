namespace Catalog.API.Data
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public const string ConnectionStringValue = nameof(ConnectionString);
        public const string DatabaseNameValue = nameof(DatabaseName);
    }
}
