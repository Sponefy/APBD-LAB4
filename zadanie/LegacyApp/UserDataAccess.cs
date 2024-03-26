using System;
using System.Threading;

namespace LegacyApp
{
    /*
     * DO NOT CHANGE THIS FILE AT ALL
     * We assume that this is some kind of legacy code which we cannot modify
     */
    public static class UserDataAccess
    {
        /// <summary>
        /// This service is simulating saving user to remote database
        /// </summary>
        public static void AddUser(User user)
        {
            int randomWaitTime = new Random().Next(1000);
            Thread.Sleep(randomWaitTime);
            Console.WriteLine($"Added the user {user} successfully");
        }
    }
}
