using System;
using System.Collections.Generic;
using System.Linq;
using GSF.Diagnostics;
using HistorianDataWalker.HistorianAPI;
using HistorianDataWalker.HistorianAPI.Metadata;

namespace HistorianDataWalker
{
    /// <summary>
    /// Defines algorithm to be executed during historian read.
    /// </summary>
    public class Algorithm
    {
        #region [ Members ]

        // Meta-data fields
        private List<MetadataRecord> m_metadata;
        private ulong[] m_voltageMagnitudes;
        private ulong[] m_currentMagnitudes;

        // Algorithm analytic fields
        private long m_voltageMagnitudeTotal;
        private double m_voltageMagnitudeSum;
        private long m_currentMagnitudeTotal;
        private double m_currentMagnitudeSum;

        // Algorithm processing statistic fields
        private long m_unexpectedPointCount;
        private long m_processedDataBlocks;

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

                if ((object)m_metadata == null)
                {
                    m_voltageMagnitudes = new ulong[0];
                    m_currentMagnitudes = new ulong[0];
                }
                else
                {
                    // Load point ID for desired signal types
                    m_voltageMagnitudes = m_metadata
                        .Where(record => record.SignalAcronym == "VPHM") // Load point IDs for all Voltage Phase Magnitudes
                        .Select(record => record.PointID)
                        .ToArray();

                    m_currentMagnitudes = m_metadata
                        .Where(record => record.SignalAcronym == "IPHM") // Load point IDs for all Current Phase Magnitudes
                        .Select(record => record.PointID)
                        .ToArray();
                }

                // Sort point ID arrays to support fast binary search
                Array.Sort(m_voltageMagnitudes);
                Array.Sort(m_currentMagnitudes);
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Default data processing entry point for <see cref="Algorithm"/>.
        /// </summary>
        /// <param name="timestamp">Timestamp of <paramref name="dataBlock"/>.</param>
        /// <param name="dataBlock">Points values read at current timestamp.</param>
        public void Execute(DateTime timestamp, DataPoint[] dataBlock)
        {
            foreach (DataPoint point in dataBlock)
            {
                if (Array.BinarySearch(m_voltageMagnitudes, point.PointID) >= 0)
                {
                    // Found a voltage
                    m_voltageMagnitudeTotal++;
                    m_voltageMagnitudeSum += point.ValueAsSingle;
                }
                else if (Array.BinarySearch(m_currentMagnitudes, point.PointID) >= 0)
                {
                    // Found a current
                    m_currentMagnitudeTotal++;
                    m_currentMagnitudeSum += point.ValueAsSingle;
                }
                else
                {
                    m_unexpectedPointCount++;
                }
            }

            if (++m_processedDataBlocks % MessageInterval == 0)
            {
                string message = $"Analyzed {m_processedDataBlocks:N0} timestamps so far with {m_unexpectedPointCount} unexpected points.{Environment.NewLine}" +
                                 $"    >> Voltage Average = {m_voltageMagnitudeSum / m_voltageMagnitudeTotal:N3}{Environment.NewLine}" +
                                 $"    >> Current Average = {m_currentMagnitudeSum / m_currentMagnitudeTotal:N3}";

                ShowMessage(message);
            }
        }

        #endregion
    }
}
