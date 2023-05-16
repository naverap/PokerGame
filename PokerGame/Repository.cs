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

        public static void Init(Context context)
        {
            db = GetDataBase(context);
        }

        public static FirebaseFirestore GetDataBase(Context context)
        {
            FirebaseFirestore db;
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
                db = FirebaseFirestore.GetInstance(app);
                return db;
            }
            catch
            {
                var app = FirebaseApp.GetApps(context);
                db = FirebaseFirestore.GetInstance(app[0]);
                return db;
            }
        }

        //public static Game LoadGame()
        //{
        //    GamesCollection.Get().AddOnSuccessListener(OnSuccess)
        //}

        public static void OnSuccess(Java.Lang.Object result)
        {
            //var snapshot = (QuerySnapshot)result;
            //Card c;
            //foreach (var doc in snapshot.Documents)
            //{
            //    c = new Card((CardSuit)(int)doc.Get("P1Card1Suit"), (CardValue)(int)doc.Get("P1Card1Value"));
            //    Console.WriteLine(c.ToString());
            //}
        }

        public static void UploadGame(Game game)
        {
            var json = JsonSerializer.Serialize(game);
            var docref = GamesCollection.Document();
            HashMap map = new HashMap();
            map.Put("data", json);
            docref.Set(map);

            //    Live.db.Collection("Cards").Document("id").Delete();
        }
    }
}