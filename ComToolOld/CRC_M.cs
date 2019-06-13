using System;
using System.Collections.Generic;
using System.Text;

namespace ComToolOld
{
    /// <summary>
    /// Modbus RTU Message CRC
    /// </summary>
    public class CRC_M
    {
        public static ushort CRC16(byte[] buffer, int size)
        {
            ushort crc = 0xFFFF;

            for (int pos = 0; pos < size; pos++)
            {
                crc ^= (UInt16)buffer[pos];          // XOR byte into least sig. byte of crc

                for (int i = 8; i != 0; i--)
                {    // Loop over each bit
                    if ((crc & 0x0001) != 0)
                    {      // If the LSB is set
                        crc >>= 1;                    // Shift right and XOR 0xA001
                        crc ^= 0xA001;
                    }
                    else                            // Else LSB is not set
                        crc >>= 1;                    // Just shift right
                }
            }
            // Note, this number has low and high bytes swapped, so use it accordingly (or swap bytes)
            return crc;
        }
    }
}
