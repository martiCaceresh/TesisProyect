namespace OptiArroz_Tesis_Proyect.Models.Utils
{
    public static class StockSemaphore
    {
        public static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);
    }
}
