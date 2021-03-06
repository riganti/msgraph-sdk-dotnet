// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

// **NOTE** This file was generated by a tool and any changes will be overwritten.

// Template Source: Templates\CSharp\Model\ComplexType.cs.tt

namespace Microsoft.Graph
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// The type Shared.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [JsonConverter(typeof(DerivedTypeConverter))]
    public partial class Shared
    {
    
        /// <summary>
        /// Gets or sets owner.
        /// The identity of the owner of the shared item. Read-only.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "owner", Required = Newtonsoft.Json.Required.Default)]
        public IdentitySet Owner { get; set; }
    
        /// <summary>
        /// Gets or sets scope.
        /// Indicates the scope of how the item is shared: anonymous, organization, or users. Read-only.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "scope", Required = Newtonsoft.Json.Required.Default)]
        public string Scope { get; set; }
    
        /// <summary>
        /// Gets or sets sharedBy.
        /// The identity of the user who shared the item. Read-only.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "sharedBy", Required = Newtonsoft.Json.Required.Default)]
        public IdentitySet SharedBy { get; set; }
    
        /// <summary>
        /// Gets or sets sharedDateTime.
        /// The UTC date and time when the item was shared. Read-only.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "sharedDateTime", Required = Newtonsoft.Json.Required.Default)]
        public DateTimeOffset? SharedDateTime { get; set; }
    
        /// <summary>
        /// Gets or sets additional data.
        /// </summary>
        [JsonExtensionData(ReadData = true)]
        public IDictionary<string, object> AdditionalData { get; set; }
    
    }
}
