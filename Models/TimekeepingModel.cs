using System;
using System.Reactive.Linq;
using System.Timers;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Types;
using static SoftwareProject.Globals;
using static SoftwareProject.ViewModels.MainWindowViewModel;

namespace SoftwareProject.Models
{
    /// <summary>
    /// Timekeeping for application state, current date and time for calculations
    /// </summary>
    public class TimekeepingModel : ReactiveObject
    {
        public readonly IObservable<DateTime> ObservableTimer;
        public readonly Timer Timer;

        public TimekeepingModel()
        {
            Timer = new Timer(TickInterval.TotalMilliseconds) { AutoReset = true };
            ObservableTimer = Observable.FromEventPattern<ElapsedEventHandler, ElapsedEventArgs>(
                h => Timer.Elapsed += h,
                h => Timer.Elapsed -= h).Select(t => t.EventArgs.SignalTime);
            ObservableTimer.Subscribe(_ => DoTick(TimeStep));
            Timer.Enabled = true;

            this.WhenAny(x => x.TickInterval, x => x.TimeStep1Second, (_, _) =>
            {
                TimeStep = TimeStep1Second * TickInterval.TotalSeconds;
                return Timer.Interval = TickInterval.TotalMilliseconds;
            }).Subscribe();
        }

        private void DoTick(TimeSpan timeSpan)
        {
            // Forward current time with the timespan
            CurrentTime = CurrentTime.Add(timeSpan);

            // Download any autorefresh stocks
            if (User.AutoRefresh)
            {
                foreach (var autoRefreshable in User.AutoRefreshList)
                {
                    if (!(DateTimeOffset.Now.Subtract(autoRefreshable.LastDownload).TotalMinutes >
                          autoRefreshable.RefreshInterval.TotalMinutes)) continue;
                    autoRefreshable.Download();
                    CurrentDatabase.ImportTestData();
                }
            }

            // TODO: All code that needs to be updated every tick

            foreach (Stock stock in CachedStocks)
            {
                stock.UpdateToTime(CurrentTime);
            }

            // Update all Investments returns and calculate based on algorithms the strategies for the next tick
            foreach (Investment investment in User.UserInvestmentPortfolio)
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
        public DateTimeOffset CurrentTime { get; set; } = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(50));

        /// <summary>
        /// The speed at which time is moving (How often is a tick?). 1 second is realtime speed (one step per second)
        /// </summary>
        [Reactive]
        public TimeSpan TickInterval { get; set; } = TimeSpan.FromSeconds(1);


        /// <summary>
        /// Actual time step every timer update, adjusted for update frequency.
        /// </summary>
        [Reactive]
        public TimeSpan TimeStep { get; private set; }

        /// <summary>
        /// Time step every second, how many time passes every second.
        /// </summary>
        [Reactive]
        public TimeSpan TimeStep1Second { get; set; } = TimeSpan.FromDays(1);
    }
}