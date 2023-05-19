using Android.Content;
using Firebase;
using Firebase.Firestore;
using Java.Util;
using PokerLib;
using System.Text.Json;

namespace PokerGame
{
    public class Repository
    {
        public static FirebaseFirestore db;
        public static CollectionReference GamesCollection => db.Collection("Games");
        public static string gameId;
        public static DocumentReference GameDocument => GamesCollection.Document(gameId);

        public static void Init(Context context)
        {
            db = GetDataBase(context);
        }

        static FirebaseFirestore GetDataBase(Context context)
        {
            // info from "google-services.json"
            var options = new FirebaseOptions.Builder()
            .SetProjectId("pokergame2")
            .SetApplicationId("pokergame2")
            .SetApiKey("AIzaSyCASOGnHrcqa5YDEXzY4lPqENz5Fk6CNcc")
            .SetStorageBucket("pokergame2.appspot.com")
            .Build();
            try
            {
                var app = FirebaseApp.InitializeApp(context, options);
                return FirebaseFirestore.GetInstance(app);
            }
            catch
            {
                var app = FirebaseApp.GetApps(context);
                return FirebaseFirestore.GetInstance(app[0]);
            }
        }

        public static Game GetGame(DocumentSnapshot document)
            => JsonSerializer.Deserialize<Game>(document.Get("data").ToString());

        public static void UploadGame(Game game)
        {
            var json = JsonSerializer.Serialize(game);
            var docref = GamesCollection.Document(game.Id.ToString());
            HashMap map = new HashMap();
            map.Put("data", json);
            docref.Set(map);
        }

        public static void DeleteGame(string gameId)
        {
            GamesCollection.Document(gameId).Delete();
        }
    }
}