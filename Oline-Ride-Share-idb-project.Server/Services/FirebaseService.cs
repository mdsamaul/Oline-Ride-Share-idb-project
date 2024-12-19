//using FirebaseAdmin;
//using FirebaseAdmin.Messaging;
//using Google.Apis.Auth.OAuth2;
//using System;
//using System.Threading.Tasks;

//namespace Oline_Ride_Share_idb_project.Server.Services
//{
//    public class FirebaseService
//    {
//        public FirebaseService()
//        {
//           
//            try
//            {
//                var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Utilities", "your_service_account_json_file.json");
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
//        public async Task SendPushNotification(string token, string title, string message)
//        {
//            var messageToSend = new Message()
//            {
//                Token = token, 
//                Notification = new Notification()
//                {
//                    Title = title,
//                    Body = message
//                }
//            };

//            try
//            {             
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
