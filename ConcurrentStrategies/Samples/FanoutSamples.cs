using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConcurrentStrategies.Things;

namespace ConcurrentStrategies.Samples
{
    internal class FanoutSamples
    {
        //Call two lookup tasks
        //Those lookup tasks should result in three gets
        //Those three gets need to feed into one save

        //        *    *
        //         \  /
        //          **
        //      /    |   \
        //     **   **   **
        //      \    |   /
        //           *

        internal async Task DoWorkAsync()
        {
            var inventory = GetInventoryRecordsAsync();
            var marketing = GetMarketingRecordsAsync();

            var prelookupFinished = Task.WhenAll(inventory, marketing);

            var builtInvoice = prelookupFinished.ContinueWith(prev =>
            {
                var inventoryResults = inventory.Result;
                var marketingResults = marketing.Result;

                return BuildInvoice(inventoryResults, marketingResults);
            });

            UpdateInventory(inventory);
        }

        private async Task<SalesRecord> BuildInvoice(InventoryRecord[] inventory, MarketingRecord[] marketing)
        {
            return await Task.Run(() => new SalesRecord());
        }

        private Task UpdateInventory(Task<InventoryRecord[]> records)
        {
            return Task.Run(() => "");
        }

        private Task UpdateMarketing(Task<MarketingRecord[]> records)
        {
            return Task.Run(() => "");
        }

        private async Task<InventoryRecord[]> GetInventoryRecordsAsync()
        {
            return await Task.Run(() => new[] { new InventoryRecord(), new InventoryRecord() });
        }

        private async Task<MarketingRecord[]> GetMarketingRecordsAsync()
        {
            return await Task.Run(() => new[] { new MarketingRecord(), new MarketingRecord() });
        }

        private class InventoryRecord { }
        private class MarketingRecord { }

        private class SalesRecord { }
    }
}
