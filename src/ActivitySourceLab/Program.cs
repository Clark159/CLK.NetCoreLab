using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ActivitySourceLab
{
    public class Program
    {
        // Fields
        private static ActivitySource _activitySource = new ActivitySource("CLK.ActivitySourceLab.MainModule");


        // Methods
        public static void Main(string[] args)
        {
            // Subscribe
            ActivitySource.AddActivityListener(new ActivitySourceSubscriber("CLK.ActivitySourceLab.MainModule").ActivityListener);
            
            // Diagnostic
            {
                // Pay
                using (var payActivity = _activitySource.StartActivity("Pay"))
                {
                    // Pay.SetTag
                    payActivity?.SetTag("User", "Clark");

                    // Print
                    using (var printActivity = _activitySource.StartActivity("Print"))
                    {
                        // Print.SetTag
                        printActivity?.SetTag("User", "Jane");
                    }
                }
            }
        }


        // Class
        public class ActivitySourceSubscriber
        {
            // Fields
            private readonly string _activitySourceName = null;

            private readonly ActivityExporter _activityExporter = new ActivityExporter();


            // Constructors
            public ActivitySourceSubscriber(string activitySourceName)
            {
                #region Contracts

                if (string.IsNullOrEmpty(activitySourceName) == true) throw new ArgumentException(nameof(activitySourceName));

                #endregion

                // Default
                _activitySourceName = activitySourceName;

                // ActivityListener
                this.ActivityListener = new ActivityListener();
                this.ActivityListener.ShouldListenTo = this.ShouldListenTo;
                this.ActivityListener.Sample = this.Sample;
                this.ActivityListener.SampleUsingParentId = this.SampleUsingParentId;
                this.ActivityListener.ActivityStarted = _activityExporter.ActivityStarted;
                this.ActivityListener.ActivityStopped = _activityExporter.ActivityStopped;
            }


            // Properties
            public ActivityListener ActivityListener { get; private set; }


            // Methods
            public bool ShouldListenTo(ActivitySource activitySource)
            {
                #region Contracts

                if (activitySource == null) throw new ArgumentException(nameof(activitySource));

                #endregion

                // Return
                return activitySource.Name == _activitySourceName;
            }

            public ActivitySamplingResult Sample(ref ActivityCreationOptions<ActivityContext> options)
            {
                // Return
                return ActivitySamplingResult.AllData;
            }

            public ActivitySamplingResult SampleUsingParentId(ref ActivityCreationOptions<string> options)
            {
                // Return
                return ActivitySamplingResult.AllData;
            }
        }

        public class ActivityExporter
        {
            // Methods
            public void ActivityStarted(Activity activity)
            {
                #region Contracts

                if (activity == null) throw new ArgumentException(nameof(activity));

                #endregion

                // Nothing

            }

            public void ActivityStopped(Activity activity)
            {
                #region Contracts

                if (activity == null) throw new ArgumentException(nameof(activity));

                #endregion

                // Display
                Console.WriteLine($"Activity.Id: {activity.Id}");
                Console.WriteLine($"Activity.ParentId: {activity.ParentId}");
                Console.WriteLine($"Activity.TraceId: {activity.TraceId}");
                Console.WriteLine($"Activity.DisplayName: {activity.DisplayName}");
                Console.WriteLine($"Activity.Kind: {activity.Kind}");
                Console.WriteLine($"Activity.StartTime: {activity.StartTimeUtc}");
                Console.WriteLine($"Activity.Duration: {activity.Duration}");
                Console.WriteLine($"Activity.TagObjects:");
                foreach (var tag in activity.TagObjects)
                {
                    Console.WriteLine($"    {tag.Key}: {tag.Value}");
                }
                Console.WriteLine();
            }
        }
    }
}
