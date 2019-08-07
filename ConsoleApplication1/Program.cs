using System;
using System.ComponentModel;
using System.Threading;
using System.Timers;

namespace ConsoleApplication1
{
    class Program
    {
        private static System.ComponentModel.BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        private static int _count = 0;

        //Timer 설정
        private static System.Timers.Timer timer = new System.Timers.Timer();

        static void Main(string[] args)
        {

            timer.Interval = 1000; //타이머 1초 단위로 실행
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed); 
            timer.Start(); //타이머 시작

            backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);

            //콘솔 종료 안되게 그냥 해 놓은거, 이거 없음바로 종료 됨
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }

        static void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("work number Start : {0} ", _count);
            e.Result = _count;


            // 취소 버튼 클릭 했을 경우
            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true; //작업취소
                return;
            }

            //실제 작업은 여기에....메소드 별도 만들어서 여기에 넣어도 되고..
            //비동기 병렬 작업 여기에 작성.
            //작업단위를 Task 배열에 담아서 Task.Start 하고 모든 작업이 끝날때 까지 기다리는  Task.Wait
            Thread.Sleep(5000);
            backgroundWorker1.ReportProgress(20); //작업 진행률 

            _count++;            
        }

        static void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //진행상태 모니터링
            //e.ProgressPercentage
        }

        static void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("work number  End : {0} ", e.Result);

            //5번 실행 이 끝나면 콘솔 프로그램 종료
            if (_count == 5)
            {
                timer.Stop();
                System.Environment.Exit(0);
            }

            if(backgroundWorker1.CancellationPending)
            {

            }
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //1초마다 타이머가 실행되고 있는거 확인
            Console.WriteLine("Timer Working {0} ", e.SignalTime.ToString());

            //백그라운드 워커 실행 중이냐고 물어 봄
            //타이머를 1초 단위로 해도 backgroundWorker1가 실행 중이면 skip함.
            if (!backgroundWorker1.IsBusy)
            {
                //작업중이지 않으면 backgroundwork  실행 시킴
                backgroundWorker1.RunWorkerAsync();
            }
            
        }
    }
}
