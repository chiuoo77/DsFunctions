namespace DsFunctions.Commons
{
    public class DsLogEvent
    {
        public static void WriteEventLogEntry(System.Diagnostics.EventLogEntryType LogType, string message, string AppName = "Ds")
        {
            // Create an instance of EventLog
            System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog();

            // Check if the event source exists. If not create it.
            if (!System.Diagnostics.EventLog.SourceExists(AppName))
            {
                System.Diagnostics.EventLog.CreateEventSource(AppName, "Application");
            }

            // Set the source name for writing log entries.
            eventLog.Source = AppName;

            // Create an event ID to add to the event log
            // 에러코드를 정의해서 넣을 수도 있을 것 같다. 
            int eventID = 8;

            // Write an entry to the event log.
            eventLog.WriteEntry(message, LogType, eventID, 10);

            // Close the Event Log
            eventLog.Close();
        }
    }
}
