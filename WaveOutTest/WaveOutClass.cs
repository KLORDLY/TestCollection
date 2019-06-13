using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WaveLib;

namespace WaveOutTest
{
    public class WaveOutClass
    {
        IntPtr hWaveOut;
        public bool playSounds(string soundFileName)
        {
            if (string.IsNullOrEmpty(soundFileName))
            {
                return false;
            }
            if (!InitAudioDevice())
            {
                return false;
            }
            byte[] block;
            int blockSize;
            //WaveNative.waveOutOpen(out m_WaveOut, device, format, m_BufferProc, 0, WaveNative.CALLBACK_FUNCTION)
            //new WaveLib.WaveOutPlayer(-1, fmt, 16384, 3, new WaveLib.BufferFillEventHandler(Filler));
            if (WaveNative.waveOutOpen(out hWaveOut, -1, waveForm, null, 0, 0) != WaveNative.MMSYSERR_NOERROR)
            {
                //::AfxMessageBox("音频设备打卡失败",MB_OK);  
                return false;
            }

            if ((block = loadAudioBlock(soundFileName, out blockSize)) == null)
            {
                //::AfxMessageBox("无法打开音频文件",MB_OK);  
                return false;
            }
            writeAudioBlock(hWaveOut, block, blockSize);
            WaveNative.waveOutClose(hWaveOut);
            return true;
        }

        WaveLib.WaveFormat waveForm = new WaveFormat(44100, 16, 2);
        bool InitAudioDevice()
        {
            try
            {
                waveForm.nSamplesPerSec = 44100; /* sample rate */
                waveForm.wBitsPerSample = 16; /* sample size */
                waveForm.nChannels = 2; /* channels*/
                waveForm.cbSize = 0; /* size of _extra_ info */
                waveForm.wFormatTag = (short)(WaveLib.WaveFormats.Pcm);//WAVE_FORMAT_PCM;  
                waveForm.nBlockAlign = (short)((waveForm.wBitsPerSample * waveForm.nChannels) >> 3);
                waveForm.nAvgBytesPerSec = waveForm.nBlockAlign * waveForm.nSamplesPerSec;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        byte[] loadAudioBlock(string filename, out int blockSize)
        {
            //HANDLE hFile = INVALID_HANDLE_VALUE;
            int size = 0;
            //if ((hFile = CreateFile(filename, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, 0, NULL)) == INVALID_HANDLE_VALUE)
            //{
            //    return NULL;
            //}
            FileStream fs = File.Open(filename, FileMode.Open);
            //do
            //{
            //    if ((size = GetFileSize(hFile, NULL)) == 0)
            //    {
            //        break;
            //    }
            //    if ((block = HeapAlloc(GetProcessHeap(), 0, size)) == NULL)
            //    {
            //        break;
            //    }
            //    ReadFile(hFile, block, size, &readBytes, NULL);
            //} while (0);
            //CloseHandle(hFile);

            byte[] block = new byte[fs.Length];
            size = fs.Read(block, 0, block.Length);
            blockSize = size;
            return block;
        }

        void writeAudioBlock(IntPtr hWaveOut, byte[] block, int size)
        {
            WaveNative.WaveHdr header = new WaveNative.WaveHdr();
            //ZeroMemory(&header, sizeof(WAVEHDR));
            header.dwBufferLength = size;
            GCHandle hObject = GCHandle.Alloc(block, GCHandleType.Pinned);
            header.lpData = hObject.AddrOfPinnedObject();
            if (hObject.IsAllocated)
                hObject.Free();
            WaveNative.waveOutPrepareHeader(hWaveOut, ref header, Marshal.SizeOf(header));
            WaveNative.waveOutWrite(hWaveOut, ref header, Marshal.SizeOf(header));
            Thread.Sleep(500);
            while (WaveNative.waveOutUnprepareHeader(hWaveOut, ref header, Marshal.SizeOf(header)) == WaveNative.MMSYSERR_NOERROR)
                Thread.Sleep(100);
        }

        public bool Pause()
        {
            if (WaveNative.waveOutPause(hWaveOut) != WaveNative.MMSYSERR_NOERROR)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool Restart()
        {
            if (WaveNative.waveOutRestart(hWaveOut) != WaveNative.MMSYSERR_NOERROR)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public bool GetPosition()
        {
            int lpInfo = 0;
            WaveNative.WaveHdr header = new WaveNative.WaveHdr();
            if (WaveNative.waveOutGetPosition(hWaveOut, out lpInfo, Marshal.SizeOf(header)) != WaveNative.MMSYSERR_NOERROR)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //public static extern int waveOutGetPosition(IntPtr hWaveOut, out int lpInfo, int uSize);
    }
}
