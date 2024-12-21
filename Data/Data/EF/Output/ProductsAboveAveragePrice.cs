using System;
using System.Collections.Generic;

namespace Data.Data.EF.Output;

public partial class ProductsAboveAveragePrice
{
    public string ProductName { get; set; } = null!;

    public decimal? UnitPrice { get; set; }
}
