﻿using System.Threading.Tasks;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.Data.Products
{
    public interface IProductsRepository
    {
        Task<AsyncResult> UpdatePrice(ProductPriceUpdate[] prices);
    }
}