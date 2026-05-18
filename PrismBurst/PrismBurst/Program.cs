using PrismBurst.Scenes;

namespace PrismBurst
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.Title = "Prism Burst";

            Console.SetWindowSize(130, 45);

            Console.SetBufferSize(130, 45);

            Console.CursorVisible = false;

            TitleScene titleScene = new TitleScene();

            titleScene.Run();
        }
    }
}