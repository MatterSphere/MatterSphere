using System.Collections.Generic;
using System.Dynamic;
using MSIndex.Common.Interfaces;
using MSIndex.Common.Models;

namespace MSIndex.Common.Handlers
{
    public class AppointmentHandler : EntityHandler
    {
        public AppointmentHandler(IDbProvider dbProvider, IMapper mapper) : base(dbProvider, mapper)
        {
        }

        public override void Index(List<ExpandoObject> items)
        {
            var appointments = Map<MSAppointment>(items);
            _dbProvider.Index(appointments);
        }
    }
}
