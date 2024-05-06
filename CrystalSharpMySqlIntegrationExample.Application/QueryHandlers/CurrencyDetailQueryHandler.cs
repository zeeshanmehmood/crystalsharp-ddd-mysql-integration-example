﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CrystalSharp.Application;
using CrystalSharp.Application.Handlers;
using CrystalSharp.Domain;
using CrystalSharpMySqlIntegrationExample.Application.Domain.Aggregates.CurrencyAggregate;
using CrystalSharpMySqlIntegrationExample.Application.Infrastructure;
using CrystalSharpMySqlIntegrationExample.Application.Queries;
using CrystalSharpMySqlIntegrationExample.Application.ReadModels;

namespace CrystalSharpMySqlIntegrationExample.Application.QueryHandlers
{
    public class CurrencyDetailQueryHandler : QueryHandler<CurrencyDetailQuery, CurrencyReadModel>
    {
        private readonly AppDbContext _dbContext;

        public CurrencyDetailQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<QueryExecutionResult<CurrencyReadModel>> Handle(CurrencyDetailQuery request, CancellationToken cancellationToken = default)
        {
            if (request == null) return await Fail("Invalid query.");

            Currency currency = await _dbContext.Currency.SingleOrDefaultAsync(x =>
                x.GlobalUId == request.GlobalUId
                && x.EntityStatus == EntityStatus.Active,
                cancellationToken)
                .ConfigureAwait(false);

            if (currency == null)
            {
                return await Fail("Currency not found.");
            }

            CurrencyReadModel readModel = new() { GlobalUId = currency.GlobalUId, Name = currency.Name };

            return await Ok(readModel);
        }
    }
}
