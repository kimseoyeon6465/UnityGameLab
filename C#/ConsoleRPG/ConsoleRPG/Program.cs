using System;

namespace ConsoleRPG
{
    internal class Program
    {
        // 정적 -> 정적 메모리 
        // 스택, 힙, 코드, 데이터
        // 메인 -> 운영체제 
        static void Main(string[] args)
        {
            
            var game = new MainLogic();
            //var는 컴파일러가 자료형을 추론하게 함, 코드 간결성 up, 반드시 초기화
            game.Run();
        }

    }
}
