using System;
using MSIndex.Common.Handlers;
using MSIndex.Common.Interfaces;
using MSIndex.Common.Models;

namespace MSIndex.Common
{
    public class HandlerProvider : IHandlerProvider
    {
        private readonly IDbProvider _dbProvider;
        private readonly IMapper _mapper;

        public HandlerProvider(IDbProvider dbProvider, IMapper mapper)
        {
            _dbProvider = dbProvider;
            _mapper = mapper;
        }
        
        public EntityHandler GetHandler(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Address:
                    return new AddressHandler(_dbProvider, _mapper);
                case EntityType.Appointment:
                    return new AppointmentHandler(_dbProvider, _mapper);
                case EntityType.Associate:
                    return new AssociateHandler(_dbProvider, _mapper);
                case EntityType.Client:
                    return new ClientHandler(_dbProvider, _mapper);
                case EntityType.Contact:
                    return new ContactHandler(_dbProvider, _mapper);
                case EntityType.Document:
                    return new DocumentHandler(_dbProvider, _mapper);
                case EntityType.File:
                    return new FileHandler(_dbProvider, _mapper);
                case EntityType.Precedent:
                    return new PrecedentHandler(_dbProvider, _mapper);
                case EntityType.Task:
                    return new TaskHandler(_dbProvider, _mapper);
                case EntityType.User:
                    return new UserHandler(_dbProvider, _mapper);
            }

            throw new ArgumentException($"{entityType} is not an expected entity type");
        }
    }
}
