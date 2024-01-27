﻿using System;

namespace Common.Timeline.Changes
{
    /// <summary>
    /// Provides functions to convert between instances of IChange and SerializedChange.
    /// </summary>
    public static class ChangeExtensions
    {
        /// <summary>
        /// Returns a deserialized change.
        /// </summary>
        public static IChange Deserialize(this SerializedChange x)
        {
            var type = Registries.TypeRegistry.GetChangeType(x.ChangeType);
            if (type == null)
                throw new ChangeNotFoundException(x.ChangeType);

            var serializer = Services.ServiceLocator.Instance.GetService<Services.IJsonSerializer>();
            var data = serializer.Deserialize<IChange>(x.ChangeData, type, false);

            data.AggregateIdentifier = x.AggregateIdentifier;
            data.AggregateVersion = x.AggregateVersion;
            data.ChangeTime = x.ChangeTime;
            data.OriginOrganization = x.OriginOrganization;
            data.OriginUser = x.OriginUser;

            return data;
        }

        /// <summary>
        /// Returns a serialized change.
        /// </summary>
        public static SerializedChange Serialize(this IChange change, Guid aggregateIdentifier, int version)
        {
            var serializer = Services.ServiceLocator.Instance.GetService<Services.IJsonSerializer>();
            var data = serializer.Serialize(change, new[] { "AggregateIdentifier", "AggregateState", "AggregateVersion", "ChangeTime", "OriginOrganization", "OriginUser" }, false);

            var serialized = new SerializedChange
            {
                AggregateIdentifier = aggregateIdentifier,
                AggregateVersion = version,

                ChangeTime = change.ChangeTime,
                ChangeType = change.GetType().Name,
                ChangeData = data,

                OriginOrganization = change.OriginOrganization,
                OriginUser = change.OriginUser
            };

            change.OriginOrganization = serialized.OriginOrganization;
            change.OriginUser = serialized.OriginUser;

            return serialized;
        }
    }
}