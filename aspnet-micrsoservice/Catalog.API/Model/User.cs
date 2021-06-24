using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Model
{
    public class User : MongoDbEntity
    {
        [BsonElement("Name")]
        [MaxLength(50)]
        public string Name { get; set; }
        [BsonElement("Surname")]
        [MaxLength(50)]
        public string Surname { get; set; }

        [BsonElement("Age")]
        [Range(1, 150)]
        [BsonRepresentation(BsonType.Int32)]
        public int Age { get; set; }
    }
}
