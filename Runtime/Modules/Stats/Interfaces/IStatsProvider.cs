using System.Collections.Generic;

namespace Devdog.InventoryPro
{
    public interface IStatsProvider
    {
        /// <summary>
        ///     Set the categories and properties, does not calculate anything.
        /// </summary>
        Dictionary<string, List<IStat>> Prepare();
    }
}