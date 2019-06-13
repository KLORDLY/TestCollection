using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AutoRestEventTest
{
    public class Reader
    {
        const int numIterations = 50;
        AutoResetEvent myResetEvent = new AutoResetEvent(false);
        AutoResetEvent ChangeEvent = new AutoResetEvent(false);
        //ManualResetEvent myResetEvent = new ManualResetEvent(false);
        //ManualResetEvent ChangeEvent = new ManualResetEvent(false);
        int number; //这是关键资源
        public string textToShow;

        public void MainProc()
        {
            Thread payMoneyThread = new Thread(new ThreadStart(PayMoneyProc));
            payMoneyThread.Name = "付钱线程";
            Thread getBookThread = new Thread(new ThreadStart(GetBookProc));
            getBookThread.Name = "取书线程";
            payMoneyThread.Start();
            getBookThread.Start();

            for (int i = 1; i <= numIterations; i++)
            {
                textToShow += string.Format("买书线程：数量{0}\r\n", i);
                number = i;
                myResetEvent.Set();
                Thread.Sleep(50);
            }
            payMoneyThread.Abort();
            getBookThread.Abort();
        }

        void PayMoneyProc()
        {
            while (true)
            {
                myResetEvent.WaitOne();
                myResetEvent.Reset();
                textToShow += string.Format("{0}：数量{1}\r\n", Thread.CurrentThread.Name, number);
                ChangeEvent.Set();
            }
        }
        void GetBookProc()
        {
            while (true)
            {
                ChangeEvent.WaitOne();
                ChangeEvent.Reset();
                textToShow += string.Format("{0}：数量{1}\r\n", Thread.CurrentThread.Name, number);
                textToShow += string.Format("------------------------------------------\r\n");
            }
        }
    }
}
