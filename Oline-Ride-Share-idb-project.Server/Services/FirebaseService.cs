//using FirebaseAdmin;
//using FirebaseAdmin.Messaging;
//using Google.Apis.Auth.OAuth2;
//using System;
//using System.Threading.Tasks;

//namespace Oline_Ride_Share_idb_project.Server.Services
//{
//    public class FirebaseService
//    {
//        // Firebase Admin SDK ইনিশিয়ালাইজ
//        public FirebaseService()
//        {
//            // Firebase Admin SDK সার্ভিস অ্যাকাউন্ট দিয়ে ইনিশিয়ালাইজ করা
//            try
//            {
//                var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Utilities", "your_service_account_json_file.json"); // সঠিক JSON ফাইলের পাথ দিন
//                FirebaseApp.Create(new AppOptions()
//                {
//                    Credential = GoogleCredential.FromFile(jsonFilePath)
//                });

//                Console.WriteLine("Firebase Initialized successfully.");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error initializing Firebase: {ex.Message}");
//            }
//        }

//        // পুশ নোটিফিকেশন পাঠানোর জন্য মেথড
//        public async Task SendPushNotification(string token, string title, string message)
//        {
//            var messageToSend = new Message()
//            {
//                Token = token, // FCM টোকেন
//                Notification = new Notification()
//                {
//                    Title = title,
//                    Body = message
//                }
//            };

//            try
//            {
//                // Firebase Messaging সিস্টেমে পাঠানোর জন্য
//                string response = await FirebaseMessaging.DefaultInstance.SendAsync(messageToSend);
//                Console.WriteLine($"Successfully sent message: {response}");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error sending message: {ex.Message}");
//            }
//        }
//    }
//}
