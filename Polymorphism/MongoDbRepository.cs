using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Polymorphism;

public class MongoDbRepository
{
    private readonly IMongoCollection<PagePreference> _preferencesCollection;

    public MongoDbRepository(string connectionString, string databaseName, string collectionName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _preferencesCollection = database.GetCollection<PagePreference>(collectionName);
    }

    public async Task Insert(PagePreference pagePreference)
    {
        await _preferencesCollection.InsertOneAsync(pagePreference);
    }

    public async Task<PagePreference?> Get(string id)
    {
        return await _preferencesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public void RegisterClassMaps()
    {
        var pack = new ConventionPack
        {
            new IgnoreExtraElementsConvention(true),
            new EnumRepresentationConvention(BsonType.String),
        };

        ConventionRegistry.Register("CustomConventions", pack, t => true);

        BsonClassMap.RegisterClassMap<WidgetPreferenceBase>(
            cm =>
            {
                cm.AutoMap();
                cm.SetIsRootClass(true);
                cm.SetDiscriminator("Type");
                cm.MapMember(c => c.Type).SetSerializer(new EnumSerializer<WidgetPreferenceType>(BsonType.String));
            });
    }
}