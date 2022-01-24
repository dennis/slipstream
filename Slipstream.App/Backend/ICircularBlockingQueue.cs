namespace Slipstream.Backend
{
    public interface ICircularBlockingQueue<T> where T : class
    {
        void Enqueue(T item);

        T? Dequeue(int millisecondsTimeout);

        ICircularBlockingQueue<T> Clone();
    }
}