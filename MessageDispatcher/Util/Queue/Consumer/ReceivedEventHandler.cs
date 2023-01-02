namespace Util.Queue.Consumer
{
    public delegate Task<bool> ReceivedEventHandler(string model);
}