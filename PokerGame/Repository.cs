using Android.Content;
using Firebase;
using Firebase.Firestore;
using Java.Interop;
using Java.Util;
using PokerLib;
using System;
using System.Text.Json;

namespace PokerGame
{
    public class Repository
    {
        
        public static FirebaseFirestore db;

        //Game
        public static CollectionReference GamesCollection => db.Collection("Games");
        public static string gameId;
        public static DocumentReference GameDocument => GamesCollection.Document(gameId);

        //Users
        public static string UserName;
        public static CollectionReference UserCollection => db.Collection("Users");
        public static DocumentReference UserDocument => GamesCollection.Document(UserName);



        public static void Init(Context context)
        {
            db ??= GetDataBase(context);
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
        //Game
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

        //user
        public static MyUser GetUser(DocumentSnapshot document)
           => JsonSerializer.Deserialize<MyUser>(document.Get("data").ToString());

        public static void UploadUser(MyUser user)
        {
            var json = JsonSerializer.Serialize(user);
            var docref = UserCollection.Document(user.Name);
            HashMap map = new HashMap();
            map.Put("data", json);
            docref.Set(map);
        }

        public static void DeleteUser(string username)
        {
            UserCollection.Document(username).Delete();
        }

        public bool DoesDocumentExist( string documentName, QueryDocumentSnapshot query)
        {
            try
            {
                // Create a reference to the document
                DocumentReference docRef = UserCollection.Document(documentName);

                // Read the document
                DocumentSnapshot snapshot = query;

                // Check if the document exists
               return snapshot.Exists();
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

    }
}