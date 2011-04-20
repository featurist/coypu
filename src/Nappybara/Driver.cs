namespace Nappybara
{
    public interface Driver
    {
        Node FindButton(string locator);
        Node FindLink(string locator);
        void Click(Node node);
        void Visit(string url);
    }
}