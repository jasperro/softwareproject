using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Timers;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Types;
using SoftwareProject.ViewModels;

namespace SoftwareProject.Models
{
    /// <summary>
    /// Timekeeping for application state, current date and time for calculations
    /// </summary>
    public class TimekeepingModel : ReactiveObject
    {
        public IObservable<long> Timer;

        public TimekeepingModel()
        {
            Timer = Observable.Timer(DateTimeOffset.Now, UpdateFrequency);
            Timer.Subscribe(_ => DoTick(TimeStep));
            //this.WhenAny(x => x.UpdateFrequency, s =>
            //{
            //    Console.WriteLine(UpdateFrequency);
            //    return Timer = Observable.Timer(DateTimeOffset.Now, s.Value);
            //});
        }

        private void DoTick(TimeSpan timeSpan)
        {
            // Forward current time with the timespan
            CurrentTime = CurrentTime.Add(timeSpan);
            Console.WriteLine(CurrentTime.ToShortDateString());

            // TODO: All code that needs to be updated every tick

            foreach (Stock stock in Globals.AvailableStocks)
            {
                stock.UpdateToTime(CurrentTime);
            }

            // Update all Investments returns and calculate based on algorithms the strategies for the next tick
            foreach (Investment investment in MainWindowViewModel.User.UserInvestmentPortfolio)
            {
                // Because stock data was just updated, the algorithms will be reapplied on newest stock data

                // This will apply the algorithms
                //investment.ApplyAlgorithms()

                // After this, it is still unknown how things will work.
                // It is possible to make everything a reactive property on the new state,
                // or a method that will calculate all the data in one step.
                //investment.UpdateToStock()
            }
        }
        /// <summary>
        /// The current time of the primary simulation,
        /// the point where we can look at profits from historical data
        /// </summary>
        [Reactive]
        public DateTime CurrentTime { get; set; } = DateTime.ParseExact("20-Nov-2021", "dd-MMM-yyyy", null);

        /// <summary>
        /// The speed at which time is moving (How often is a tick?). 1.0 is normal speed (one second per second)
        /// </summary>
        [Reactive]
        public TimeSpan UpdateFrequency { get; set; } = TimeSpan.FromSeconds(1);

        /// <summary>
        /// How much time in seconds has passed since the last update timer tick
        /// </summary>
        [Reactive]
        public TimeSpan TimeStep { get; set; } = TimeSpan.FromDays(1);
    }
}
