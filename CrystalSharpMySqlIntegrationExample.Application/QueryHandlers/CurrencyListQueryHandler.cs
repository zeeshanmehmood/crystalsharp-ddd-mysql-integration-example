﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrystalSharp.Application;
using CrystalSharp.Application.Handlers;
using CrystalSharp.Domain;
using CrystalSharpMySqlIntegrationExample.Application.Domain.Aggregates.CurrencyAggregate;
using CrystalSharpMySqlIntegrationExample.Application.Infrastructure;
using CrystalSharpMySqlIntegrationExample.Application.Queries;
using CrystalSharpMySqlIntegrationExample.Application.ReadModels;

namespace CrystalSharpMySqlIntegrationExample.Application.QueryHandlers
{
    public class CurrencyListQueryHandler : QueryHandler<CurrencyListQuery, CurrencyReadModelList>
    {
        private readonly AppDbContext _dbContext;

        public CurrencyListQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<QueryExecutionResult<CurrencyReadModelList>> Handle(CurrencyListQuery request, CancellationToken cancellationToken = default)
        {
            if (request == null) return await Fail("Invalid query.");

            IQueryable<Currency> currencies = _dbContext.Currency.Where(x => x.EntityStatus == EntityStatus.Active);
            CurrencyReadModelList readModel = null;

            if (currencies != null && currencies.Any())
            {
                readModel = new CurrencyReadModelList
                {
                    Currencies = currencies.Select(x => new CurrencyReadModel { GlobalUId = x.GlobalUId, Name = x.Name })
                };
            }

            return await Ok(readModel);
        }
    }
}
