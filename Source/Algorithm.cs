using System;
using System.Collections.Generic;
using System.Linq;
using GSF;
using GSF.Diagnostics;
using HistorianDataWalker.HistorianAPI;
using HistorianDataWalker.HistorianAPI.Metadata;
using System.Runtime.CompilerServices;

namespace HistorianDataWalker
{
    /// <summary>
    /// Defines algorithm to be executed during historian read.
    /// </summary>
    public class Algorithm : IDisposable
    {
        #region [ Members ]

        // Constants
        //private const string OutputFile = "Output for {0}.txt";

        // Meta-data fields
        private List<MetadataRecord> m_metadata;

        // TODO: Define your output file and/or other state variables here...
        //private StreamWriter m_ouputFile;

        // Algorithm processing statistic fields
        private long m_missingPointCount;
        private long m_nanPointCount;
        private long m_processedDataBlocks;
        private long m_receivedPoints;
        private DateTime m_lastTimestamp;
        private readonly Dictionary<ulong, long> m_pointCounts;
        private readonly Dictionary<ulong, long> m_nanCounts;

        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        public Algorithm()
        {
            m_lastTimestamp = new DateTime(0L);
            m_pointCounts = new Dictionary<ulong, long>();
            m_nanCounts = new Dictionary<ulong, long>();
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets UI message display function.
        /// </summary>
        public Action<string> ShowMessage { get; set; }

        /// <summary>
        /// Gets or sets current message display interval.
        /// </summary>
        public int MessageInterval { get; set; }

        /// <summary>
        /// Gets or sets current logging publisher.
        /// </summary>
        public LogPublisher Log { get; set; }

        /// <summary>
        /// Gets or sets start time for data read.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets end time for data read.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets total time-range, in seconds, for data read.
        /// </summary>
        public double TimeRange { get; set; }

        /// <summary>
        /// Gets or sets the expected frames per second.
        /// </summary>
        public int FrameRate { get; set; }

        /// <summary>
        /// Gets or sets received historian meta-data.
        /// </summary>
        public List<MetadataRecord> Metadata
        {
            get
            {
                return m_metadata;
            }
            set
            {
                // Cache meta-data in case algorithm needs it
                m_metadata = value;

                // Initialize counter dictionaries
                m_pointCounts.Clear();
                m_nanCounts.Clear();

                foreach (MetadataRecord record in m_metadata)
                {
                    m_pointCounts[record.PointID] = 0L;
                    m_nanCounts[record.PointID] = 0L;
                }

                // TODO: Can scan metadata here to determine "type of points", e.g.:

                // Load point ID for desired signal types
                //m_voltageMagnitudes = m_metadata
                //    .Where(record => record.SignalAcronym == "VPHM") // Load point IDs for all Voltage Phase Magnitudes
                //    .Select(record => record.PointID);

                //m_currentMagnitudes = m_metadata
                //    .Where(record => record.SignalAcronym == "IPHM") // Load point IDs for all Current Phase Magnitudes
                //    .Select(record => record.PointID);

                // May want to use a dictionary or hashset for fast lookup...
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Releases all the resources used by the <see cref="Algorithm"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Algorithm"/> object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                try
                {
                    if (disposing)
                    {
                        // TODO: Close/dispose any open ouput files here
                        //m_outputFile.Dispose();
                    }
                }
                finally
                {
                    m_disposed = true;  // Prevent duplicate dispose.
                }
            }
        }

        /// <summary>
        /// Notifies algorithm that it should prepare to receive data.
        /// </summary>
        public void Initialize()
        {
            // TODO: Open any output files here, can also write initial headers, etc...
            //m_outputFile = File.CreateText(FilePath.GetAbsolutePath(string.Format(OutputFile, "My Summary")));
            //m_outputFile.WriteLine($"This file captures any instantaneous imbalances exceeding {ImbalanceThreshold:0.00%}");
            //m_outputFile.WriteLine($"Details cover data from {StartTime:yyyy-MM-dd HH:mm:ss.fff} to {EndTime:yyyy-MM-dd HH:mm:ss.fff} spanning {Time.ToElapsedTimeString(TimeRange, 3).ToLowerInvariant()}{Environment.NewLine}");
        }

        // TODO: You can define helper functions to "extract" the desired data points for a given signal type:
        //private DataPoint[] GetDataPoints(DataPoint[] dataBlock, ulong[] voltageMagnitudeIDs)
        //{
        //    DataPoint[] dataPoints = new DataPoint[voltageMagnitudeIDs.Length];

        //    for (int i = 0; i < voltageMagnitudeIDs.Length; i++)
        //        dataPoints[i] = dataBlock.FirstOrDefault(point => point.PointID == voltageMagnitudeIDs[i]);

        //    return dataPoints;
        //}

        /// <summary>
        /// Allows algorithm to augment point ID list provided by UI.
        /// </summary>
        /// <param name="pointIDList">Point ID list.</param>
        public void AugmentPointIDList(List<ulong> pointIDList)
        {
            // TODO: If algorithm desires, it can completely ignore user interface and hard code desired points to process and/or augment the points to process here...
            //pointIDList.Clear();
            //pointIDList.AddRange(new ulong[] { 12, 16, 25, 19 });
        }

        /// <summary>
        /// Default data processing entry point for <see cref="Algorithm"/>.
        /// </summary>
        /// <param name="timestamp">Timestamp of <paramref name="dataBlock"/>.</param>
        /// <param name="dataBlock">Points values read at current timestamp.</param>
        public void Execute(DateTime timestamp, DataPoint[] dataBlock)
        {
            //ShowMessage($"{timestamp:yyyy-MM-dd HH:mm:ss.fff} - point count = {dataBlock.Length}");

            // TODO: Start processing data points per timestamp here...

            // Example code:
            foreach (DataPoint point in dataBlock)
            {
                // Count total values per incoming data point
                m_pointCounts[point.PointID] = m_pointCounts[point.PointID] + 1;

                // Count values per incoming data point that are NaN
                if (float.IsNaN(point.ValueAsSingle))
                {
                    m_nanPointCount++;
                    m_nanCounts[point.PointID] = m_nanCounts[point.PointID] + 1;
                }
            }

            TrackStatistics(timestamp, dataBlock.Length);
        }

        /// <summary>
        /// Notifies algorithm that data read is complete.
        /// </summary>
        public void Complete()
        {
            ShowMessage($"The following points had missing values:{Environment.NewLine}");

            long expectedPoints = (long)(TimeRange * FrameRate);
            int pointsWithMissingValues = 0;

            foreach (KeyValuePair<ulong, long> item in m_pointCounts)
            {
                MetadataRecord record = Metadata.FirstOrDefault(metadata => metadata.PointID == item.Key);

                if ((object)record != null && item.Value < expectedPoints)
                {
                    ShowMessage($"     [{record.PointTag}] = {item.Value:N0} total out of {expectedPoints:N0} expected: {item.Value / (double)expectedPoints * 100.0D:N3}%");
                    pointsWithMissingValues++;
                }
            }
            
            ShowMessage($"{Environment.NewLine}Total points with missing values: {pointsWithMissingValues:N0}{Environment.NewLine}");

            ShowMessage($"The following points had NaN values:{Environment.NewLine}");

            int pointsWithNanValues = 0;

            foreach (KeyValuePair<ulong, long> item in m_nanCounts)
            {
                MetadataRecord record = Metadata.FirstOrDefault(metadata => metadata.PointID == item.Key);

                if ((object)record != null && item.Value > 0L)
                {
                    ShowMessage($"     [{record.PointTag}] = {item.Value:N0} total out of {expectedPoints:N0} expected: {item.Value / (double)expectedPoints * 100.0D:N3}%");
                    pointsWithNanValues++;
                }
            }

            ShowMessage($"{Environment.NewLine}Total points with NaN values: {pointsWithNanValues:N0}{Environment.NewLine}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void TrackStatistics(DateTime timestamp, int pointCount)
        {
            // Make sure last timestamp is initialized
            if (m_lastTimestamp.Ticks == 0L)
                m_lastTimestamp = timestamp;

            DateTime currentSecond = timestamp.BaselinedTimestamp(BaselineTimeInterval.Second);
            DateTime lastSecond = m_lastTimestamp.BaselinedTimestamp(BaselineTimeInterval.Second);

            if (currentSecond > lastSecond)
            {
                long expectedPoints = (long)((currentSecond - m_lastTimestamp).TotalSeconds * FrameRate * Metadata.Count);
                m_missingPointCount += expectedPoints - m_receivedPoints;

                m_lastTimestamp = currentSecond;
                m_receivedPoints = 0;
            }

            m_receivedPoints += pointCount;

            if (++m_processedDataBlocks % MessageInterval == 0)
                ShowMessage($"Analyzed {m_processedDataBlocks:N0} timestamps so far with {m_missingPointCount:N0} missing points and {m_nanPointCount:N0} NaN points.{Environment.NewLine}");
        }

        #endregion
    }
}
