﻿using DocumentCrudService.Application.Services.Results;
using DocumentCrudService.Application.Services.Units;

namespace DocumentCrudService.Application.Services.Handlers
{
    public interface IQueryHandler<T> : IQueryHandler where T : IQuery
    {
        IList<IResult> Handle(T query);
    }
}
