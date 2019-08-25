using System;
using Server.Main.objects;
using Server.Utils;

namespace Server.Main.managers
{
    class ConsoleManager
    {
        public static ConsoleManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ConsoleManager();
                }

                return _instance;
            }
        }

        private static ConsoleManager _instance;

        /// <summary>
        /// Updating the controller update time
        /// </summary>
        /// <param name="controllerKey">Key</param>
        public void UpdateControllerTimes(string controllerKey)
        {
            var controller = GetTimeStatistics(controllerKey);
            controller.UpdateTime = DateTime.Now;
        }

        /// <summary>
        /// Creating a new instance for console statistics which will have update times and MS of controller update
        /// </summary>
        /// <param name="controllerKey">Key</param>
        /// <exception cref="ArgumentException">If something went wrong during adding or found a duplicate instance</exception>
        public void CreateControlledInstance(string controllerKey)
        {
            if (Global.ConsoleStatistics.UpdateTimes.ContainsKey(controllerKey) || !Global.ConsoleStatistics.UpdateTimes.TryAdd(controllerKey, new UpdateTimeStatistic()))
            {
                Out.QuickLog("Something went wrong when trying to add a time statistic instance");
                throw new ArgumentException("Key with same value already exists");
            }
        }

        /// <summary>
        /// Removing the console statistic instance from dictionary
        /// </summary>
        /// <param name="controllerKey">Key</param>
        /// <exception cref="ArgumentException">If something went wrong during remove</exception>
        public void RemoveControllerInstance(string controllerKey)
        {
            if (!Global.ConsoleStatistics.UpdateTimes.ContainsKey(controllerKey) || !Global.ConsoleStatistics.UpdateTimes.Remove(controllerKey))
            {
                Out.QuickLog("Something went wrong when trying to remove a time statistic instance");
                throw new ArgumentException("Key with same value doesn't exists");
            }
        }

        /// <summary>
        /// Getting time since last update
        /// </summary>
        /// <param name="controllerKey">Key</param>
        /// <returns>Timespan between now and last update</returns>
        public TimeSpan GetTimeSinceLastUpdate(string controllerKey)
        {
            var controller = GetTimeStatistics(controllerKey);
            return DateTime.Now - controller.UpdateTime;
        }

        /// <summary>
        /// Getting the time statistics without having repetitive code hanging around
        /// </summary>
        /// <param name="controllerKey">Key</param>
        /// <returns>Instance of time statistics</returns>
        /// <exception cref="ArgumentException">Not found in dictionary</exception>
        private UpdateTimeStatistic GetTimeStatistics(string controllerKey)
        {
            if (!Global.ConsoleStatistics.UpdateTimes.ContainsKey(controllerKey))
            {
                Out.QuickLog("The specified time statistic instance doesn't exist");
                throw new ArgumentException("Time statistic controller key doesn't exist in dictionary");
            }

            var controller = Global.ConsoleStatistics.UpdateTimes[controllerKey];
            return controller;
        }
    }
}