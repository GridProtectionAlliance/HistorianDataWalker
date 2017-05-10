﻿//******************************************************************************************************
//  Settings.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  04/21/2017 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.Configuration;
using ExpressionEvaluator;
using GSF.ComponentModel;
using GSF.Configuration;

namespace HistorianDataWalker
{
    /// <summary>
    /// Defines settings for the Historian Data Walker application.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Default value expressions in this class reference the primary form instance, as a result,
    /// instances of this class should only be created from the primary UI thread or otherwise
    /// use <see cref="System.Windows.Forms.Form.Invoke(Delegate)"/>.
    /// </para>
    /// <para>
    /// In order for properties of this class decorated with <see cref="TypeConvertedValueExpressionAttribute"/>
    /// to have access to form element values, the elements should be declared with "public" access.
    /// </para>
    /// </remarks>
    public sealed class Settings : CategorizedSettingsBase<Settings>
    {
        /// <summary>
        /// Creates a new <see cref="Settings"/> instance.
        /// </summary>
        /// <param name="typeRegistry">
        /// Type registry to use when parsing <see cref="TypeConvertedValueExpressionAttribute"/> instances,
        /// or <c>null</c> to use <see cref="ValueExpressionParser.DefaultTypeRegistry"/>.
        /// </param>
        public Settings(TypeRegistry typeRegistry) : base("systemSettings", typeRegistry)
        {
        }

        /// <summary>
        /// Gets or sets host address for historian connection.
        /// </summary>
        [TypeConvertedValueExpression("Form.textBoxHistorianHostAddress.Text")]
        [Description("Host address for historian connection.")]
        [UserScopedSetting]
        public string HostAddress { get; set; }

        /// <summary>
        /// Gets or sets data port for historian connection.
        /// </summary>
        [TypeConvertedValueExpression("Form.maskedTextBoxHistorianDataPort.Text")]
        [Description("Data port for historian connection.")]
        [UserScopedSetting]
        public int DataPort { get; set; }

        /// <summary>
        /// Gets or sets meta-data port for historian connection.
        /// </summary>
        [TypeConvertedValueExpression("Form.maskedTextBoxHistorianMetadataPort.Text")]
        [Description("Meta-data port for historian connection.")]
        [UserScopedSetting]
        public int MetadataPort { get; set; }

        /// <summary>
        /// Gets or sets name of historian instance to access.
        /// </summary>
        [TypeConvertedValueExpression("Form.textBoxHistorianInstanceName.Text")]
        [Description("Name of historian instance to access.")]
        [UserScopedSetting]
        public string InstanceName { get; set; }

        /// <summary>
        /// Gets or sets frame-rate, in frames per second, used to estimate total data for timespan.
        /// </summary>
        [TypeConvertedValueExpression("Form.maskedTextBoxFrameRate.Text")]
        [Description("Frame rate, in frames per second, used to estimate total data for timespan.")]
        [UserScopedSetting]
        public int FrameRate { get; set; }

        /// <summary>
        /// Gets or sets meta-data retriever timeout, in milliseconds.
        /// </summary>
        [UserScopedSetting]
        [Description("Meta-data retriever timeout, in milliseconds.")]
        public int MetadataTimeout { get; set; }

        /// <summary>
        /// Gets or sets meta-data retriever timeout, in seconds.
        /// </summary>
        [TypeConvertedValueExpression("Form.maskedTextBoxMetadataTimeout.Text")]
        [SerializeSetting(false)] // <-- Do not synchronize to config file
        public int MetadataTimeoutSeconds
        {
            get
            {
                return MetadataTimeout / 1000;
            }
            set
            {
                MetadataTimeout = value * 1000;
            }
        }

        /// <summary>
        /// Gets or sets selected start time range for historian read.
        /// </summary>
        [TypeConvertedValueExpression("Form.dateTimePickerSourceTime.Value")]
        [Description("Selected start time range for historian read.")]
        [UserScopedSetting]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets selected end time range for historian read.
        /// </summary>
        [TypeConvertedValueExpression("Form.dateTimePickerEndTime.Value")]
        [Description("Selected end time range for historian read.")]
        [UserScopedSetting]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets selected point list or filter expression for historian read.
        /// </summary>
        [TypeConvertedValueExpression("Form.textBoxPointList.Text")]
        [Description("Selected point list or filter expression for historian read.")]
        [UserScopedSetting]
        public string PointList { get; set; }

        /// <summary>
        /// Gets or sets message display interval.
        /// </summary>
        [TypeConvertedValueExpression("Form.maskedTextBoxMessageInterval.Text")]
        [Description("Message display interval.")]
        [UserScopedSetting]
        public int MessageInterval { get; set; }

        /// <summary>
        /// Gets or sets flag that determines if logging should be enabled.
        /// </summary>
        [TypeConvertedValueExpression("Form.checkBoxEnableLogging.Checked")]
        [Description("Flag that determines if logging should be enabled.")]
        [UserScopedSetting]
        public bool EnableLogging { get; set; }
    }
}
