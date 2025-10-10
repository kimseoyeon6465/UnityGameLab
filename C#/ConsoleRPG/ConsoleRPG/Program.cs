namespace ConsoleRPG
{
    internal class Program
    {
        // 정적 -> 정적 메모리 
        // 스택, 힙, 코드, 데이터
        // 메인 -> 운영체제 
        static void Main(string[] args)
        {
            MainLogic mainLogic = new MainLogic();
            mainLogic.Run();
        }

    }
}
