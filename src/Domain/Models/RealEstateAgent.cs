using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class RealEstateAgent
    {
        public RealEstateAgent(int id, string name, IEnumerable<Guid> offerIds)
        {
            Id = id;
            Name = name;
            Offers = offerIds;
        }

        /// <summary>
        /// The unique identifier of a real estate agent.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The name of the real estate agent.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The unique identifiers of the offers for this real estate agent.
        /// </summary>
        public IEnumerable<Guid> Offers { get; private set; }
    }
}
