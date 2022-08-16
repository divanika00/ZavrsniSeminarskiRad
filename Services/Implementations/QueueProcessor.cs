using ZavrsniSeminarskiRad.Services.Interfaces;

namespace ZavrsniSeminarskiRad.Services.Implementations
{
    public class QueueProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;

        public QueueProcessor(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;

        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var productService = scope.ServiceProvider.GetService<IItemService>();
                    if (productService != null)
                    {

                        await productService.UpdateShoppinBasketStatus();
                    }



                }
                await Task.Delay(100000, stoppingToken);
            }
        }

    }
}
