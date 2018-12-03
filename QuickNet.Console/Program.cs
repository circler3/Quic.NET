﻿using QuickNet.Utilities;
using QuicNet;
using QuicNet.Infrastructure;
using QuicNet.Infrastructure.Frames;
using QuicNet.Infrastructure.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNet.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] bytes = new VariableInteger(12345);
            VariableInteger integer = bytes;
            UInt64 uinteger = integer;
            int size = VariableInteger.Size(bytes[0]);

            InitialPacket packet = new InitialPacket()
            {
                Version = 16,
                SourceConnectionId = 124,
                DestinationConnectionId = 0,
                PacketNumber = 777521,
                TokenLength = 0
            };
            ConnectionCloseFrame frame = new ConnectionCloseFrame(ErrorCode.SERVER_BUSY, "The server is too busy to process your request.");
            packet.AttachFrame(frame);

            byte[] data = packet.Encode();
            packet.Decode(data);

            byte[] ccfData = frame.Encode();
            frame.Decode(new ByteArray(ccfData));

            string b64 = ToBase64(data);

            StreamId streamId;
            byte[] streamIdData = StreamId.Encode(123, StreamType.ClientUnidirectional);
            streamId = StreamId.Decode(streamIdData);

            QuicListener listener = new QuicListener(11000);
            listener.Start();
        }

        static string ToBase64(byte[] data)
        {
            return Convert.ToBase64String(data);
        }
    }
}
