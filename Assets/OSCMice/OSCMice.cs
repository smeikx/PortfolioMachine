using UnityEngine;
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using VVVV_OSC;

public class OSCMice : MonoBehaviour
{
	[SerializeField] int port = 8010;

	Thread thread;
	OSCReceiver oscin;

	[HideInInspector] public Vector2 mouse_1;
	[HideInInspector]	public Vector2 mouse_2;


	void Start()
	{
		oscin = new OSCReceiver(port);
		thread = new Thread(new ThreadStart(UpdateOSC));
		thread.Start();
	}

	void OnDestroy()
	{
		if (oscin != null)
			oscin.Close();
		if (thread != null)
		{
			thread.Interrupt();
			if (!thread.Join(2000))
			{ 
				thread.Abort();
			}
		}
	}


	void UpdateOSC()
	{
		while (true)
		{
			OSCPacket msg = oscin.Receive();
			if (msg != null)
			{
				if (msg.IsBundle())
				{
					OSCBundle b = (OSCBundle)msg;
					foreach (OSCPacket subm in b.Values)
					{
						parseMessage(subm);
					}
				}
				else
				{
					parseMessage(msg);
				}
			}
			Thread.Sleep(5);
		}
	}


	void parseMessage(OSCPacket msg)
	{ 
		Debug.Log("message with address: " + msg.Address);
		// get a value:
		mouse_1.x = (float)msg.Values[0];
		mouse_1.y = (float)msg.Values[1];
		mouse_2.x = (float)msg.Values[2];
		mouse_2.y = (float)msg.Values[3];
	}
}
